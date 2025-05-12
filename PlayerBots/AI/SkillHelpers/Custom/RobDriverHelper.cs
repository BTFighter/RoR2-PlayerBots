using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using System;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("RobDriverBody")]
    [CustomSurvivor("https://thunderstore.io/c/riskofrain2/p/public_ParticleSystem/Driver/", "2.1.6")]
    class RobDriverHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "Utility";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 0;
            skill3.maxDistance = 20;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = true;
            skill3.activationRequiresAimConfirmation = false;
            skill3.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            skill3.aimType = AISkillDriver.AimType.MoveDirection;
            skill3.ignoreNodeGraph = false;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = true;
            skill3.shouldSprint = false;

            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Special";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0;
            skill4.maxDistance = 30;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = true;
            skill4.activationRequiresAimConfirmation = false;
            skill4.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = true;
            skill4.shouldSprint = false;
            //skill4.driverUpdateTimerOverride = 0.5f;
			
			AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Secondary";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0;
            skill2.maxDistance = 60;
            skill2.selectionRequiresTargetLoS = true;
            skill2.activationRequiresTargetLoS = true;
            skill2.activationRequiresAimConfirmation = false;
            skill2.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill2.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill2.ignoreNodeGraph = false;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = false;
            skill2.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Primary";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = true;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0;
            skill1.maxDistance = 60;
            skill1.selectionRequiresTargetLoS = true;
            skill1.activationRequiresTargetLoS = true;
            skill1.activationRequiresAimConfirmation = true;
            skill1.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill1.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill1.ignoreNodeGraph = false;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = false;

            AISkillDriver skillchase = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skillchase.customName = "Primary";
            skillchase.skillSlot = RoR2.SkillSlot.None;
            skillchase.requireSkillReady = false;
            skillchase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skillchase.minDistance = 0;
            skillchase.maxDistance = 60;
            skillchase.selectionRequiresTargetLoS = false;
            skillchase.activationRequiresTargetLoS = false;
            skillchase.activationRequiresAimConfirmation = false;
            skillchase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skillchase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skillchase.ignoreNodeGraph = false;
            skillchase.resetCurrentEnemyOnNextDriverSelection = false;
            skillchase.noRepeat = false;
            skillchase.shouldSprint = true;
            skillchase.driverUpdateTimerOverride = 2f;
            skillchase.maxTargetHealthFraction = .1f;

            skill2.nextHighPriorityOverride = skill1;
            skill1.nextHighPriorityOverride = skillchase;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 15);
        }
    }
}
