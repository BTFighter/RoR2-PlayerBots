using RoR2.CharacterAI;
using UnityEngine;
using System;
using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("RobomandoBody")]
    [CustomSurvivor("https://thunderstore.io/c/riskofrain2/p/The_Bozos/RobomandoMod/", "1.0.13")]
    class RobomandoHelper : AiSkillHelper
    {
        private Run.FixedTimeStamp lastItemTime;
        private float itemDelay;
        private int itemsReceived = 0;
        private int itemsReceivedOthers = 0;
        private int currentStage = 0;
        private const int BASE_MAX_ITEMS = 7;
        private int MAX_ITEMS_OTHERS = 3;

        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "UtilityDefensive";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 0;
            skill3.maxDistance = 20;
            skill3.maxUserHealthFraction = .50f;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = false;
            skill3.activationRequiresAimConfirmation = false;
            skill3.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            skill3.aimType = AISkillDriver.AimType.MoveDirection;
            skill3.ignoreNodeGraph = false;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = true;
            skill3.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            /*AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0;
            skill4.maxDistance = 35;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = true;
            skill4.activationRequiresAimConfirmation = true;
            skill4.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = false;
            skill4.shouldSprint = false;
            skill4.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;*/

            AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Secondary";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0;
            skill2.maxDistance = 30;
            skill2.selectionRequiresTargetLoS = true;
            skill2.activationRequiresTargetLoS = true;
            skill2.activationRequiresAimConfirmation = true;
            skill2.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill2.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill2.ignoreNodeGraph = false;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = false;
            skill2.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Shoot";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = true;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0;
            skill1.maxDistance = 40;
            skill1.selectionRequiresTargetLoS = true;
            skill1.activationRequiresTargetLoS = true;
            skill1.activationRequiresAimConfirmation = true;
            skill1.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill1.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill1.ignoreNodeGraph = false;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = false;
            skill1.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 20);

            // Initialize item timer
            ResetItemTimer();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            // Check if we've moved to a new stage
            if (Run.instance != null && Run.instance.stageClearCount != currentStage)
            {
                currentStage = Run.instance.stageClearCount;
                itemsReceived = 0;
                itemsReceivedOthers = 0;
                ResetItemTimer();
            }

            if (controller.master.IsDeadAndOutOfLivesServer() || itemsReceived > GetMaxItems())
            {
                return;
            }

            if (lastItemTime.timeSince >= itemDelay)
            {
                GiveRandomItem();
                ResetItemTimer();
            }
        }

        private int GetMaxItems()
        {
            MAX_ITEMS_OTHERS = MAX_ITEMS_OTHERS + currentStage;
            return BASE_MAX_ITEMS + (currentStage * 2);
        }

        private void ResetItemTimer()
        {
            lastItemTime = Run.FixedTimeStamp.now;
            itemDelay = UnityEngine.Random.Range(30f, 45f);
        }

        private void GiveRandomItem()
        {
            if (itemsReceived > GetMaxItems())
                return;

            // Determine tier (80% white, 20% green)
            bool isGreen = UnityEngine.Random.value < 0.2f;
            List<PickupIndex> dropList;

            if (isGreen)
            {
                dropList = Run.instance.mediumChestDropTierSelector.Evaluate(UnityEngine.Random.value);
            }
            else
            {
                dropList = Run.instance.smallChestDropTierSelector.Evaluate(UnityEngine.Random.value);
            }

            if (dropList != null && dropList.Count > 0)
            {
                PickupIndex dropPickup = Run.instance.treasureRng.NextElementUniform<PickupIndex>(dropList);
                PickupDef pickup = PickupCatalog.GetPickupDef(dropPickup);
                
                if (pickup.itemIndex != ItemIndex.None)
                {
                    // Give item to Robomando
                    controller.master.inventory.GiveItem(pickup.itemIndex, 1);
                    itemsReceived++;

                    // Send chat message for Robomando
                    Chat.SendBroadcastChat(new Chat.PlayerPickupChatMessage
                    {
                        subjectAsCharacterBody = controller.master.GetBody(),
                        baseToken = "PLAYER_PICKUP",
                        pickupToken = (pickup.nameToken ?? PickupCatalog.invalidPickupToken),
                        pickupColor = pickup.baseColor,
                        pickupQuantity = (uint)controller.master.inventory.GetItemCount(pickup.itemIndex)
                    });

                    // Find a random player or AI to give the item to
                    List<CharacterMaster> possibleTargets = new List<CharacterMaster>();
                    
                    // Add all players
                    foreach (NetworkUser user in NetworkUser.readOnlyInstancesList)
                    {
                        if (user.master != null && !user.master.IsDeadAndOutOfLivesServer() && user.master != controller.master)
                        {
                            possibleTargets.Add(user.master);
                        }
                    }

                    // Add all AI bots
                    foreach (CharacterMaster master in CharacterMaster.readOnlyInstancesList)
                    {
                        if (master != controller.master && !master.IsDeadAndOutOfLivesServer() && master.teamIndex == TeamIndex.Player)
                        {
                            possibleTargets.Add(master);
                        }
                    }

                    // Give item to random target if any exist
                    if (possibleTargets.Count > 0)
                    {
                        if (itemsReceivedOthers > MAX_ITEMS_OTHERS)
                            return;

                        CharacterMaster targetMaster = possibleTargets[UnityEngine.Random.Range(0, possibleTargets.Count)];
                        targetMaster.inventory.GiveItem(pickup.itemIndex, 1);
                        itemsReceivedOthers++;

                        // Send chat message for the target
                        Chat.SendBroadcastChat(new Chat.PlayerPickupChatMessage
                        {
                            subjectAsCharacterBody = targetMaster.GetBody(),
                            baseToken = "PLAYER_PICKUP",
                            pickupToken = (pickup.nameToken ?? PickupCatalog.invalidPickupToken),
                            pickupColor = pickup.baseColor,
                            pickupQuantity = (uint)targetMaster.inventory.GetItemCount(pickup.itemIndex)
                        });
                    }
                }
            }
        }
    }
}