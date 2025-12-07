using EnforcerPlugin;
using Modules.Characters;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("NemesisEnforcerBody")]
    [CustomSurvivor("https://thunderstore.io/package/EnforcerGang/Enforcer/", "3.11.6")]
    class NemesisEnforcerHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            ai.aimVectorMaxSpeed = 40f;
            ai.aimVectorDampTime = 0.2f;

            // HammerTap - Quick hammer attack at close range
            AISkillDriver hammerTap = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            hammerTap.customName = "HammerTap";
            hammerTap.skillSlot = RoR2.SkillSlot.Secondary;
            hammerTap.requireSkillReady = true;
            hammerTap.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            hammerTap.minDistance = 2;
            hammerTap.maxDistance = 8;
            hammerTap.selectionRequiresTargetLoS = true;
            hammerTap.activationRequiresTargetLoS = true;
            hammerTap.activationRequiresAimConfirmation = true;
            hammerTap.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            hammerTap.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            hammerTap.ignoreNodeGraph = true;
            hammerTap.driverUpdateTimerOverride = 1.5f;
            hammerTap.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            hammerTap.noRepeat = true;
            hammerTap.requiredSkill = NemforcerPlugin.hammerChargeDef;

			// HammerCharge - Charged hammer attack at medium range
			AISkillDriver hammerCharge = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            hammerCharge.customName = "HammerCharge";
            hammerCharge.skillSlot = RoR2.SkillSlot.Secondary;
            hammerCharge.requireSkillReady = true;
            hammerCharge.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            hammerCharge.minDistance = 12;
            hammerCharge.maxDistance = 24;
            hammerCharge.selectionRequiresTargetLoS = true;
            hammerCharge.activationRequiresTargetLoS = true;
            hammerCharge.activationRequiresAimConfirmation = true;
            hammerCharge.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            hammerCharge.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            hammerCharge.ignoreNodeGraph = true;
            hammerCharge.driverUpdateTimerOverride = 2.5f;
            hammerCharge.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            hammerCharge.noRepeat = true;
			hammerCharge.requiredSkill = NemforcerPlugin.hammerChargeDef;

			// HammerCloseRange - Close combat hammer swing
			AISkillDriver hammerClose = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            hammerClose.customName = "HammerCloseRange";
            hammerClose.skillSlot = RoR2.SkillSlot.Primary;
            hammerClose.requireSkillReady = true;
            hammerClose.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            hammerClose.minDistance = 0;
            hammerClose.maxDistance = 3;
            hammerClose.selectionRequiresTargetLoS = true;
            hammerClose.activationRequiresTargetLoS = true;
            hammerClose.activationRequiresAimConfirmation = true;
            hammerClose.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            hammerClose.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            hammerClose.ignoreNodeGraph = true;
            hammerClose.driverUpdateTimerOverride = 0.5f;
            hammerClose.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            hammerClose.moveInputScale = 0.4f;
			hammerClose.requiredSkill = NemforcerPlugin.hammerSwingDef;

			// WalkAndHammer - Medium range hammer combat
			AISkillDriver walkHammer = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            walkHammer.customName = "WalkAndHammer";
            walkHammer.skillSlot = RoR2.SkillSlot.Primary;
            walkHammer.requireSkillReady = true;
            walkHammer.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            walkHammer.minDistance = 0;
            walkHammer.maxDistance = 12;
            walkHammer.selectionRequiresTargetLoS = true;
            walkHammer.activationRequiresTargetLoS = true;
            walkHammer.activationRequiresAimConfirmation = true;
            walkHammer.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            walkHammer.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            walkHammer.ignoreNodeGraph = true;
            walkHammer.driverUpdateTimerOverride = 0.5f;
            walkHammer.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			walkHammer.requiredSkill = NemforcerPlugin.hammerSwingDef;

			// MinigunSecondary - Hammer slam for area damage
			AISkillDriver minigunSecondary = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            minigunSecondary.customName = "MinigunSecondary";
            minigunSecondary.skillSlot = RoR2.SkillSlot.Secondary;
            minigunSecondary.requireSkillReady = true;
            minigunSecondary.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            minigunSecondary.minDistance = 0;
            minigunSecondary.maxDistance = 14;
            minigunSecondary.selectionRequiresTargetLoS = true;
            minigunSecondary.activationRequiresTargetLoS = true;
            minigunSecondary.activationRequiresAimConfirmation = true;
            minigunSecondary.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            minigunSecondary.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            minigunSecondary.ignoreNodeGraph = false;
            minigunSecondary.driverUpdateTimerOverride = -1f;
            minigunSecondary.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			minigunSecondary.requiredSkill = NemforcerPlugin.hammerSlamDef;

			// SwapToHammer - Switch to hammer combat mode
			AISkillDriver swapHammer = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            swapHammer.customName = "SwapToHammer";
            swapHammer.skillSlot = RoR2.SkillSlot.Special;
            swapHammer.requireSkillReady = true;
            swapHammer.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            swapHammer.minDistance = 0;
            swapHammer.maxDistance = 12;
            swapHammer.selectionRequiresTargetLoS = false;
            swapHammer.activationRequiresTargetLoS = false;
            swapHammer.activationRequiresAimConfirmation = false;
            swapHammer.movementType = AISkillDriver.MovementType.Stop;
            swapHammer.aimType = AISkillDriver.AimType.MoveDirection;
            swapHammer.ignoreNodeGraph = false;
            swapHammer.driverUpdateTimerOverride = 0.5f;
            swapHammer.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			swapHammer.requiredSkill = NemforcerPlugin.minigunUpDef;

			// StrafeAndShoot - Minigun ranged combat
			AISkillDriver strafeShoot = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            strafeShoot.customName = "StrafeAndShoot";
            strafeShoot.skillSlot = RoR2.SkillSlot.Primary;
            strafeShoot.requireSkillReady = true;
            strafeShoot.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafeShoot.minDistance = 8;
            strafeShoot.maxDistance = 80;
            strafeShoot.selectionRequiresTargetLoS = true;
            strafeShoot.activationRequiresTargetLoS = true;
            strafeShoot.activationRequiresAimConfirmation = true;
            strafeShoot.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            strafeShoot.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            strafeShoot.ignoreNodeGraph = false;
            strafeShoot.driverUpdateTimerOverride = -1f;
            strafeShoot.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			strafeShoot.requiredSkill = NemforcerPlugin.minigunFireDef;

			// SwapToMinigun - Switch to minigun combat mode
			AISkillDriver swapMinigun = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            swapMinigun.customName = "SwapToMinigun";
            swapMinigun.skillSlot = RoR2.SkillSlot.Special;
            swapMinigun.requireSkillReady = true;
            swapMinigun.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            swapMinigun.minDistance = 30;
            swapMinigun.maxDistance = 90;
            swapMinigun.selectionRequiresTargetLoS = false;
            swapMinigun.activationRequiresTargetLoS = false;
            swapMinigun.activationRequiresAimConfirmation = false;
            swapMinigun.movementType = AISkillDriver.MovementType.Stop;
            swapMinigun.aimType = AISkillDriver.AimType.MoveDirection;
            swapMinigun.ignoreNodeGraph = false;
            swapMinigun.driverUpdateTimerOverride = -1f;
            swapMinigun.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			swapMinigun.requiredSkill = NemforcerPlugin.minigunDownDef;

			// ThrowGrenade - Utility grenade attack
			AISkillDriver throwGrenade = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            throwGrenade.customName = "ThrowGrenade";
            throwGrenade.skillSlot = RoR2.SkillSlot.Utility;
            throwGrenade.requireSkillReady = true;
            throwGrenade.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            throwGrenade.minDistance = 0;
            throwGrenade.maxDistance = 64;
            throwGrenade.selectionRequiresTargetLoS = true;
            throwGrenade.activationRequiresTargetLoS = false;
            throwGrenade.activationRequiresAimConfirmation = true;
            throwGrenade.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            throwGrenade.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            throwGrenade.ignoreNodeGraph = false;
            throwGrenade.driverUpdateTimerOverride = 0.5f;
            throwGrenade.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

			// Chase - Default movement behavior
			//AISkillDriver chase = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
   //         chase.customName = "Chase";
   //         chase.skillSlot = RoR2.SkillSlot.None;
   //         chase.requireSkillReady = false;
   //         chase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
   //         chase.minDistance = 0;
   //         chase.maxDistance = float.PositiveInfinity;
   //         chase.selectionRequiresTargetLoS = false;
   //         chase.activationRequiresTargetLoS = false;
   //         chase.activationRequiresAimConfirmation = false;
   //         chase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
   //         chase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
   //         chase.ignoreNodeGraph = false;
   //         chase.driverUpdateTimerOverride = -1f;
   //         chase.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 10);
        }
    }
}
