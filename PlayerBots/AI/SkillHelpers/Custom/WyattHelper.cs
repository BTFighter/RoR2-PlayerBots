using Modules.Characters;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("WyattBody")]
    [CustomSurvivor("https://thunderstore.io/package/Cloudburst/Custodian/", "1.0.0")]
    class WyattHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            ai.aimVectorDampTime = 0.1f;
            ai.aimVectorMaxSpeed = 180f;

            // Primary: Melee Combo - Close range combat with 3-step combo
            AISkillDriver primaryClose = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            primaryClose.customName = "Primary Close Range";
            primaryClose.skillSlot = RoR2.SkillSlot.Primary;
            primaryClose.requireSkillReady = false;
            primaryClose.minDistance = 0;
            primaryClose.maxDistance = 6;
            primaryClose.selectionRequiresTargetLoS = false;
            primaryClose.selectionRequiresOnGround = false;
            primaryClose.selectionRequiresAimTarget = false;
            primaryClose.maxTimesSelected = -1;
            primaryClose.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryClose.activationRequiresTargetLoS = false;
            primaryClose.activationRequiresAimTargetLoS = false;
            primaryClose.activationRequiresAimConfirmation = true;
            primaryClose.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            primaryClose.moveInputScale = 0.3f;
            primaryClose.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryClose.ignoreNodeGraph = true;
            primaryClose.shouldSprint = false;
            primaryClose.shouldFireEquipment = false;
            primaryClose.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryClose.driverUpdateTimerOverride = -1;
            primaryClose.resetCurrentEnemyOnNextDriverSelection = false;
            primaryClose.noRepeat = false;
            primaryClose.nextHighPriorityOverride = null;

            // Primary: Medium Range Melee Combat
            AISkillDriver primaryMedium = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            primaryMedium.customName = "Primary Medium Range";
            primaryMedium.skillSlot = RoR2.SkillSlot.Primary;
            primaryMedium.requireSkillReady = false;
            primaryMedium.minDistance = 4;
            primaryMedium.maxDistance = 12;
            primaryMedium.selectionRequiresTargetLoS = false;
            primaryMedium.selectionRequiresOnGround = false;
            primaryMedium.selectionRequiresAimTarget = false;
            primaryMedium.maxTimesSelected = -1;
            primaryMedium.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryMedium.activationRequiresTargetLoS = false;
            primaryMedium.activationRequiresAimTargetLoS = false;
            primaryMedium.activationRequiresAimConfirmation = true;
            primaryMedium.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primaryMedium.moveInputScale = 0.8f;
            primaryMedium.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryMedium.ignoreNodeGraph = true;
            primaryMedium.shouldSprint = false;
            primaryMedium.shouldFireEquipment = false;
            primaryMedium.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryMedium.driverUpdateTimerOverride = -1;
            primaryMedium.resetCurrentEnemyOnNextDriverSelection = false;
            primaryMedium.noRepeat = false;
            primaryMedium.nextHighPriorityOverride = null;

            // Secondary: TrashOut - Gap closing mobility attack
            AISkillDriver trashOut = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            trashOut.customName = "Secondary TrashOut";
            trashOut.requireSkillReady = true;
            trashOut.minDistance = 8;
            trashOut.maxDistance = 55;
            trashOut.selectionRequiresTargetLoS = false;
            trashOut.selectionRequiresOnGround = false;
            trashOut.selectionRequiresAimTarget = true;
            trashOut.maxTimesSelected = -1;
            trashOut.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            trashOut.activationRequiresTargetLoS = false;
            trashOut.activationRequiresAimTargetLoS = true;
            trashOut.activationRequiresAimConfirmation = false;
            trashOut.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            trashOut.moveInputScale = 1.5f;
            trashOut.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            trashOut.ignoreNodeGraph = true;
            trashOut.shouldSprint = true;
            trashOut.shouldFireEquipment = false;
            trashOut.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            trashOut.driverUpdateTimerOverride = -1;
            trashOut.resetCurrentEnemyOnNextDriverSelection = false;
            trashOut.noRepeat = true;
            trashOut.nextHighPriorityOverride = null;

            // Special: Deploy MAID - Ranged projectile attack
            AISkillDriver maidRanged = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            maidRanged.customName = "Special MAID Ranged";
            maidRanged.skillSlot = RoR2.SkillSlot.Special;
            maidRanged.requireSkillReady = true;
            maidRanged.minDistance = 12;
            maidRanged.maxDistance = 60;
            maidRanged.selectionRequiresTargetLoS = false;
            maidRanged.selectionRequiresOnGround = false;
            maidRanged.selectionRequiresAimTarget = false;
            maidRanged.maxTimesSelected = -1;
            maidRanged.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            maidRanged.activationRequiresTargetLoS = false;
            maidRanged.activationRequiresAimTargetLoS = false;
            maidRanged.activationRequiresAimConfirmation = true;
            maidRanged.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            maidRanged.moveInputScale = 0.7f;
            maidRanged.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            maidRanged.ignoreNodeGraph = false;
            maidRanged.shouldSprint = false;
            maidRanged.shouldFireEquipment = false;
            maidRanged.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            maidRanged.driverUpdateTimerOverride = -1;
            maidRanged.resetCurrentEnemyOnNextDriverSelection = false;
            maidRanged.noRepeat = false;
            maidRanged.nextHighPriorityOverride = null;

            // Special: Retrieve MAID - Retrieve MAID when it's been deployed
            AISkillDriver retrieveMaid = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            retrieveMaid.customName = "Retrieve MAID";
            retrieveMaid.skillSlot = RoR2.SkillSlot.Special;
            retrieveMaid.requireSkillReady = false;
            retrieveMaid.minDistance = 0;
            retrieveMaid.maxDistance = float.PositiveInfinity;
            retrieveMaid.selectionRequiresTargetLoS = false;
            retrieveMaid.selectionRequiresOnGround = false;
            retrieveMaid.selectionRequiresAimTarget = false;
            retrieveMaid.maxTimesSelected = 1;
            retrieveMaid.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            retrieveMaid.activationRequiresTargetLoS = false;
            retrieveMaid.activationRequiresAimTargetLoS = false;
            retrieveMaid.activationRequiresAimConfirmation = false;
            retrieveMaid.movementType = AISkillDriver.MovementType.Stop;
            retrieveMaid.moveInputScale = 1;
            retrieveMaid.aimType = AISkillDriver.AimType.MoveDirection;
            retrieveMaid.ignoreNodeGraph = false;
            retrieveMaid.shouldSprint = false;
            retrieveMaid.shouldFireEquipment = false;
            retrieveMaid.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            retrieveMaid.driverUpdateTimerOverride = -1;
            retrieveMaid.resetCurrentEnemyOnNextDriverSelection = false;
            retrieveMaid.noRepeat = true;
            retrieveMaid.nextHighPriorityOverride = null;

            // Utility: Flow - Buff activation for enhanced combat
            AISkillDriver flowActivation = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            flowActivation.customName = "Utility Flow";
            flowActivation.skillSlot = RoR2.SkillSlot.Utility;
            flowActivation.requireSkillReady = true;
            flowActivation.minDistance = 0;
            flowActivation.maxDistance = 80;
            flowActivation.selectionRequiresTargetLoS = false;
            flowActivation.selectionRequiresOnGround = false;
            flowActivation.selectionRequiresAimTarget = false;
            flowActivation.maxTimesSelected = 1;
            flowActivation.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            flowActivation.activationRequiresTargetLoS = false;
            flowActivation.activationRequiresAimTargetLoS = false;
            flowActivation.activationRequiresAimConfirmation = false;
            flowActivation.movementType = AISkillDriver.MovementType.Stop;
            flowActivation.moveInputScale = 1;
            flowActivation.aimType = AISkillDriver.AimType.MoveDirection;
            flowActivation.ignoreNodeGraph = false;
            flowActivation.shouldSprint = false;
            flowActivation.shouldFireEquipment = false;
            flowActivation.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            flowActivation.driverUpdateTimerOverride = 5f;
            flowActivation.resetCurrentEnemyOnNextDriverSelection = false;
            flowActivation.noRepeat = true;
            flowActivation.nextHighPriorityOverride = null;

            // Chase: Default movement behavior
            AISkillDriver chase = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            chase.customName = "Chase";
            chase.skillSlot = RoR2.SkillSlot.None;
            chase.requireSkillReady = false;
            chase.minDistance = 0;
            chase.maxDistance = 60;
            chase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chase.activationRequiresTargetLoS = false;
            chase.activationRequiresAimTargetLoS = false;
            chase.activationRequiresAimConfirmation = false;
            chase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chase.moveInputScale = 1;
            chase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            chase.ignoreNodeGraph = false;
            chase.driverUpdateTimerOverride = -1;
            chase.resetCurrentEnemyOnNextDriverSelection = false;
            chase.noRepeat = false;
            chase.shouldSprint = false;
            chase.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Strafe: Movement for medium range combat
            AISkillDriver strafe = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            strafe.customName = "Strafe";
            strafe.skillSlot = RoR2.SkillSlot.None;
            strafe.requireSkillReady = false;
            strafe.minDistance = 6;
            strafe.maxDistance = 25;
            strafe.selectionRequiresTargetLoS = false;
            strafe.selectionRequiresOnGround = false;
            strafe.selectionRequiresAimTarget = true;
            strafe.maxTimesSelected = -1;
            strafe.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafe.activationRequiresTargetLoS = false;
            strafe.activationRequiresAimTargetLoS = false;
            strafe.activationRequiresAimConfirmation = false;
            strafe.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            strafe.moveInputScale = 1;
            strafe.aimType = AISkillDriver.AimType.AtMoveTarget;
            strafe.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            strafe.shouldSprint = false;
            strafe.ignoreNodeGraph = false;
            strafe.driverUpdateTimerOverride = -1;
            strafe.resetCurrentEnemyOnNextDriverSelection = false;
            strafe.noRepeat = true;

            // Sprint: Use mobility skills for repositioning
            AISkillDriver sprint = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            sprint.customName = "Sprint";
            sprint.skillSlot = RoR2.SkillSlot.None;
            sprint.requireSkillReady = false;
            sprint.minDistance = 20;
            sprint.maxDistance = 80;
            sprint.selectionRequiresTargetLoS = false;
            sprint.selectionRequiresOnGround = true;
            sprint.selectionRequiresAimTarget = true;
            sprint.maxTimesSelected = -1;
            sprint.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            sprint.activationRequiresTargetLoS = false;
            sprint.activationRequiresAimTargetLoS = false;
            sprint.activationRequiresAimConfirmation = false;
            sprint.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            sprint.moveInputScale = 1.5f;
            sprint.aimType = AISkillDriver.AimType.AtMoveTarget;
            sprint.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            sprint.shouldSprint = true;
            sprint.ignoreNodeGraph = false;
            sprint.driverUpdateTimerOverride = -1;
            sprint.resetCurrentEnemyOnNextDriverSelection = false;
            sprint.noRepeat = true;

            // Stop sprinting when too close
            AISkillDriver stopSprint = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            stopSprint.customName = "Stop Sprinting";
            stopSprint.skillSlot = RoR2.SkillSlot.None;
            stopSprint.requireSkillReady = false;
            stopSprint.minDistance = 0;
            stopSprint.maxDistance = 15;
            stopSprint.selectionRequiresTargetLoS = false;
            stopSprint.selectionRequiresOnGround = false;
            stopSprint.selectionRequiresAimTarget = false;
            stopSprint.maxTimesSelected = -1;
            stopSprint.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            stopSprint.activationRequiresTargetLoS = false;
            stopSprint.activationRequiresAimTargetLoS = false;
            stopSprint.activationRequiresAimConfirmation = false;
            stopSprint.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            stopSprint.moveInputScale = 1;
            stopSprint.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            stopSprint.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            stopSprint.shouldSprint = false;
            stopSprint.ignoreNodeGraph = false;
            stopSprint.driverUpdateTimerOverride = -1;
            stopSprint.resetCurrentEnemyOnNextDriverSelection = false;
            stopSprint.noRepeat = false;

            // Set up skill chain priorities for proper AI behavior
            primaryClose.nextHighPriorityOverride = primaryMedium;
            primaryMedium.nextHighPriorityOverride = trashOut;
            trashOut.nextHighPriorityOverride = maidRanged;
            maidRanged.nextHighPriorityOverride = flowActivation;
            flowActivation.nextHighPriorityOverride = chase;

            // Add default skills for fallback behavior
            AddDefaultSkills(gameObject, ai, 10);
        }
    }
}