using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("KindredBody")]
    [CustomSurvivor("https://thunderstore.io/c/riskofrain2/p/DragonycksModdingComms/Kindred/", "1.0.2")]
    class KindredHelper : AiSkillHelper
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
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = true;
            skill3.activationRequiresAimConfirmation = false;
            skill3.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill3.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill3.ignoreNodeGraph = false;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = true;
            skill3.driverUpdateTimerOverride = 2f;

            AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Secondary";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0;
            skill2.maxDistance = 30;
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
            skill4.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            skill4.minDistance = 0;
            skill4.maxDistance = 20;
            skill4.maxTargetHealthFraction = 0.7f;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = false;
            skill4.activationRequiresAimConfirmation = false;
            skill4.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill4.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = true;
            skill4.shouldSprint = false;

            AISkillDriver chaseSkill = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            chaseSkill.customName = "ChaseTarget";
            chaseSkill.skillSlot = RoR2.SkillSlot.None;
            chaseSkill.requireSkillReady = false;
            chaseSkill.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseSkill.minDistance = 30;
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

            AISkillDriver skill1_shoot = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1_shoot.customName = "Primary";
            skill1_shoot.skillSlot = RoR2.SkillSlot.Primary;
            skill1_shoot.requireSkillReady = true;
            skill1_shoot.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1_shoot.minDistance = 0;
            skill1_shoot.maxDistance = 50;
            skill1_shoot.selectionRequiresTargetLoS = true;
            skill1_shoot.activationRequiresTargetLoS = true;
            skill1_shoot.activationRequiresAimConfirmation = false;
            skill1_shoot.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill1_shoot.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill1_shoot.ignoreNodeGraph = true;
            skill1_shoot.resetCurrentEnemyOnNextDriverSelection = false;
            skill1_shoot.noRepeat = false;
            skill1_shoot.shouldSprint = false;
            skill1_shoot.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            

            // Add default skills
            AddDefaultSkills(gameObject, ai, 20);
        }
    }
}
