using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("SpaceMarineBody")]
    [CustomSurvivor("https://thunderstore.io/package/DragonycksModdingComms/SpaceMarine/", "1.0.1")]
    class SpaceMarineHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill3_chase = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3_chase.customName = "UtilityChase";
            skill3_chase.skillSlot = RoR2.SkillSlot.Utility;
            skill3_chase.requireSkillReady = true;
            skill3_chase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3_chase.minDistance = 0;
            skill3_chase.maxDistance = 20;
            skill3_chase.selectionRequiresTargetLoS = true;
            skill3_chase.activationRequiresTargetLoS = true;
            skill3_chase.activationRequiresAimConfirmation = false;
            skill3_chase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill3_chase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill3_chase.ignoreNodeGraph = false;
            skill3_chase.resetCurrentEnemyOnNextDriverSelection = false;
            skill3_chase.noRepeat = false;
            skill3_chase.shouldSprint = true;

            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            skill4.minDistance = 0;
            skill4.maxDistance = 10;
            skill4.selectionRequiresTargetLoS = false;
            skill4.activationRequiresTargetLoS = false;
            skill4.activationRequiresAimConfirmation = false;
            skill4.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill4.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = false;
            skill4.shouldSprint = false;

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

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Shoot";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = true;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0;
            skill1.maxDistance = 50;
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
        }
    }
}
