using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerBots
{
    class ItemManager : MonoBehaviour
    {
        public CharacterMaster master;
        private WeightedSelection<ChestTier> chestPicker;

        private ChestTier nextChestTier = ChestTier.WHITE;
        private int nextChestPrice = 25;
        private int purchases = 0;
        private int maxPurchases = 8;

        private Run.FixedTimeStamp lastBuyCheck;
        private float buyingDelay;

        // Sale Star item indices
        private ItemIndex saleStarItemIndex = ItemIndex.None;
        private ItemIndex saleStarConsumedItemIndex = ItemIndex.None;

        public void Awake()
        {
            this.master = base.gameObject.GetComponent<CharacterMaster>();
            this.maxPurchases = PlayerBotManager.MaxBotPurchasesPerStage.Value;

            this.chestPicker = new WeightedSelection<ChestTier>();
            this.chestPicker.AddChoice(ChestTier.WHITE, PlayerBotManager.Tier1ChestBotWeight.Value);
            this.chestPicker.AddChoice(ChestTier.GREEN, PlayerBotManager.Tier2ChestBotWeight.Value);
            this.chestPicker.AddChoice(ChestTier.RED, PlayerBotManager.Tier3ChestBotWeight.Value);

            // Initialize Sale Star item indices
            InitializeSaleStarItems();

            ResetPurchases();
            ResetBuyingDelay();
        }

        private void InitializeSaleStarItems()
        {
            // Try to find Sale Star items by common name patterns
            saleStarItemIndex = ItemCatalog.FindItemIndex("LowerPricedChests");
            if (saleStarItemIndex == ItemIndex.None)
            {
                saleStarItemIndex = ItemCatalog.FindItemIndex("SaleStar");
            }
            
            saleStarConsumedItemIndex = ItemCatalog.FindItemIndex("LowerPricedChestsConsumed");
            if (saleStarConsumedItemIndex == ItemIndex.None)
            {
                saleStarConsumedItemIndex = ItemCatalog.FindItemIndex("SaleStarConsumed");
            }
        }

        public void FixedUpdate()
        {
            if (this.lastBuyCheck.timeSince >= this.buyingDelay)
            {
                CheckBuy();
                ResetBuyingDelay();
            }
        }

        public void ResetPurchases()
        {
            this.ResetChest();
            this.maxPurchases = GetMaxPurchases();
        }

        private void ResetBuyingDelay()
        {
            this.lastBuyCheck = Run.FixedTimeStamp.now;
            this.buyingDelay = Random.Range(PlayerBotManager.MinBuyingDelay.Value, PlayerBotManager.MaxBuyingDelay.Value);
        }

        public void ResetChest()
        {
            this.nextChestTier = this.chestPicker.Evaluate(UnityEngine.Random.value);
            switch (this.nextChestTier)
            {
                case ChestTier.WHITE:
                    this.nextChestPrice = Run.instance.GetDifficultyScaledCost(PlayerBotManager.Tier1ChestBotCost.Value);
                    break;
                case ChestTier.GREEN:
                    this.nextChestPrice = Run.instance.GetDifficultyScaledCost(PlayerBotManager.Tier2ChestBotCost.Value);
                    break;
                case ChestTier.RED:
                    this.nextChestPrice = Run.instance.GetDifficultyScaledCost(PlayerBotManager.Tier3ChestBotCost.Value);
                    break;
            }
        }

        private int GetMaxPurchases()
        {
            return PlayerBotManager.MaxBotPurchasesPerStage.Value * (RoR2.Run.instance.stageClearCount + 1);
        }

        private void CheckBuy()
        {
            if (master.IsDeadAndOutOfLivesServer())
            {
                return;
            }

            // Only apply purchase cap to non-Robomando bots
            bool isRobomando = this.master.GetBody() && this.master.GetBody().bodyIndex == BodyCatalog.FindBodyIndex("RobomandoBody");
            if (!isRobomando && this.purchases >= this.maxPurchases)
            {
                return;
            }

            uint price = (uint)this.nextChestPrice;
            // Apply 40% discount for Robomando
            if (isRobomando)
            {
                price = (uint)Mathf.CeilToInt(this.nextChestPrice * 0.6f);
            }
            if (this.master.money >= price)
            {
                Buy(this.nextChestTier);
                this.master.money -= price;
                this.purchases++;
                ResetChest();
            }
        }

        private void Buy(ChestTier chestTier)
        {
            // Check for Sale Star and consume it if present
            bool hasSaleStar = HasActiveSaleStar();
            if (hasSaleStar)
            {
                TryConsumeSaleStar();
            }

            // Get drop list
            List<PickupIndex> dropList = null;
            switch (chestTier)
            {
                case ChestTier.WHITE:
                    if (this.master.inventory.currentEquipmentIndex == EquipmentIndex.None && PlayerBotManager.EquipmentBuyChance.Value > UnityEngine.Random.Range(0, 100))
                    {
                        dropList = Run.instance.availableEquipmentDropList;
                        break;
                    }
                    dropList = Run.instance.smallChestDropTierSelector.Evaluate(UnityEngine.Random.value);
                    break;
                case ChestTier.GREEN:
                    dropList = Run.instance.mediumChestDropTierSelector.Evaluate(UnityEngine.Random.value);
                    break;
                case ChestTier.RED:
                    dropList = Run.instance.largeChestDropTierSelector.Evaluate(UnityEngine.Random.value);
                    break;
            }

            // Pickup
            if (dropList != null && dropList.Count > 0)
            {
                List<PickupIndex> filteredDropList = dropList.Where((PickupIndex pickupIndex) => 
                {
                    if (IsScrapPickup(pickupIndex))
                        return false;
                        
                    PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                    if (pickupDef == null || pickupDef.itemIndex == ItemIndex.None)
                        return true; // Allow non-item pickups (equipment, etc.)
                        
                    return PlayerBotManager.IsItemAllowed(pickupDef.itemIndex);
                }).ToList();
                
                if (filteredDropList.Count == 0)
                {
                    // No valid (non-scrap, non-blacklisted) pickups available, so abort the purchase
                    return;
                }

                // Determine how many items to give (2 if Sale Star was active, otherwise 1)
                int itemsToGive = hasSaleStar ? 2 : 1;
                int itemsGiven = 0;
                PickupIndex firstPickupIndex = PickupIndex.none;
                ItemIndex firstItemIndex = ItemIndex.None;

                while (itemsGiven < itemsToGive && filteredDropList.Count > 0)
                {
                    PickupIndex dropPickup = Run.instance.treasureRng.NextElementUniform<PickupIndex>(filteredDropList);
                    PickupDef pickup = PickupCatalog.GetPickupDef(dropPickup);
                    
                    if (pickup.itemIndex != ItemIndex.None)
                    {
                        this.master.inventory.GiveItem(pickup.itemIndex, 1);
                        
                        // Store first pickup info for chat message
                        if (itemsGiven == 0)
                        {
                            firstPickupIndex = dropPickup;
                            firstItemIndex = pickup.itemIndex;
                        }
                    }
                    else if (pickup.equipmentIndex != EquipmentIndex.None)
                    {
                        // For equipment, only give one regardless of Sale Star
                        if (itemsGiven == 0)
                        {
                            this.master.inventory.SetEquipmentIndex(pickup.equipmentIndex);
                            firstPickupIndex = dropPickup;
                            firstItemIndex = ItemIndex.None;
                        }
                    }
                    else
                    {
                        // Neither item nor valid equipment, skip
                        continue;
                    }

                    itemsGiven++;
                }

                // Chat message (only for the first item to avoid spam)
                if (PlayerBotManager.ShowBuyMessages.Value && itemsGiven > 0 && firstPickupIndex != PickupIndex.none)
                {
                    PickupDef firstPickupDef = PickupCatalog.GetPickupDef(firstPickupIndex);
                    Chat.SendBroadcastChat(new Chat.PlayerPickupChatMessage
                    {
                        subjectAsCharacterBody = this.master.GetBody(),
                        baseToken = "PLAYER_PICKUP",
                        pickupToken = ((firstPickupDef != null) ? firstPickupDef.nameToken : null) ?? PickupCatalog.invalidPickupToken,
                        pickupColor = (firstPickupDef != null) ? firstPickupDef.baseColor : Color.black,
                        pickupQuantity = firstItemIndex != ItemIndex.None ? (uint)this.master.inventory.GetItemCount(firstItemIndex) : 1
                    });
                }
            }
        }

        enum ChestTier
        {
            WHITE = 0,
            GREEN = 1,
            RED = 2
        }

        private static bool IsScrapPickup(PickupIndex pickupIndex)
        {
            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
            if (pickupDef == null)
            {
                return false;
            }

            ItemIndex itemIndex = pickupDef.itemIndex;
            if (itemIndex == ItemIndex.None)
            {
                return false;
            }

            ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
            if (itemDef == null)
            {
                return false;
            }

            return itemDef.ContainsTag(ItemTag.Scrap) || itemDef.ContainsTag(ItemTag.PriorityScrap);
        }

        private bool HasActiveSaleStar()
        {
            return saleStarItemIndex != ItemIndex.None && this.master.inventory.GetItemCount(saleStarItemIndex) > 0;
        }

        private bool TryConsumeSaleStar()
        {
            if (saleStarItemIndex == ItemIndex.None || saleStarConsumedItemIndex == ItemIndex.None)
            {
                return false;
            }

            int currentCount = this.master.inventory.GetItemCount(saleStarItemIndex);
            if (currentCount > 0)
            {
                // Transform active Sale Star to consumed version
                this.master.inventory.RemoveItem(saleStarItemIndex, 1);
                this.master.inventory.GiveItem(saleStarConsumedItemIndex, 1);
                return true;
            }

            return false;
        }
    }
}
