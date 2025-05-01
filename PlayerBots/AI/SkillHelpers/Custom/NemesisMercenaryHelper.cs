using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("NemMercBody")]
    [CustomSurvivor("https://thunderstore.io/package/TeamMoonstorm/Starstorm2/", "0.6.17")]
    class NemesisMercenaryHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "Utility";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 0;
            skill3.maxDistance = 40;
            //skill3.maxUserHealthFraction = .25f;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = true;
            skill3.activationRequiresAimConfirmation = true;
            skill3.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill3.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill3.ignoreNodeGraph = false;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = false;
            skill3.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0;
            skill4.maxDistance = 40;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = false;
            skill4.activationRequiresAimConfirmation = true;
            skill4.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = true;
            skill4.shouldSprint = true;

            AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Secondary";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0;
            skill2.maxDistance = 20;
            skill2.selectionRequiresTargetLoS = true;
            skill2.activationRequiresTargetLoS = true;
            skill2.activationRequiresAimConfirmation = true;
            skill2.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill2.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill2.ignoreNodeGraph = false;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = true;

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Primary";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = true;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0;
            skill1.maxDistance = 30;
            skill1.selectionRequiresTargetLoS = true;
            skill1.activationRequiresTargetLoS = false;
            skill1.activationRequiresAimConfirmation = true;
            skill1.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill1.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill1.ignoreNodeGraph = true;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = true;
            skill1.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            skill3.nextHighPriorityOverride = skill1;
            skill4.nextHighPriorityOverride = skill3;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 10);
        }
    }
}