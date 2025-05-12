using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("GnomeChefBody")]
    [CustomSurvivor("https://thunderstore.io/package/Gnome/ChefMod/", "2.3.10")]
    class ChefModHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0;
            skill4.maxDistance = float.PositiveInfinity;
            skill4.selectionRequiresTargetLoS = false;
            skill4.activationRequiresTargetLoS = false;
            skill4.activationRequiresAimConfirmation = true;
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
            skill2.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill2.ignoreNodeGraph = false;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = false;

            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "UtilityOffensive";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 5;
            skill3.maxDistance = 100;
            skill3.minUserHealthFraction = .7f;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = false;
            skill3.activationRequiresAimConfirmation = false;
            skill3.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill3.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill3.ignoreNodeGraph = true;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = true;

            AISkillDriver skill3def = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3def.customName = "UtilityOffensive";
            skill3def.skillSlot = RoR2.SkillSlot.Utility;
            skill3def.requireSkillReady = true;
            skill3def.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3def.minDistance = 5;
            skill3def.maxDistance = 40;
            skill3def.maxUserHealthFraction = .3f;
            skill3def.selectionRequiresTargetLoS = true;
            skill3def.activationRequiresTargetLoS = false;
            skill3def.activationRequiresAimConfirmation = false;
            skill3def.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            skill3def.aimType = AISkillDriver.AimType.MoveDirection;
            skill3def.ignoreNodeGraph = true;
            skill3def.resetCurrentEnemyOnNextDriverSelection = false;
            skill3def.noRepeat = false;
            skill3def.shouldSprint = true;

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Primary";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = false;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0;
            skill1.maxDistance = 50;
            skill1.selectionRequiresTargetLoS = true;
            skill1.activationRequiresTargetLoS = true;
            skill1.activationRequiresAimConfirmation = true;
            skill1.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill1.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill1.ignoreNodeGraph = false;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = false;

            skill4.nextHighPriorityOverride = skill1;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 20);
        }
    }
}
