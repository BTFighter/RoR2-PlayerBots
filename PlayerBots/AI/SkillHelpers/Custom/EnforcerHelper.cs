using Modules.Characters;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("EnforcerBody")]
    [CustomSurvivor("https://thunderstore.io/package/EnforcerGang/Enforcer/", "3.11.6")]
    class EnforcerHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            // Configure base AI settings
            ai.aimVectorMaxSpeed = 40f;
            ai.aimVectorDampTime = 0.2f;

            // ExitShield - Exit shield when far from enemy
            AISkillDriver exitShield = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            exitShield.customName = "ExitShield";
            exitShield.movementType = AISkillDriver.MovementType.Stop;
            exitShield.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            exitShield.activationRequiresAimConfirmation = false;
            exitShield.activationRequiresTargetLoS = false;
            exitShield.selectionRequiresTargetLoS = false;
            exitShield.maxDistance = 512f;
            exitShield.minDistance = 45f;
            exitShield.requireSkillReady = true;
            exitShield.aimType = AISkillDriver.AimType.MoveDirection;
            exitShield.ignoreNodeGraph = false;
            exitShield.moveInputScale = 1f;
            exitShield.driverUpdateTimerOverride = 0.5f;
            exitShield.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            exitShield.minTargetHealthFraction = float.NegativeInfinity;
            exitShield.maxTargetHealthFraction = float.PositiveInfinity;
            exitShield.minUserHealthFraction = float.NegativeInfinity;
            exitShield.maxUserHealthFraction = float.PositiveInfinity;
            exitShield.skillSlot = SkillSlot.Special;
            exitShield.requiredSkill = EnforcerSurvivor.shieldExitDef;

            // ShoulderBash - Close range bash attack
            AISkillDriver shoulderBash = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            shoulderBash.customName = "ShoulderBash";
            shoulderBash.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shoulderBash.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shoulderBash.activationRequiresAimConfirmation = true;
            shoulderBash.activationRequiresTargetLoS = false;
            shoulderBash.selectionRequiresTargetLoS = true;
            shoulderBash.maxDistance = 6f;
            shoulderBash.minDistance = 0f;
            shoulderBash.requireSkillReady = true;
            shoulderBash.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shoulderBash.ignoreNodeGraph = true;
            shoulderBash.moveInputScale = 1f;
            shoulderBash.driverUpdateTimerOverride = 2f;
            shoulderBash.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            shoulderBash.minTargetHealthFraction = float.NegativeInfinity;
            shoulderBash.maxTargetHealthFraction = float.PositiveInfinity;
            shoulderBash.minUserHealthFraction = float.NegativeInfinity;
            shoulderBash.maxUserHealthFraction = float.PositiveInfinity;
            shoulderBash.skillSlot = SkillSlot.Secondary;
            shoulderBash.shouldSprint = true;

            // EnterShield - Enter shield when close to enemy
            AISkillDriver enterShield = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            enterShield.customName = "EnterShield";
            enterShield.movementType = AISkillDriver.MovementType.Stop;
            enterShield.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            enterShield.activationRequiresAimConfirmation = false;
            enterShield.activationRequiresTargetLoS = false;
            enterShield.selectionRequiresTargetLoS = false;
            enterShield.maxDistance = 30f;
            enterShield.minDistance = 0f;
            enterShield.requireSkillReady = true;
            enterShield.aimType = AISkillDriver.AimType.MoveDirection;
            enterShield.ignoreNodeGraph = true;
            enterShield.moveInputScale = 1f;
            enterShield.driverUpdateTimerOverride = -1f;
            enterShield.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            enterShield.minTargetHealthFraction = float.NegativeInfinity;
            enterShield.maxTargetHealthFraction = float.PositiveInfinity;
            enterShield.minUserHealthFraction = float.NegativeInfinity;
            enterShield.maxUserHealthFraction = float.PositiveInfinity;
            enterShield.skillSlot = SkillSlot.Special;
            enterShield.requiredSkill = EnforcerSurvivor.shieldEnterDef;

            // ShieldBash - Bash while in shield
            AISkillDriver shieldBash = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            shieldBash.customName = "ShieldBash";
            shieldBash.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shieldBash.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shieldBash.activationRequiresAimConfirmation = false;
            shieldBash.activationRequiresTargetLoS = false;
            shieldBash.selectionRequiresTargetLoS = false;
            shieldBash.maxDistance = 6f;
            shieldBash.minDistance = 0f;
            shieldBash.requireSkillReady = true;
            shieldBash.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shieldBash.ignoreNodeGraph = true;
            shieldBash.moveInputScale = 1f;
            shieldBash.driverUpdateTimerOverride = -1f;
            shieldBash.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shieldBash.minTargetHealthFraction = float.NegativeInfinity;
            shieldBash.maxTargetHealthFraction = float.PositiveInfinity;
            shieldBash.minUserHealthFraction = float.NegativeInfinity;
            shieldBash.maxUserHealthFraction = float.PositiveInfinity;
            shieldBash.skillSlot = SkillSlot.Secondary;

            // StandAndShoot - Shoot when at medium range
            AISkillDriver standAndShoot = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            standAndShoot.customName = "StandAndShoot";
            standAndShoot.movementType = AISkillDriver.MovementType.Stop;
            standAndShoot.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            standAndShoot.activationRequiresAimConfirmation = true;
            standAndShoot.activationRequiresTargetLoS = false;
            standAndShoot.selectionRequiresTargetLoS = false;
            standAndShoot.maxDistance = 40f;
            standAndShoot.minDistance = 0f;
            standAndShoot.requireSkillReady = true;
            standAndShoot.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            standAndShoot.ignoreNodeGraph = true;
            standAndShoot.moveInputScale = 1f;
            standAndShoot.driverUpdateTimerOverride = -1f;
            standAndShoot.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            standAndShoot.minTargetHealthFraction = float.NegativeInfinity;
            standAndShoot.maxTargetHealthFraction = float.PositiveInfinity;
            standAndShoot.minUserHealthFraction = float.NegativeInfinity;
            standAndShoot.maxUserHealthFraction = float.PositiveInfinity;
            standAndShoot.skillSlot = SkillSlot.Primary;

            // StrafeAndShoot - Strafe while shooting at medium range
            AISkillDriver strafeAndShoot = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            strafeAndShoot.customName = "StrafeAndShoot";
            strafeAndShoot.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            strafeAndShoot.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafeAndShoot.activationRequiresAimConfirmation = true;
            strafeAndShoot.activationRequiresTargetLoS = false;
            strafeAndShoot.selectionRequiresTargetLoS = false;
            strafeAndShoot.maxDistance = 50f;
            strafeAndShoot.minDistance = 8f;
            strafeAndShoot.requireSkillReady = true;
            strafeAndShoot.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            strafeAndShoot.ignoreNodeGraph = false;
            strafeAndShoot.moveInputScale = 1f;
            strafeAndShoot.driverUpdateTimerOverride = -1f;
            strafeAndShoot.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            strafeAndShoot.minTargetHealthFraction = float.NegativeInfinity;
            strafeAndShoot.maxTargetHealthFraction = float.PositiveInfinity;
            strafeAndShoot.minUserHealthFraction = float.NegativeInfinity;
            strafeAndShoot.maxUserHealthFraction = float.PositiveInfinity;
            strafeAndShoot.skillSlot = SkillSlot.Primary;

            // Chase - Move towards enemy when no other actions
            //AISkillDriver chase = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            //chase.customName = "Chase";
            //chase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            //chase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //chase.activationRequiresAimConfirmation = false;
            //chase.activationRequiresTargetLoS = false;
            //chase.maxDistance = float.PositiveInfinity;
            //chase.minDistance = 0f;
            //chase.aimType = AISkillDriver.AimType.AtMoveTarget;
            //chase.ignoreNodeGraph = false;
            //chase.moveInputScale = 1f;
            //chase.driverUpdateTimerOverride = -1f;
            //chase.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            //chase.minTargetHealthFraction = float.NegativeInfinity;
            //chase.maxTargetHealthFraction = float.PositiveInfinity;
            //chase.minUserHealthFraction = float.NegativeInfinity;
            //chase.maxUserHealthFraction = float.PositiveInfinity;
            //chase.skillSlot = SkillSlot.None;

            // Set up skill chain priorities
            // Shield exit has highest priority (when far away)
            exitShield.nextHighPriorityOverride = enterShield;
            enterShield.nextHighPriorityOverride = shoulderBash;
            shoulderBash.nextHighPriorityOverride = shieldBash;
            shieldBash.nextHighPriorityOverride = standAndShoot;
            standAndShoot.nextHighPriorityOverride = strafeAndShoot;

            // Add default skills for fallback behavior
            AddDefaultSkills(gameObject, ai, 0);
        }
    }
}