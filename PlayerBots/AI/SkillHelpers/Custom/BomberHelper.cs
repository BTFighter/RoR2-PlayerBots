using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("DragonBomberBody")]
    [CustomSurvivor("https://thunderstore.io/package/Dragonyck/Bomber/", "1.4.1")]
    class BomberHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            // Skills
            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "FlashBomb";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0;
            skill4.maxDistance = 50;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = false;
            skill4.activationRequiresAimConfirmation = false;
            skill4.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill4.ignoreNodeGraph = false;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = true;
            skill4.shouldSprint = false;
            skill4.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            AISkillDriver skill4_start = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4_start.customName = "FlashBombStart";
            skill4_start.skillSlot = RoR2.SkillSlot.Primary;
            skill4_start.requireSkillReady = true;
            skill4_start.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4_start.minDistance = 0;
            skill4_start.maxDistance = 50;
            skill4_start.selectionRequiresTargetLoS = true;
            skill4_start.activationRequiresTargetLoS = false;
            skill4_start.activationRequiresAimConfirmation = false;
            skill4_start.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill4_start.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill4_start.ignoreNodeGraph = false;
            skill4_start.resetCurrentEnemyOnNextDriverSelection = false;
            skill4_start.noRepeat = true;
            skill4_start.shouldSprint = false;

            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "PulseBomb";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 0;
            skill3.maxDistance = 50;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = true;
            skill3.activationRequiresAimConfirmation = false;
            skill3.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill3.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill3.ignoreNodeGraph = false;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = true;

            AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Detonate";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0;
            skill2.maxDistance = 50;
            skill2.selectionRequiresTargetLoS = true;
            skill2.activationRequiresTargetLoS = true;
            skill2.activationRequiresAimConfirmation = false;
            skill2.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill2.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill2.ignoreNodeGraph = false;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = false;

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
            skill1.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill1.ignoreNodeGraph = false;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = false;
            skill1.driverUpdateTimerOverride = 2f;

            skill4.nextHighPriorityOverride = skill4_start;
            skill4_start.nextHighPriorityOverride = skill1;
            skill1.nextHighPriorityOverride = skill2;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 15);
        }
    }
}
