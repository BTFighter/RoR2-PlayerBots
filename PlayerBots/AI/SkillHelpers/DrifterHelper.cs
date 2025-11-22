using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("DrifterBody")]
    class DrifterHelper : AiSkillHelper
    {
        private DrifterBagController bagController;
        private AISkillDriver primarySkillDriver;
        private AISkillDriver utilitySkillDriver;
        private AISkillDriver[] allSkillDrivers;
        
        // Track swings for throw logic
        private int swingCount = 0;
        private bool wasBagFull = false;
        private float throwChance = 0.35f; // 35% chance to throw after 2 swings
        private int swingsBeforeThrow = 2;

        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            skill4.minDistance = 0;
            skill4.maxDistance = 15;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = true;
            skill4.activationRequiresAimConfirmation = true;
            skill4.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill4.aimType = AISkillDriver.AimType.AtCurrentLeader;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = true;
            skill4.shouldSprint = false;

            // Utility driver for throwing bagged entities
            utilitySkillDriver = gameObject.AddComponent<AISkillDriver>();
            utilitySkillDriver.customName = "UtilityThrow";
            utilitySkillDriver.skillSlot = RoR2.SkillSlot.Utility;
            utilitySkillDriver.requireSkillReady = true;
            utilitySkillDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilitySkillDriver.minDistance = 0f;
            utilitySkillDriver.maxDistance = 999f; // Will be controlled dynamically
            utilitySkillDriver.selectionRequiresTargetLoS = false;
            utilitySkillDriver.activationRequiresTargetLoS = false;
            utilitySkillDriver.activationRequiresAimConfirmation = true;
            utilitySkillDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            utilitySkillDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            utilitySkillDriver.ignoreNodeGraph = false;
            utilitySkillDriver.resetCurrentEnemyOnNextDriverSelection = false;
            utilitySkillDriver.noRepeat = false;
            utilitySkillDriver.shouldSprint = false;
            utilitySkillDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            utilitySkillDriver.driverUpdateTimerOverride = 0.5f;

            AISkillDriver utilityDriver = gameObject.AddComponent<AISkillDriver>();
            utilityDriver.customName = "Utility";
            utilityDriver.skillSlot = RoR2.SkillSlot.Utility;
            utilityDriver.requireSkillReady = true;
            utilityDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilityDriver.minDistance = 35f;
            utilityDriver.maxDistance = 120f;
            utilityDriver.selectionRequiresTargetLoS = true;
            utilityDriver.activationRequiresTargetLoS = true;
            utilityDriver.activationRequiresAimConfirmation = true;
            utilityDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            utilityDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            utilityDriver.ignoreNodeGraph = false;
            utilityDriver.resetCurrentEnemyOnNextDriverSelection = true;
            utilityDriver.noRepeat = false;
            utilityDriver.shouldSprint = false;
            utilityDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            utilityDriver.driverUpdateTimerOverride = 0.5f;

            AISkillDriver utilityReleaseDriver = gameObject.AddComponent<AISkillDriver>();
            utilityReleaseDriver.customName = "UtilityRelease";
            utilityReleaseDriver.skillSlot = RoR2.SkillSlot.Utility;
            utilityReleaseDriver.requireSkillReady = false;
            utilityReleaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilityReleaseDriver.minDistance = 0f;
            utilityReleaseDriver.maxDistance = 45f;
            utilityReleaseDriver.selectionRequiresTargetLoS = true;
            utilityReleaseDriver.activationRequiresTargetLoS = true;
            utilityReleaseDriver.activationRequiresAimConfirmation = true;
            utilityReleaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            utilityReleaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            utilityReleaseDriver.ignoreNodeGraph = false;
            utilityReleaseDriver.resetCurrentEnemyOnNextDriverSelection = false;
            utilityReleaseDriver.noRepeat = true;
            utilityReleaseDriver.shouldSprint = true;
            utilityReleaseDriver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

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

            AISkillDriver chaseSkill = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            chaseSkill.customName = "ChaseTarget";
            chaseSkill.skillSlot = RoR2.SkillSlot.None;
            chaseSkill.requireSkillReady = false;
            chaseSkill.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseSkill.minDistance = 20;
            chaseSkill.maxDistance = 60;
            chaseSkill.selectionRequiresTargetLoS = true;
            chaseSkill.activationRequiresTargetLoS = true;
            chaseSkill.activationRequiresAimConfirmation = false;
            chaseSkill.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseSkill.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseSkill.ignoreNodeGraph = false;
            chaseSkill.resetCurrentEnemyOnNextDriverSelection = false;
            chaseSkill.noRepeat = false;
            chaseSkill.shouldSprint = true;

            // Primary skill - will be used when bag is full
            primarySkillDriver = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            primarySkillDriver.customName = "Primary";
            primarySkillDriver.skillSlot = RoR2.SkillSlot.Primary;
            primarySkillDriver.requireSkillReady = false; // Don't require ready so it can spam
            primarySkillDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primarySkillDriver.minDistance = 0;
            primarySkillDriver.maxDistance = 10;
            primarySkillDriver.selectionRequiresTargetLoS = false; // Don't require LOS when bagged
            primarySkillDriver.activationRequiresTargetLoS = false; // Don't require LOS when bagged
            primarySkillDriver.activationRequiresAimConfirmation = false;
            primarySkillDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primarySkillDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            primarySkillDriver.ignoreNodeGraph = true;
            primarySkillDriver.resetCurrentEnemyOnNextDriverSelection = false;
            primarySkillDriver.noRepeat = false;
            primarySkillDriver.shouldSprint = false;
            primarySkillDriver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 0);

            // Cache all skill drivers for reordering
            allSkillDrivers = gameObject.GetComponents<AISkillDriver>();
        }

        public override void OnBodyChange()
        {
            base.OnBodyChange();
            this.bagController = null;
            this.swingCount = 0;
            this.wasBagFull = false;
            
            // Re-cache skill drivers when body changes
            if (controller?.body != null)
            {
                allSkillDrivers = controller.body.gameObject.GetComponents<AISkillDriver>();
                foreach (var driver in allSkillDrivers)
                {
                    if (driver.customName == "Primary")
                    {
                        primarySkillDriver = driver;
                    }
                    else if (driver.customName == "UtilityThrow")
                    {
                        utilitySkillDriver = driver;
                    }
                }
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            // Get bag controller if not cached
            if (this.bagController == null && controller?.body != null)
            {
                this.bagController = controller.body.GetComponent<DrifterBagController>();
            }

            if (this.bagController == null || primarySkillDriver == null || utilitySkillDriver == null)
            {
                return;
            }

            bool isBagFull = this.bagController.bagFull;

            // Track when bag status changes
            if (isBagFull && !wasBagFull)
            {
                // Just grabbed something, reset swing count
                swingCount = 0;
            }
            else if (!isBagFull && wasBagFull)
            {
                // Bag was emptied, reset swing count
                swingCount = 0;
            }

            wasBagFull = isBagFull;

            // Check if primary skill was just used (approximate by checking skill stock/cooldown)
            if (isBagFull && controller?.body?.skillLocator?.primary != null)
            {
                var primarySkill = controller.body.skillLocator.primary;
                
                // If skill is on cooldown, it was just used
                if (primarySkill.stock < primarySkill.maxStock)
                {
                    // Increment swing count (this will increment multiple times during cooldown,
                    // but we'll use a simple approach and check every few frames)
                    if (Time.frameCount % 30 == 0) // Check roughly every 0.5 seconds at 60fps
                    {
                        swingCount++;
                    }
                }
            }

            // Adjust skill driver priorities based on bag status and swing count
            if (isBagFull)
            {
                // Check if we should throw after enough swings
                bool shouldAttemptThrow = swingCount >= swingsBeforeThrow && Random.value < throwChance;

                if (shouldAttemptThrow && controller?.body?.skillLocator?.utility != null)
                {
                    var utilitySkill = controller.body.skillLocator.utility;
                    
                    // Only enable throw if utility is ready
                    if (utilitySkill.IsReady())
                    {
                        // Enable utility throw driver
                        utilitySkillDriver.minDistance = 0;
                        utilitySkillDriver.maxDistance = 999f;
                        
                        // Disable primary temporarily to allow throw
                        primarySkillDriver.minDistance = 0;
                        primarySkillDriver.maxDistance = 0; // Disable
                        
                        // Reset swing count after attempting throw
                        swingCount = 0;
                    }
                    else
                    {
                        // Utility not ready, continue smacking
                        EnablePrimaryDriver();
                        DisableUtilityThrowDriver();
                    }
                }
                else
                {
                    // Continue smacking
                    EnablePrimaryDriver();
                    DisableUtilityThrowDriver();
                }
            }
            else
            {
                // Reset to normal behavior when bag is empty
                primarySkillDriver.minDistance = 0;
                primarySkillDriver.maxDistance = 10;
                primarySkillDriver.requireSkillReady = false;
                primarySkillDriver.selectionRequiresTargetLoS = true;
                primarySkillDriver.activationRequiresTargetLoS = true;
                
                DisableUtilityThrowDriver();
            }
        }

        private void EnablePrimaryDriver()
        {
            primarySkillDriver.minDistance = 0;
            primarySkillDriver.maxDistance = 999f; // Always in range
            primarySkillDriver.requireSkillReady = false;
            primarySkillDriver.selectionRequiresTargetLoS = false;
            primarySkillDriver.activationRequiresTargetLoS = false;
        }

        private void DisableUtilityThrowDriver()
        {
            utilitySkillDriver.minDistance = 0;
            utilitySkillDriver.maxDistance = 0; // Disable
        }
    }
}
