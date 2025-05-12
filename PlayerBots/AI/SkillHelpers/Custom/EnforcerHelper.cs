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
            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "Utility";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 0;
            skill3.maxDistance = 30;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = true;
            skill3.activationRequiresAimConfirmation = false;
            skill3.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill3.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill3.ignoreNodeGraph = false;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = false;

            AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Secondary";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0;
            skill2.maxDistance = 10;
            skill2.selectionRequiresTargetLoS = true;
            skill2.activationRequiresTargetLoS = true;
            skill2.activationRequiresAimConfirmation = false;
            skill2.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill2.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill2.ignoreNodeGraph = false;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = false;

            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0;
            skill4.maxDistance = 50;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = true;
            skill4.activationRequiresAimConfirmation = false;
            skill4.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = true;
            skill4.shouldSprint = false;

            AISkillDriver skill4_shoot = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4_shoot.customName = "SpecialShoot";
            skill4_shoot.skillSlot = RoR2.SkillSlot.Primary;
            skill4_shoot.requireSkillReady = true;
            skill4_shoot.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4_shoot.minDistance = 0;
            skill4_shoot.maxDistance = 50;
            skill4_shoot.selectionRequiresTargetLoS = true;
            skill4_shoot.activationRequiresTargetLoS = true;
            skill4_shoot.activationRequiresAimConfirmation = true;
            skill4_shoot.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4_shoot.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4_shoot.ignoreNodeGraph = false;
            skill4_shoot.resetCurrentEnemyOnNextDriverSelection = true;
            skill4_shoot.noRepeat = false;
            skill4_shoot.shouldSprint = false;
            skill4_shoot.driverUpdateTimerOverride = 3f;
            skill4_shoot.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 0);

            // Set up the skill chain
            skill4.nextHighPriorityOverride = skill4_shoot;
            skill4_shoot.nextHighPriorityOverride = skill2;
            skill2.nextHighPriorityOverride = skill3;
        }
    }
}