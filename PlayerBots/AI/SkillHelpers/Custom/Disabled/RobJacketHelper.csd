using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("RobJacketBody")]
    [CustomSurvivor("https://thunderstore.io/package/EnforcerGang/Enforcer/", "3.7.4")]
    class NemesisEnforcerHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver aiskillDriver = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver.customName = "SideDodge";
            aiskillDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            aiskillDriver.moveTargetType = 0;
            aiskillDriver.activationRequiresAimConfirmation = true;
            aiskillDriver.activationRequiresTargetLoS = false;
            aiskillDriver.selectionRequiresTargetLoS = true;
            aiskillDriver.maxDistance = 10;
            aiskillDriver.minDistance = 0;
            aiskillDriver.requireSkillReady = true;
            aiskillDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aiskillDriver.ignoreNodeGraph = true;
            aiskillDriver.moveInputScale = 1f;
            aiskillDriver.driverUpdateTimerOverride = 0.15f;
            aiskillDriver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            aiskillDriver.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver.maxUserHealthFraction = 0.99f;
            aiskillDriver.skillSlot = RoR2.SkillSlot.Utility;

            AISkillDriver aiskillDriver2 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver2.customName = "Dodge";
            aiskillDriver2.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            aiskillDriver2.moveTargetType = 0;
            aiskillDriver2.activationRequiresAimConfirmation = true;
            aiskillDriver2.activationRequiresTargetLoS = false;
            aiskillDriver2.selectionRequiresTargetLoS = true;
            aiskillDriver2.maxDistance = 24;
            aiskillDriver2.minDistance = 9;
            aiskillDriver2.requireSkillReady = true;
            aiskillDriver2.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aiskillDriver2.ignoreNodeGraph = true;
            aiskillDriver2.moveInputScale = 1f;
            aiskillDriver2.driverUpdateTimerOverride = 0.15f;
            aiskillDriver2.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            aiskillDriver2.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver2.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver2.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver2.maxUserHealthFraction = 0.99f;
            aiskillDriver2.skillSlot = RoR2.SkillSlot.Utility;

            AISkillDriver aiskillDriver3 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver3.customName = "Aim";
            aiskillDriver3.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            aiskillDriver3.moveTargetType = 0;
            aiskillDriver3.activationRequiresAimConfirmation = true;
            aiskillDriver3.activationRequiresTargetLoS = true;
            aiskillDriver3.maxDistance = 150;
            aiskillDriver3.minDistance = 10;
            aiskillDriver3.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aiskillDriver3.ignoreNodeGraph = false;
            aiskillDriver3.moveInputScale = 1f;
            aiskillDriver3.buttonPressType = 0;
            aiskillDriver3.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver3.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver3.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver3.maxUserHealthFraction = float.PositiveInfinity;
            aiskillDriver3.skillSlot = RoR2.SkillSlot.Secondary;
            aiskillDriver3.selectionRequiresAimTarget = true;

            AISkillDriver aiskillDriver5 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver5.customName = "Strafe";
            aiskillDriver5.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            aiskillDriver5.moveTargetType = 0;
            aiskillDriver5.activationRequiresAimConfirmation = false;
            aiskillDriver5.activationRequiresTargetLoS = false;
            aiskillDriver5.maxDistance = 50;
            aiskillDriver5.minDistance = 25;
            aiskillDriver5.aimType = AISkillDriver.AimType.AtMoveTarget;
            aiskillDriver5.ignoreNodeGraph = false;
            aiskillDriver5.moveInputScale = 1f;
            aiskillDriver5.driverUpdateTimerOverride = -1f;
            aiskillDriver5.buttonPressType = 0;
            aiskillDriver5.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver5.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver5.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver5.maxUserHealthFraction = float.PositiveInfinity;

            AISkillDriver aiskillDriver6 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver6.customName = "Flee";
            aiskillDriver6.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            aiskillDriver6.moveTargetType = 0;
            aiskillDriver6.activationRequiresAimConfirmation = false;
            aiskillDriver6.activationRequiresTargetLoS = false;
            aiskillDriver6.maxDistance = 40;
            aiskillDriver6.minDistance = 6;
            aiskillDriver6.aimType = AISkillDriver.AimType.AtMoveTarget;
            aiskillDriver6.ignoreNodeGraph = false;
            aiskillDriver6.moveInputScale = 1f;
            aiskillDriver6.driverUpdateTimerOverride = -1f;
            aiskillDriver6.buttonPressType = 0;
            aiskillDriver6.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver6.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver6.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver6.maxUserHealthFraction = float.PositiveInfinity;
            aiskillDriver6.shouldSprint = true;

            AISkillDriver aiskillDriver7 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver7.customName = "Knife";
            aiskillDriver7.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            aiskillDriver7.moveTargetType = 0;
            aiskillDriver7.activationRequiresAimConfirmation = false;
            aiskillDriver7.activationRequiresTargetLoS = false;
            aiskillDriver7.maxDistance = 10;
            aiskillDriver7.minDistance = 0;
            aiskillDriver7.aimType = AISkillDriver.AimType.AtMoveTarget;
            aiskillDriver7.ignoreNodeGraph = false;
            aiskillDriver7.moveInputScale = 1f;
            aiskillDriver7.driverUpdateTimerOverride = 0.01f;
            aiskillDriver7.buttonPressType = 0;
            aiskillDriver7.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver7.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver7.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver7.maxUserHealthFraction = float.PositiveInfinity;
            aiskillDriver7.skillSlot = 0;
            aiskillDriver7.shouldSprint = false;

            AISkillDriver aiskillDriver8 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver8.customName = "Chase";
            aiskillDriver8.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            aiskillDriver8.moveTargetType = 0;
            aiskillDriver8.activationRequiresAimConfirmation = false;
            aiskillDriver8.activationRequiresTargetLoS = false;
            aiskillDriver8.maxDistance = float.PositiveInfinity;
            aiskillDriver8.minDistance = 0f;
            aiskillDriver8.aimType = AISkillDriver.AimType.AtMoveTarget;
            aiskillDriver8.ignoreNodeGraph = false;
            aiskillDriver8.moveInputScale = 1f;
            aiskillDriver8.driverUpdateTimerOverride = -1f;
            aiskillDriver8.buttonPressType = 0;
            aiskillDriver8.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver8.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver8.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver8.maxUserHealthFraction = float.PositiveInfinity;
            aiskillDriver8.shouldSprint = true;

            base.AddDefaultSkills(gameObject, ai, 0f);
        }
    }
}
