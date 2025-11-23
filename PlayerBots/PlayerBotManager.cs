using BepInEx;
using BepInEx.Configuration;
using PlayerBots.AI;
using PlayerBots.AI.SkillHelpers;
using PlayerBots.Custom;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PlayerBots
{
    [BepInPlugin("com.meledy.PlayerBots", "PlayerBots", "1.7.1")]
    public class PlayerBotManager : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource BotLogger { get; private set; }

        public static System.Random random = new System.Random();

        public static List<GameObject> playerbots = new List<GameObject>();

        public static List<SurvivorIndex> RandomSurvivorsList = new List<SurvivorIndex>();
        public static Dictionary<string, SurvivorIndex> SurvivorDict = new Dictionary<string, SurvivorIndex>();

        // Config options
        public static ConfigEntry<int> InitialRandomBots { get; set; }
        public static ConfigEntry<int>[] InitialBots;

        public static ConfigEntry<int> MaxBotPurchasesPerStage { get; set; }
        public static ConfigEntry<bool> AutoPurchaseItems { get; set; }
        public static ConfigEntry<float> Tier1ChestBotWeight { get; set; }
        public static ConfigEntry<int> Tier1ChestBotCost { get; set; }
        public static ConfigEntry<float> Tier2ChestBotWeight { get; set; }
        public static ConfigEntry<int> Tier2ChestBotCost { get; set; }
        public static ConfigEntry<float> Tier3ChestBotWeight { get; set; }
        public static ConfigEntry<int> Tier3ChestBotCost { get; set; }
        public static ConfigEntry<int> EquipmentBuyChance { get; set; }
        public static ConfigEntry<float> MinBuyingDelay { get; set; }
        public static ConfigEntry<float> MaxBuyingDelay { get; set; }
        public static ConfigEntry<bool> ShowBuyMessages { get; set; }
        public static ConfigEntry<bool> HostOnlySpawnBots { get; set; }
        public static ConfigEntry<bool> ShowNameplates { get; set; }
        public static ConfigEntry<bool> PlayerMode { get; set; }
        public static ConfigEntry<bool> DontScaleInteractables { get; set; }
        public static ConfigEntry<bool> BotsUseInteractables { get; set; }
        public static ConfigEntry<bool> ContinueAfterDeath { get; set; }
        public static ConfigEntry<string> ContinueAfterDeathBlacklist { get; set; }
        public static ConfigEntry<bool> RespawnAfterWave { get; set; }
        public static ConfigEntry<float> BotTeleportDistance { get; set; }
        public static ConfigEntry<bool> EnableDroneSupport { get; set; }

        public static ConfigEntry<bool> EnableDroneSupportAllBots { get; set; }

        // TeammateRevival compatibility
        public static ConfigEntry<bool> EnableTeammateRevivalCompatibility { get; set; }

        // Survivor blacklist for random bot spawning
        public static ConfigEntry<string> SurvivorBlacklist { get; set; }

        // Item blacklist for bot purchases
        public static ConfigEntry<string> ItemBlacklist { get; set; }

        //
        public static bool allRealPlayersDead;

        /// <summary>
        /// Checks if a CharacterMaster is controlled by a bot.
        /// This is used for compatibility with TeammateRevival and other mods.
        /// </summary>
        /// <param name="master">The CharacterMaster to check</param>
        /// <returns>True if the master is controlled by a bot, false otherwise</returns>
        public static bool IsBot(CharacterMaster master)
        {
            if (!EnableTeammateRevivalCompatibility.Value)
                return false;

            var body = master?.GetBodyObject();
            return body && body.GetComponent<PlayerBotController>() != null;
        }

        public void Awake()
        {
            BotLogger = Logger;

            // Config
            InitialRandomBots = Config.Bind("Starting Bots", "StartingBots.Random", 0, "Starting amount of bots to spawn at the start of a run. (Random)");
            SurvivorBlacklist = Config.Bind("Starting Bots", "SurvivorBlacklist", "", "List of survivor names to exclude from random bot spawning. Supports display names (e.g., 'Chef', 'Mercenary') and asset names (e.g., 'GnomeChefBody', 'MercenaryBody'). Leave empty to disable filtering. Use pb_listsurvivors to see available survivor names.");

            AutoPurchaseItems = Config.Bind("Bot Inventory", "AutoPurchaseItems", true, "Maximum amount of purchases a playerbot can do per stage. Items are purchased directly instead of from chests.");
            MaxBotPurchasesPerStage = Config.Bind("Bot Inventory", "MaxBotPurchasesPerStage", 10, "Maximum amount of putchases a playerbot can do per stage.");
            Tier1ChestBotWeight = Config.Bind("Bot Inventory", "Tier1ChestBotWeight", 0.8f, "Weight of a bot picking an item from a small chest's loot table.");
            Tier2ChestBotWeight = Config.Bind("Bot Inventory", "Tier2ChestBotWeight", 0.2f, "Weight of a bot picking an item from a large chest's loot table.");
            Tier3ChestBotWeight = Config.Bind("Bot Inventory", "Tier3ChestBotWeight", 0f, "Weight of a bot picking an item from a legendary chest's loot table.");
            Tier1ChestBotCost = Config.Bind("Bot Inventory", "Tier1ChestBotCost", 25, "Base price of a small chest for the bot.");
            Tier2ChestBotCost = Config.Bind("Bot Inventory", "Tier2ChestBotCost", 50, "Base price of a large chest for the bot.");
            Tier3ChestBotCost = Config.Bind("Bot Inventory", "Tier3ChestBotCost", 400, "Base price of a legendary chest for the bot.");
            EquipmentBuyChance = Config.Bind("Bot Inventory", "EquipmentBuyChance", 15, "Chance between 0 and 100 for a bot to buy from an equipment barrel instead of a tier 1 chest. Only active while the bot does not have a equipment item. (Default: 15)");
            MinBuyingDelay = Config.Bind("Bot Inventory", "MinBuyingDelay", 0f, "Minimum delay in seconds between the time it takes for a bot checks to buy an item.");
            MaxBuyingDelay = Config.Bind("Bot Inventory", "MaxBuyingDelay", 5f, "Maximum delay in seconds between the time it takes for a bot checks to buy an item.");
            ShowBuyMessages = Config.Bind("Bot Inventory", "ShowBuyMessages", true, "Displays whenever a bot buys an item in chat.");
            ItemBlacklist = Config.Bind("Bot Inventory", "ItemBlacklist", "", "List of item names that bots will never buy.");

            HostOnlySpawnBots = Config.Bind("Misc", "HostOnlySpawnBots", true, "Set true so that only the host may spawn bots");
            ShowNameplates = Config.Bind("Misc", "ShowNameplates", true, "Show player nameplates on playerbots if PlayerMode is false. (Host only)");

            PlayerMode = Config.Bind("Player Mode", "PlayerMode", false, "Makes the game treat playerbots like how regular players are treated. The bots now show up on the scoreboard, can pick up items, influence the map scaling, etc.");
            DontScaleInteractables = Config.Bind("Player Mode", "DontScaleInteractables", true, "Prevents interactables spawn count from scaling with bots. Only active is PlayerMode is true.");
            BotsUseInteractables = Config.Bind("Player Mode", "BotsUseInteractables", false, "[Experimental] Allow bots to use interactables, such as buying from a chest and picking up items on the ground. Only active is PlayerMode is true.");
            ContinueAfterDeath = Config.Bind("Player Mode", "ContinueAfterDeath", false, "Bots will activate and use teleporters when all real players die. Only active is PlayerMode is true.");
            ContinueAfterDeathBlacklist = Config.Bind("Player Mode", "ContinueAfterDeathBlacklist", "arena,artifactworld,artifactworld01,artifactworld02,artifactworld03,bazaar,computationalexchange,conduitcanyon,goldshores,meridian,moon,moon2,mysteryspace,solusweb,solutionalhaunt,voidraid,voidstage", "List of stage names where ContinueAfterDeath should be disabled. Only active if PlayerMode and ContinueAfterDeath are true.");
            EnableDroneSupport = Config.Bind("Player Mode", "EnableDroneSupport", true, "Allow Operator bots to purchase support drones.");
            EnableDroneSupportAllBots = Config.Bind("Player Mode", "EnableDroneSupportAllBots", false, "Allow all bots to purchase support drones. EnableDroneSupport must be enabled.");

            RespawnAfterWave = Config.Bind("Simulacrum", "RespawnAfterWave", false, "Respawns bots after each wave in simulacrum");

            BotTeleportDistance = Config.Bind("Misc", "BotTeleportDistance", 100f, "Maximum distance in meters a bot can be from their master player before teleporting to them. Set to 0 to disable teleportation.");
            EnableTeammateRevivalCompatibility = Config.Bind("Misc", "EnableTeammateRevivalCompatibility", true, "Enable compatibility fixes for TeammateRevival mod. Should prevent SkillDriver null reference errors.");

            // Sanity check
            MaxBuyingDelay.Value = Math.Max(MaxBuyingDelay.Value, MinBuyingDelay.Value);

            // Add console commands
            On.RoR2.Console.Awake += (orig, self) =>
            {
                CommandHelper.RegisterCommands(self);
                orig(self);
            };

            // Content manager load hook - Will find a better place for this later
            RoR2Application.onLoad += OnContentLoad;

            // Apply hooks
            PlayerBotHooks.AddHooks();
        }

        public void OnContentLoad()
        {
            // Base game survivors
            SurvivorDict.Add("mult", SurvivorCatalog.FindSurvivorIndex("Toolbot"));
            SurvivorDict.Add("mul-t", SurvivorCatalog.FindSurvivorIndex("Toolbot"));
            SurvivorDict.Add("toolbot", SurvivorCatalog.FindSurvivorIndex("Toolbot"));
            SurvivorDict.Add("hunt", SurvivorCatalog.FindSurvivorIndex("Huntress"));
            SurvivorDict.Add("huntress", SurvivorCatalog.FindSurvivorIndex("Huntress"));
            SurvivorDict.Add("engi", SurvivorCatalog.FindSurvivorIndex("Engi"));
            SurvivorDict.Add("engineer", SurvivorCatalog.FindSurvivorIndex("Engi"));
            SurvivorDict.Add("mage", SurvivorCatalog.FindSurvivorIndex("Mage"));
            SurvivorDict.Add("arti", SurvivorCatalog.FindSurvivorIndex("Mage"));
            SurvivorDict.Add("artificer", SurvivorCatalog.FindSurvivorIndex("Mage"));
            SurvivorDict.Add("merc", SurvivorCatalog.FindSurvivorIndex("Merc"));
            SurvivorDict.Add("mercenary", SurvivorCatalog.FindSurvivorIndex("Merc"));
            SurvivorDict.Add("rex", SurvivorCatalog.FindSurvivorIndex("Treebot"));
            SurvivorDict.Add("treebot", SurvivorCatalog.FindSurvivorIndex("Treebot"));
            SurvivorDict.Add("croco", SurvivorCatalog.FindSurvivorIndex("Croco"));
            SurvivorDict.Add("capt", SurvivorCatalog.FindSurvivorIndex("Captain"));
            SurvivorDict.Add("captain", SurvivorCatalog.FindSurvivorIndex("Captain"));

            // SoTV survivors
            SurvivorDict.Add("railgunner", SurvivorCatalog.FindSurvivorIndex("Railgunner"));
            SurvivorDict.Add("rail", SurvivorCatalog.FindSurvivorIndex("Railgunner"));
            SurvivorDict.Add("void", SurvivorCatalog.FindSurvivorIndex("VoidSurvivor"));
            SurvivorDict.Add("voidfiend", SurvivorCatalog.FindSurvivorIndex("VoidSurvivor"));
            SurvivorDict.Add("voidsurvivor", SurvivorCatalog.FindSurvivorIndex("VoidSurvivor"));

            // SotS survivors
            SurvivorDict.Add("seeker", SurvivorCatalog.FindSurvivorIndex("Seeker"));
            SurvivorDict.Add("chef", SurvivorCatalog.FindSurvivorIndex("Chef"));
            SurvivorDict.Add("son", SurvivorCatalog.FindSurvivorIndex("FalseSon"));
            SurvivorDict.Add("falseson", SurvivorCatalog.FindSurvivorIndex("FalseSon"));

            // Init skill helpers
            AiSkillHelperCatalog.Populate();

            // Config
            InitialBots = new ConfigEntry<int>[RandomSurvivorsList.Count];
            for (int i = 0; i < RandomSurvivorsList.Count; i++)
            {
                string name = BodyCatalog.GetBodyName(SurvivorCatalog.GetBodyIndexFromSurvivorIndex(RandomSurvivorsList[i])).Replace("\'", "");
                InitialBots[i] = Config.Bind("Starting Bots", "StartingBots." + name, 0, "Starting amount of bots to spawn at the start of a run. (" + name + ")");
            }
        }

        public static int GetInitialBotCount()
        {
            int count = InitialRandomBots.Value;
            for (int randomSurvivorsIndex = 0; randomSurvivorsIndex < InitialBots.Length; randomSurvivorsIndex++)
            {
                count += InitialBots[randomSurvivorsIndex].Value;
            }
            return count;
        }

        public static void SpawnPlayerbot(CharacterMaster owner, SurvivorIndex survivorIndex)
        {
            if (PlayerMode.Value)
            {
                SpawnPlayerbotAsPlayer(owner, survivorIndex);
            }
            else
            {
                SpawnPlayerbotAsSummon(owner, survivorIndex);
            }
        }

        private static void SpawnPlayerbotAsPlayer(CharacterMaster owner, SurvivorIndex survivorIndex)
        {
            SurvivorDef def = SurvivorCatalog.GetSurvivorDef(survivorIndex);
            if (def == null)
            {
                return;
            }
            else if (!def.CheckRequiredExpansionEnabled())
            {
                Debug.Log("You do not have the proper expansion enabled.");
                return;
            }

            GameObject bodyPrefab = def.bodyPrefab;
            if (bodyPrefab == null)
            {
                return;
            }

            // Create spawn card
            PlayerBotSpawnCard card = ScriptableObject.CreateInstance<PlayerBotSpawnCard>();
            card.hullSize = HullClassification.Human;
            card.nodeGraphType = MapNodeGroup.GraphType.Ground;
            card.occupyPosition = false;
            card.sendOverNetwork = true;
            card.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            card.prefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster");
            card.bodyPrefab = bodyPrefab;

            // Get spawn position
            Transform spawnPosition = GetRandomSpawnPosition(owner);

            if (spawnPosition == null)
            {
                Debug.LogError("No spawn positions found for playerbot");
                return;
            }

            // Spawn
            DirectorSpawnRequest spawnRequest = new DirectorSpawnRequest(card, new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                minDistance = 3f,
                maxDistance = 40f,
                spawnOnTarget = spawnPosition
            }, RoR2Application.rng);
            spawnRequest.ignoreTeamMemberLimit = true;
            spawnRequest.teamIndexOverride = new TeamIndex?(TeamIndex.Player);

            spawnRequest.onSpawnedServer = result =>
            {
                GameObject gameObject = result.spawnedInstance;

                if (gameObject == null)
                {
                    return;
                }

                // Add components
                EntityStateMachine stateMachine = gameObject.AddComponent<PlayerBotStateMachine>() as EntityStateMachine;
                BaseAI ai = gameObject.AddComponent<PlayerBotBaseAI>() as BaseAI;
                AIOwnership aiOwnership = gameObject.AddComponent<AIOwnership>() as AIOwnership;
                aiOwnership.ownerMaster = owner;

                // Add PlayerCharacterMasterController for TeammateRevival compatibility
                if (EnableTeammateRevivalCompatibility.Value)
                {
                    var playerCharacterMasterController = gameObject.AddComponent<PlayerCharacterMasterController>();
                    
                    BotLogger.LogInfo("Added PlayerCharacterMasterController for TeammateRevival compatibility");
                }

                CharacterMaster master = gameObject.GetComponent<CharacterMaster>();

                // Random skin
                SetRandomSkin(master, bodyPrefab);

                // Set commponent values
                master.SetFieldValue("aiComponents", gameObject.GetComponents<BaseAI>());
                master.destroyOnBodyDeath = false; // Allow the bots to spawn in the next stage

                // Starting items
                GiveStartingItems(owner, master);

                // Add custom skills
                InjectSkillDrivers(gameObject, ai, survivorIndex);

                if (AutoPurchaseItems.Value)
                {
                    // Add item manager
                    ItemManager itemManager = gameObject.AddComponent<ItemManager>() as ItemManager;
                }

                MaybeAttachOperatorDroneSupport(gameObject, bodyPrefab);

                // Add to playerbot list
                playerbots.Add(gameObject);
            };

            // Don't freeze the game if there is an error spawning the bot
            try
            {
                DirectorCore.instance.TrySpawnObject(spawnRequest);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            // Cleanup spawn card
            Destroy(card);
        }


        private static void SpawnPlayerbotAsSummon(CharacterMaster owner, SurvivorIndex survivorIndex)
        {
            SurvivorDef def = SurvivorCatalog.GetSurvivorDef(survivorIndex);
            if (def == null)
            {
                return;
            }
            else if (!def.CheckRequiredExpansionEnabled())
            {
                Debug.Log("You do not have the proper expansion enabled.");
                return;
            }

            GameObject bodyPrefab = def.bodyPrefab;
            if (bodyPrefab == null)
            {
                return;
            }

            // Create spawn card
            PlayerBotSpawnCard card = ScriptableObject.CreateInstance<PlayerBotSpawnCard>();
            card.hullSize = HullClassification.Human;
            card.nodeGraphType = MapNodeGroup.GraphType.Ground;
            card.occupyPosition = false;
            card.sendOverNetwork = true;
            card.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            card.prefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster");
            card.bodyPrefab = bodyPrefab;

            // Get spawn position
            Transform spawnPosition = GetRandomSpawnPosition(owner);

            if (spawnPosition == null)
            {
                Debug.LogError("No spawn positions found for playerbot");
                return;
            }

            // Spawn request
            DirectorSpawnRequest spawnRequest = new DirectorSpawnRequest(card, new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                minDistance = 3f,
                maxDistance = 40f,
                spawnOnTarget = spawnPosition
            }, RoR2Application.rng);
            spawnRequest.ignoreTeamMemberLimit = true;
            spawnRequest.teamIndexOverride = new TeamIndex?(TeamIndex.Player);

            spawnRequest.onSpawnedServer = result =>
            {
                GameObject gameObject = result.spawnedInstance;

                if (gameObject == null)
                {
                    return;
                }

                CharacterMaster master = gameObject.GetComponent<CharacterMaster>();
                BaseAI ai = gameObject.GetComponent<BaseAI>();
                AIOwnership aiOwnership = gameObject.AddComponent<AIOwnership>() as AIOwnership;
                aiOwnership.ownerMaster = owner;

                // Add PlayerCharacterMasterController for TeammateRevival compatibility
                if (EnableTeammateRevivalCompatibility.Value)
                {
                    var playerCharacterMasterController = gameObject.AddComponent<PlayerCharacterMasterController>();
                    
                    BotLogger.LogInfo("Added PlayerCharacterMasterController for TeammateRevival compatibility (summon mode)");
                }

                if (master)
                {
                    master.name = "PlayerBot";
                    master.teamIndex = TeamIndex.Player;

                    SetRandomSkin(master, bodyPrefab);

                    GiveStartingItems(owner, master);

                    // Allow the bots to spawn in the next stage
                    master.destroyOnBodyDeath = false;
                    master.gameObject.AddComponent<SetDontDestroyOnLoad>();
                }

                InjectSkillDrivers(gameObject, ai, survivorIndex);

                if (AutoPurchaseItems.Value)
                {
                    // Add item manager
                    ItemManager itemManager = gameObject.AddComponent<ItemManager>() as ItemManager;
                }

                MaybeAttachOperatorDroneSupport(gameObject, bodyPrefab);

                // Add to playerbot list
                playerbots.Add(gameObject);
            };

            // Don't freeze the game if there is an error spawning the bot
            try
            {
                DirectorCore.instance.TrySpawnObject(spawnRequest);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            // Cleanup spawn card
            Destroy(card);
        }

        private static void MaybeAttachOperatorDroneSupport(GameObject masterObject, GameObject bodyPrefab)
        {
            if (!masterObject || !bodyPrefab || EnableDroneSupport == null || !EnableDroneSupport.Value)
            {
                return;
            }

            CharacterBody operatorBody = DLC3Content.BodyPrefabs.DroneTechBody;
            if ((operatorBody && bodyPrefab == operatorBody.gameObject && !masterObject.GetComponent<OperatorDroneSupport>()) || EnableDroneSupportAllBots.Value == true)
            {
                masterObject.AddComponent<OperatorDroneSupport>();
            }
        }

        private static Transform GetRandomSpawnPosition(CharacterMaster owner)
        {
            if (owner.GetBody() != null)
            {
                return owner.GetBody().transform;
            }
            else
            {
                SpawnPoint spawnPoint = SpawnPoint.ConsumeSpawnPoint();
                if (spawnPoint != null)
                {
                    spawnPoint.consumed = false;
                    return spawnPoint.transform;
                }
            }

            return null;
        }

        private static void GiveStartingItems(CharacterMaster owner, CharacterMaster master)
        {
            master.GiveMoney(owner.money);
            master.inventory.CopyItemsFrom(owner.inventory);
            master.inventory.RemoveItem(ItemCatalog.FindItemIndex("CaptainDefenseMatrix"), owner.inventory.GetItemCount(ItemCatalog.FindItemIndex("CaptainDefenseMatrix")));
            master.inventory.GiveItem(ItemCatalog.FindItemIndex("DrizzlePlayerHelper"), 1);
            
            // DEBUG: Give every bot a Sale Star for testing (REMOVE AFTER TESTING)
            GiveDebugSaleStar(master);
        }

        private static void GiveDebugSaleStar(CharacterMaster master)
        {
            // Try to find and give Sale Star item
            ItemIndex saleStarIndex = ItemCatalog.FindItemIndex("LowerPricedChests");
            if (saleStarIndex == ItemIndex.None)
            {
                saleStarIndex = ItemCatalog.FindItemIndex("SaleStar");
            }
            
            if (saleStarIndex != ItemIndex.None)
            {
                master.inventory.GiveItem(saleStarIndex, 1);
                BotLogger.LogInfo($"Gave debug Sale Star to bot {master.name}");
            }
            else
            {
                BotLogger.LogWarning("Could not find Sale Star item to give to bot");
            }
        }

        private static void SetRandomSkin(CharacterMaster master, GameObject bodyPrefab)
        {
            BodyIndex bodyIndex = bodyPrefab.GetComponent<CharacterBody>().bodyIndex;
            SkinDef[] skins = BodyCatalog.GetBodySkins(bodyIndex);
            master.loadout.bodyLoadoutManager.SetSkinIndex(bodyIndex, (uint)UnityEngine.Random.Range(0, skins.Length));
        }

        private static void InjectSkillDrivers(GameObject gameObject, BaseAI ai, SurvivorIndex survivorIndex)
        {
            // Get skill helper
            AiSkillHelper skillHelper = AiSkillHelperCatalog.CreateSkillHelper(survivorIndex);

            // Remove old skill drivers if custom skill drivers exist
            if (skillHelper.GetType() != typeof(DefaultSkillHelper))
            {
                // Get old skill drivers
                AISkillDriver[] skillDrivers = gameObject.GetComponents<AISkillDriver>();
                if (skillDrivers != null)
                {
                    // Remove skill drivers
                    StripSkills(skillDrivers);
                }

                // Add skill drivers based on class
                skillHelper.InjectSkills(gameObject, ai);

                // Get the newly injected skill drivers and properly replace them using BaseAI method
                AISkillDriver[] newSkillDrivers = gameObject.GetComponents<AISkillDriver>();
                if (newSkillDrivers != null && newSkillDrivers.Length > 0)
                {
                    // Filter out null skill drivers before setting them
                    var validSkillDrivers = new List<AISkillDriver>();
                    int nullCount = 0;
                    
                    for (int i = 0; i < newSkillDrivers.Length; i++)
                    {
                        if (newSkillDrivers[i] != null)
                        {
                            validSkillDrivers.Add(newSkillDrivers[i]);
                        }
                        else
                        {
                            nullCount++;
                        }
                    }
                    
                    if (validSkillDrivers.Count > 0)
                    {
                        // Use the proper BaseAI method to replace skill drivers
                        ai.ReplaceSkillDrivers(validSkillDrivers.ToArray());
                    }
                }
                else
                {
                    BotLogger.LogWarning("No skill drivers found after injection!");
                }
            }
            else
            {
                // Add default skills
                skillHelper.AddDefaultSkills(gameObject, ai, 0);
                
                // Get the skill drivers and replace them
                AISkillDriver[] defaultSkillDrivers = gameObject.GetComponents<AISkillDriver>();
                if (defaultSkillDrivers != null && defaultSkillDrivers.Length > 0)
                {
                    // Filter out null skill drivers before setting them
                    var validSkillDrivers = new List<AISkillDriver>();
                    int nullCount = 0;
                    
                    for (int i = 0; i < defaultSkillDrivers.Length; i++)
                    {
                        if (defaultSkillDrivers[i] != null)
                        {
                            validSkillDrivers.Add(defaultSkillDrivers[i]);
                        }
                        else
                        {
                            nullCount++;
                        }
                    }
                    
                    if (validSkillDrivers.Count > 0)
                    {
                        ai.ReplaceSkillDrivers(validSkillDrivers.ToArray());
                    }
                }
            }

            // Set BaseAI properties
            if (ai)
            {
                ai.name = "PlayerBot";
                ai.neverRetaliateFriendlies = true;
                ai.fullVision = true;
                ai.aimVectorDampTime = .0005f;
                ai.aimVectorMaxSpeed = 18000f;
            }

            // Add playerbot controller for extra behaviors and fixes
            PlayerBotController controller = gameObject.AddComponent<PlayerBotController>();
            controller.SetSkillHelper(skillHelper);

            // Force skill driver re-initialization after a short delay if TeammateRevival compatibility is enabled
            if (EnableTeammateRevivalCompatibility.Value)
            {
                gameObject.AddComponent<BotSkillDriverInitializer>();
                gameObject.AddComponent<SafeSkillDriverEvaluator>();
            }
        }

        public static void SpawnPlayerbots(CharacterMaster owner, SurvivorIndex characterType, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                SpawnPlayerbot(owner, characterType);
            }
        }

        public static void SpawnRandomPlayerbots(CharacterMaster owner, int amount)
        {
            int lastCharacterType = -1;
            for (int i = 0; i < amount; i++)
            {
                int randomSurvivorIndex = -1;
                int attempts = 0;
                do
                {
                    randomSurvivorIndex = random.Next(0, RandomSurvivorsList.Count);
                    attempts++;
                    
                    // Prevent infinite loop if all survivors are blacklisted/whitelisted
                    if (attempts > RandomSurvivorsList.Count * 2)
                    {
                        BotLogger.LogWarning("All survivors appear to be filtered out. Defaulting to first available survivor.");
                        break;
                    }
                }
                while ((randomSurvivorIndex == lastCharacterType || 
                        !SurvivorCatalog.GetSurvivorDef((SurvivorIndex) RandomSurvivorsList[randomSurvivorIndex]).CheckRequiredExpansionEnabled() ||
                        !IsSurvivorAllowed(RandomSurvivorsList[randomSurvivorIndex])) 
                       && RandomSurvivorsList.Count > 1);

                SpawnPlayerbot(owner, RandomSurvivorsList[randomSurvivorIndex]);

                lastCharacterType = randomSurvivorIndex;
            }
        }

        private static void StripSkills(AISkillDriver[] skillDrivers)
        {
            foreach (AISkillDriver skill in skillDrivers)
            {
                DestroyImmediate(skill);
            }
        }

        public void FixedUpdate()
        {
            allRealPlayersDead = !PlayerCharacterMasterController.instances.Any(p => p.preventGameOver && p.isConnected);
        }

        /// <summary>
        /// Checks if ContinueAfterDeath should be active for the current stage.
        /// Returns false if the current stage is in the blacklist.
        /// </summary>
        public static bool IsContinueAfterDeathAllowedForCurrentStage()
        {
            if (!ContinueAfterDeath.Value)
            {
                return false;
            }

            string blacklist = ContinueAfterDeathBlacklist?.Value;
            if (string.IsNullOrWhiteSpace(blacklist))
            {
                return true;
            }

            SceneDef currentScene = SceneCatalog.mostRecentSceneDef;
            if (currentScene == null)
            {
                return true;
            }

            string currentSceneName = currentScene.baseSceneName;
            if (string.IsNullOrEmpty(currentSceneName))
            {
                return true;
            }

            string[] blacklistedStages = blacklist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string stage in blacklistedStages)
            {
                string trimmedStage = stage.Trim();
                if (string.Equals(trimmedStage, currentSceneName, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if a survivor is allowed based on the blacklist configuration.
        /// Returns true if the survivor should be included in random selection.
        /// Supports both display names (e.g., "Chef") and body prefab names (e.g., "GnomeChefBody").
        /// </summary>
        public static bool IsSurvivorAllowed(SurvivorIndex survivorIndex)
        {
            string blacklist = SurvivorBlacklist?.Value;
            if (string.IsNullOrWhiteSpace(blacklist))
            {
                return true; // No filtering enabled
            }

            string[] blacklistedSurvivors = blacklist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Get both display name and body prefab name
            string displayName = GetSurvivorDisplayName(survivorIndex).ToLowerInvariant();
            string bodyPrefabName = GetSurvivorBodyPrefabName(survivorIndex).ToLowerInvariant();

            foreach (string survivor in blacklistedSurvivors)
            {
                string trimmedSurvivor = survivor.Trim().ToLowerInvariant();
                
                // Check if the blacklist entry matches either the display name or body prefab name
                if (trimmedSurvivor == displayName || trimmedSurvivor == bodyPrefabName)
                {
                    return false; // Survivor is blacklisted
                }
            }

            return true; // Survivor is not in blacklist
        }

        /// <summary>
        /// Gets the display name for a survivor index.
        /// </summary>
        private static string GetSurvivorDisplayName(SurvivorIndex survivorIndex)
        {
            SurvivorDef def = SurvivorCatalog.GetSurvivorDef(survivorIndex);
            if (def == null)
            {
                return survivorIndex.ToString();
            }

            CharacterBody body = def.bodyPrefab.GetComponent<CharacterBody>();
            return body ? body.GetDisplayName() : def.survivorIndex.ToString();
        }

        /// <summary>
        /// Gets the body prefab name for a survivor index.
        /// </summary>
        private static string GetSurvivorBodyPrefabName(SurvivorIndex survivorIndex)
        {
            SurvivorDef def = SurvivorCatalog.GetSurvivorDef(survivorIndex);
            if (def == null)
            {
                return survivorIndex.ToString();
            }

            return def.bodyPrefab ? def.bodyPrefab.name : def.survivorIndex.ToString();
        }

        /// <summary>
        /// Gets the display name for a survivor index.
        /// </summary>
        private static string GetSurvivorName(SurvivorIndex survivorIndex)
        {
            return GetSurvivorDisplayName(survivorIndex);
        }

        /// <summary>
        /// Checks if an item is blacklisted from bot purchases.
        /// Returns true if the item is allowed to be purchased.
        /// </summary>
        public static bool IsItemAllowed(ItemIndex itemIndex)
        {
            string blacklist = ItemBlacklist?.Value;
            if (string.IsNullOrWhiteSpace(blacklist))
            {
                return true; // No filtering enabled
            }

            string[] blacklistedItems = blacklist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string itemName = GetItemName(itemIndex).ToLowerInvariant();

            foreach (string item in blacklistedItems)
            {
                string trimmedItem = item.Trim().ToLowerInvariant();
                if (trimmedItem == itemName)
                {
                    return false; // Item is blacklisted
                }
            }

            return true; // Item is not in blacklist
        }

        /// <summary>
        /// Gets the name for an item index.
        /// </summary>
        private static string GetItemName(ItemIndex itemIndex)
        {
            ItemDef def = ItemCatalog.GetItemDef(itemIndex);
            if (def == null)
            {
                return itemIndex.ToString();
            }

            return def.name;
        }

        [ConCommand(commandName = "addbot", flags = ConVarFlags.ExecuteOnServer, helpText = "Adds a playerbot. Usage: addbot [character index] [amount] [network user index]")]
        private static void CCAddBot(ConCommandArgs args)
        {
            NetworkUser user = args.sender;
            if (HostOnlySpawnBots.Value)
            {
                if (NetworkUser.readOnlyInstancesList[0].netId != user.netId)
                {
                    return;
                }
            }

            if (Stage.instance == null)
            {
                return;
            }

            int characterType = 0;
            bool useRandom = false;
            if (args.userArgs.Count == 0)
            {
                useRandom = true;
            }
            else if (args.userArgs.Count > 0)
            {
                string classString = args.userArgs[0];
                if (classString.ToLower() == "random")
                {
                    useRandom = true;
                }
                else if (!Int32.TryParse(classString, out characterType))
                {
                    SurvivorIndex index;
                    if (SurvivorDict.TryGetValue(classString.ToLower(), out index))
                    {
                        characterType = (int)index;
                    }
                    else
                    {
                        characterType = 0;
                        Debug.LogError("No survivor with that name exists.");
                        return;
                    }
                }
            }

            int amount = 1;
            if (args.userArgs.Count > 1)
            {
                string amountString = args.userArgs[1];
                Int32.TryParse(amountString, out amount);
            }

            if (args.userArgs.Count > 2)
            {
                int userIndex = 0;
                string userString = args.userArgs[2];
                if (Int32.TryParse(userString, out userIndex))
                {
                    userIndex--;
                    if (userIndex >= 0 && userIndex < NetworkUser.readOnlyInstancesList.Count)
                    {
                        user = NetworkUser.readOnlyInstancesList[userIndex];
                    }
                }
                else
                {
                    return;
                }
            }

            if (!user || user.master.IsDeadAndOutOfLivesServer())
            {
                return;
            }

            if (useRandom)
            {
                if (RandomSurvivorsList.Count == 0)
                {
                    Debug.LogWarning("No random survivors available, defaulting to Bandit.");
                    SpawnPlayerbots(user.master, (SurvivorIndex)0, amount);
                }
                else
                {
                    var rand = new System.Random();
                    for (int i = 0; i < amount; i++)
                    {
                        int randomIndex = rand.Next(0, RandomSurvivorsList.Count);
                        // Check if the selected survivor is allowed before spawning
                        if (IsSurvivorAllowed(RandomSurvivorsList[randomIndex]))
                        {
                            SpawnPlayerbots(user.master, RandomSurvivorsList[randomIndex], 1);
                        }
                        else
                        {
                            // Try to find an allowed survivor
                            bool foundAllowed = false;
                            for (int attempts = 0; attempts < RandomSurvivorsList.Count; attempts++)
                            {
                                int retryIndex = rand.Next(0, RandomSurvivorsList.Count);
                                if (IsSurvivorAllowed(RandomSurvivorsList[retryIndex]))
                                {
                                    SpawnPlayerbots(user.master, RandomSurvivorsList[retryIndex], 1);
                                    foundAllowed = true;
                                    break;
                                }
                            }
                            
                            if (!foundAllowed)
                            {
                                Debug.LogWarning("All available survivors appear to be filtered out. Skipping this bot spawn.");
                            }
                        }
                    }
                }
            }
            else
            {
                SpawnPlayerbots(user.master, (SurvivorIndex)characterType, amount);
            }

            Debug.Log(user.userName + " spawned " + amount + " bots for " + user.userName);
        }

        [ConCommand(commandName = "addrandombot", flags = ConVarFlags.ExecuteOnServer, helpText = "Adds a random playerbot. Usage: addrandombot [amount] [network user index]")]
        private static void CCAddRandomBot(ConCommandArgs args)
        {
            NetworkUser user = args.sender;
            if (HostOnlySpawnBots.Value)
            {
                if (NetworkUser.readOnlyInstancesList[0].netId != user.netId)
                {
                    return;
                }
            }

            if (Stage.instance == null)
            {
                return;
            }

            int amount = 1;
            if (args.userArgs.Count > 0)
            {
                string amountString = args.userArgs[0];
                Int32.TryParse(amountString, out amount);
            }

            if (args.userArgs.Count > 1)
            {
                int userIndex = 0;
                string userString = args.userArgs[1];
                if (Int32.TryParse(userString, out userIndex))
                {
                    userIndex--;
                    if (userIndex >= 0 && userIndex < NetworkUser.readOnlyInstancesList.Count)
                    {
                        user = NetworkUser.readOnlyInstancesList[userIndex];
                    }
                }
                else
                {
                    return;
                }
            }

            if (!user || user.master.IsDeadAndOutOfLivesServer())
            {
                return;
            }

            SpawnRandomPlayerbots(user.master, amount);

            Debug.Log(user.userName + " spawned " + amount + " bots for " + user.userName);
        }

        [ConCommand(commandName = "removebots", flags = ConVarFlags.SenderMustBeServer, helpText = "Removes all bots")]
        private static void CCRemoveBots(ConCommandArgs args)
        {
            foreach (GameObject gameObject in playerbots)
            {
                CharacterMaster master = gameObject.GetComponent<CharacterMaster>();
                BaseAI ai = gameObject.GetComponent<BaseAI>();
                ai.name = "";

                master.TrueKill();

                Destroy(gameObject);
            }

            playerbots.Clear();
        }

        [ConCommand(commandName = "killbots", flags = ConVarFlags.SenderMustBeServer, helpText = "Removes all bots")]
        private static void CCKillBots(ConCommandArgs args)
        {
            foreach (GameObject gameObject in playerbots)
            {
                if (gameObject)
                {
                    CharacterMaster master = gameObject.GetComponent<CharacterMaster>();
                    master.TrueKill();
                }
            }
        }

        [ConCommand(commandName = "tpbots", flags = ConVarFlags.SenderMustBeServer, helpText = "Teleports all bots to you")]
        private static void CCTpBots(ConCommandArgs args)
        {
            NetworkUser user = args.sender;

            if (Stage.instance == null || user.master == null || user.master.IsDeadAndOutOfLivesServer())
            {
                return;
            }

            foreach (GameObject gameObject in playerbots)
            {
                if (gameObject)
                {
                    CharacterMaster master = gameObject.GetComponent<CharacterMaster>();

                    if (!master.IsDeadAndOutOfLivesServer())
                    {
                        TeleportHelper.TeleportGameObject(master.GetBody().gameObject, new Vector3(
                            user.master.GetBody().transform.position.x,
                            user.master.GetBody().transform.position.y,
                            user.master.GetBody().transform.position.z
                        ));
                    }
                }
            }
        }

        [ConCommand(commandName = "pb_startingbots", flags = ConVarFlags.None, helpText = "Set initial bot count [character type] [amount]")]
        private static void CCInitialBot(ConCommandArgs args)
        {
            if (RandomSurvivorsList.Count == 0 || args.userArgs.Count < 2)
            {
                return;
            }

            string survivorToken = args.userArgs[0];
            if (!TryResolveStartingBotIndex(survivorToken, out int characterType))
            {
                Debug.LogWarning($"pb_startingbots: Unable to resolve survivor '{survivorToken}'.");
                return;
            }

            if (!Int32.TryParse(args.userArgs[1], out int amount))
            {
                return;
            }

            InitialBots[characterType].Value = amount;

            SurvivorIndex survivor = RandomSurvivorsList[characterType];
            BodyIndex bodyIndex = SurvivorCatalog.GetBodyIndexFromSurvivorIndex(survivor);
            string survivorName = BodyCatalog.GetBodyName(bodyIndex);

            Debug.Log($"Set StartingBots.{survivorName} to {amount}");
        }

        [ConCommand(commandName = "pb_startingbots_random", flags = ConVarFlags.None, helpText = "Set initial random bot count [amount]")]
        private static void CCInitialRandomBot(ConCommandArgs args)
        {
            int amount = 0;
            if (args.userArgs.Count > 0)
            {
                string amountString = args.userArgs[0];
                Int32.TryParse(amountString, out amount);
            }
            else
            {
                return;
            }

            InitialRandomBots.Value = amount;
            Debug.Log("Set StartingBots.Random to " + amount);
        }

        [ConCommand(commandName = "pb_startingbots_reset", flags = ConVarFlags.None, helpText = "Resets all initial bots to 0")]
        private static void CCClearInitialBot(ConCommandArgs args)
        {
            InitialRandomBots.Value = 0;
            for (int i = 0; i < InitialBots.Length; i++)
            {
                InitialBots[i].Value = 0;
            }
            Debug.Log("Reset all StartingBots values to 0");
        }

        [ConCommand(commandName = "pb_maxpurchases", flags = ConVarFlags.None, helpText = "Sets the MaxBotPurchasesPerStage value.")]
        private static void CCSetMaxPurchases(ConCommandArgs args)
        {
            int amount = 0;
            if (args.userArgs.Count > 0)
            {
                string amountString = args.userArgs[0];
                Int32.TryParse(amountString, out amount);
            }
            else
            {
                return;
            }

            MaxBotPurchasesPerStage.Value = amount;
            Debug.Log("Set MaxBotPurchasesPerStage to " + amount);
        }

        [ConCommand(commandName = "pb_listbots", flags = ConVarFlags.SenderMustBeServer, helpText = "Lists bots in console.")]
        private static void CCTestBots(ConCommandArgs args)
        {
            if (Stage.instance == null)
            {
                return;
            }

            NetworkUser user = args.sender;

            foreach (GameObject gameObject in playerbots)
            {
                CharacterMaster master = gameObject.GetComponent<CharacterMaster>();
                AIOwnership aiOwnership = gameObject.GetComponent<AIOwnership>();
                string name = master.GetBody().GetDisplayName();

                if (aiOwnership.ownerMaster)
                {
                    Debug.Log(name + "'s master: " + aiOwnership.ownerMaster.GetBody().GetUserName());
                }
                else
                {
                    Debug.Log(name + " has no master");
                }

                Debug.Log(name + "'s money: " + master.money);
            }
        }

        private static bool TryResolveStartingBotIndex(string survivorToken, out int characterType)
        {
            characterType = 0;
            if (string.IsNullOrWhiteSpace(survivorToken))
            {
                return false;
            }

            // Allow direct numeric index into RandomSurvivorsList.
            if (Int32.TryParse(survivorToken, out int numericIndex))
            {
                characterType = Mathf.Clamp(numericIndex, 0, RandomSurvivorsList.Count - 1);
                return true;
            }

            SurvivorIndex survivorIndex;
            if (SurvivorDict.TryGetValue(survivorToken.ToLowerInvariant(), out survivorIndex) && TryGetRandomSurvivorListIndex(survivorIndex, out characterType))
            {
                return true;
            }

            if (PlayerBotUtils.TryGetSurvivorIndexByBodyPrefabName(survivorToken, out survivorIndex) && TryGetRandomSurvivorListIndex(survivorIndex, out characterType))
            {
                return true;
            }

            // Try again with "Body" suffix trimming if provided.
            if (survivorToken.EndsWith("body", StringComparison.OrdinalIgnoreCase))
            {
                string trimmed = survivorToken.Substring(0, survivorToken.Length - 4);
                if (PlayerBotUtils.TryGetSurvivorIndexByBodyPrefabName(trimmed + "Body", out survivorIndex) && TryGetRandomSurvivorListIndex(survivorIndex, out characterType))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetRandomSurvivorListIndex(SurvivorIndex survivorIndex, out int characterType)
        {
            characterType = RandomSurvivorsList.IndexOf(survivorIndex);
            return characterType >= 0;
        }

        [ConCommand(commandName = "pb_listsurvivors", flags = ConVarFlags.None, helpText = "Lists survivor indexes.")]
        private static void CCListSurvivors(ConCommandArgs args)
        {
            Debug.Log("Listing all registered survivors and their indexes.");
            foreach (SurvivorDef def in SurvivorCatalog.allSurvivorDefs)
            {
                Debug.Log(def.bodyPrefab.GetComponent<CharacterBody>().GetDisplayName() + " (" + def.bodyPrefab.name  + ") : " + (int)def.survivorIndex);
            }
        }

        [ConCommand(commandName = "killplayer", flags = ConVarFlags.SenderMustBeServer, helpText = "Kills the player")]
        private static void CCKillPlayer(ConCommandArgs args)
        {
            NetworkUser user = args.sender;
            user.master.TrueKill();
        }


    }
}
