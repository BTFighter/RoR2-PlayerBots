using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("BasicTankBody")]
    [CustomSurvivor("https://thunderstore.io/package/CheeseWithHoles/Celestial_War_Tank/", "2.0.2")]
    class BasicTankHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            AISkillDriver skill4 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill4.customName = "Drop Landmine";
            skill4.skillSlot = RoR2.SkillSlot.Special;
            skill4.requireSkillReady = true;
            skill4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill4.minDistance = 0f;
            skill4.maxDistance = 14f;
            skill4.selectionRequiresTargetLoS = true;
            skill4.activationRequiresTargetLoS = false;
            skill4.activationRequiresAimConfirmation = true;
            skill4.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill4.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill4.ignoreNodeGraph = false;
            skill4.moveInputScale = 1f;
            skill4.driverUpdateTimerOverride = 0.5f;
            skill4.resetCurrentEnemyOnNextDriverSelection = false;
            skill4.noRepeat = false;
            skill4.shouldSprint = false;
            skill4.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            AISkillDriver skill3 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill3.customName = "Plow";
            skill3.skillSlot = RoR2.SkillSlot.Utility;
            skill3.requireSkillReady = true;
            skill3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill3.minDistance = 0;
            skill3.maxDistance = 20f;
            skill3.selectionRequiresTargetLoS = true;
            skill3.activationRequiresTargetLoS = false;
            skill3.activationRequiresAimConfirmation = true;
            skill3.selectionRequiresOnGround = true;
            skill3.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill3.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill3.ignoreNodeGraph = false;
            skill3.moveInputScale = 1f;
            skill3.driverUpdateTimerOverride = 0.5f;
            skill3.resetCurrentEnemyOnNextDriverSelection = false;
            skill3.noRepeat = false;
            skill3.shouldSprint = false;
            skill3.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            AISkillDriver skill2 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill2.customName = "Crit Cannon";
            skill2.skillSlot = RoR2.SkillSlot.Secondary;
            skill2.requireSkillReady = true;
            skill2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill2.minDistance = 0f;
            skill2.maxDistance = 100f;
            skill2.selectionRequiresTargetLoS = true;
            skill2.activationRequiresTargetLoS = false;
            skill2.activationRequiresAimConfirmation = true;
            skill2.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            skill2.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            skill2.ignoreNodeGraph = false;
            skill2.moveInputScale = 1f;
            skill2.driverUpdateTimerOverride = -1f;
            skill2.resetCurrentEnemyOnNextDriverSelection = false;
            skill2.noRepeat = false;
            skill2.shouldSprint = false;
            skill2.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Skills
            AISkillDriver skill1 = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            skill1.customName = "Machine Gun";
            skill1.skillSlot = RoR2.SkillSlot.Primary;
            skill1.requireSkillReady = true;
            skill1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill1.minDistance = 0f;
            skill1.maxDistance = 100f;
            skill1.selectionRequiresTargetLoS = true;
            skill1.activationRequiresTargetLoS = false;
            skill1.activationRequiresAimConfirmation = true;
            skill1.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill1.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill1.ignoreNodeGraph = false;
            skill1.moveInputScale = 1f;
            skill1.driverUpdateTimerOverride = -1f;
            skill1.resetCurrentEnemyOnNextDriverSelection = false;
            skill1.noRepeat = false;
            skill1.shouldSprint = false;

            skill2.nextHighPriorityOverride = skill1;

            // Add default skills
            AddDefaultSkills(gameObject, ai, 15);
        }
    }
}
