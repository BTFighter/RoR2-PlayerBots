using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("DrifterBody")]
    class DrifterHelper : AiSkillHelper
    {
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

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Primary";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = true;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0;
            skill1.maxDistance = 10;
            skill1.selectionRequiresTargetLoS = true;
            skill1.activationRequiresTargetLoS = true;
            skill1.activationRequiresAimConfirmation = false;
            skill1.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill1.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill1.ignoreNodeGraph = true;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = false;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 0);
        }
    }
}
