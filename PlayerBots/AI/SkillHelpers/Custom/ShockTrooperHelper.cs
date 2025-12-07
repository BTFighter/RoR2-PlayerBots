using Modules.Characters;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers.Custom
{
    [SkillHelperSurvivor("GaleShockTrooperBody")]
    [CustomSurvivor("https://thunderstore.io/package/The_Constellate/Shock_Trooper/", "1.1.11")]
    class ShockTrooperHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            ai.aimVectorDampTime = 0.1f;
            ai.aimVectorMaxSpeed = 360f;

            // Use Primary Swing - Auto shotgun at close range (0-8 distance)
            AISkillDriver primary = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            primary.customName = "Use Primary Swing";
            primary.skillSlot = SkillSlot.Primary;
            primary.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            primary.requireSkillReady = false; //usually false for primaries
            primary.requireEquipmentReady = false;
            primary.minUserHealthFraction = float.NegativeInfinity;
            primary.maxUserHealthFraction = float.PositiveInfinity;
            primary.minTargetHealthFraction = float.NegativeInfinity;
            primary.maxTargetHealthFraction = float.PositiveInfinity;
            primary.minDistance = 0;
            primary.maxDistance = 60;
            primary.selectionRequiresTargetLoS = false;
            primary.selectionRequiresOnGround = false;
            primary.selectionRequiresAimTarget = false;
            primary.maxTimesSelected = -1;
            primary.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primary.activationRequiresTargetLoS = false;
            primary.activationRequiresAimTargetLoS = false;
            primary.activationRequiresAimConfirmation = false;
            primary.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primary.moveInputScale = 1;
            primary.aimType = AISkillDriver.AimType.AtMoveTarget;
            primary.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            primary.shouldSprint = false; 
            primary.shouldFireEquipment = false;
            primary.buttonPressType = AISkillDriver.ButtonPressType.Hold; 
            primary.driverUpdateTimerOverride = -1;
            primary.resetCurrentEnemyOnNextDriverSelection = false;
            primary.noRepeat = false;
            primary.nextHighPriorityOverride = null;

            // Use Secondary Shoot - Missile Painter/Sticky Grenades (0-25 distance)
            AISkillDriver secondary = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            secondary.customName = "Use Secondary Shoot";
            secondary.skillSlot = SkillSlot.Secondary;
            secondary.requireSkillReady = true;
            secondary.minDistance = 0;
            secondary.maxDistance = 60;
            secondary.selectionRequiresTargetLoS = false;
            secondary.selectionRequiresOnGround = false;
            secondary.selectionRequiresAimTarget = false;
            secondary.maxTimesSelected = -1;
            secondary.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondary.activationRequiresTargetLoS = false;
            secondary.activationRequiresAimTargetLoS = false;
            secondary.activationRequiresAimConfirmation = true;
            secondary.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            secondary.moveInputScale = 1;
            secondary.aimType = AISkillDriver.AimType.AtMoveTarget;
            secondary.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Use Utility Roll - Shock Dash (8-20 distance)
            AISkillDriver utility = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            utility.customName = "Use Utility Roll";
            utility.skillSlot = SkillSlot.Utility;
            utility.requireSkillReady = true;
            utility.minDistance = 8;
            utility.maxDistance = 20;
            utility.selectionRequiresTargetLoS = true;
            utility.selectionRequiresOnGround = false;
            utility.selectionRequiresAimTarget = false;
            utility.maxTimesSelected = -1;
            utility.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utility.activationRequiresTargetLoS = false;
            utility.activationRequiresAimTargetLoS = false;
            utility.activationRequiresAimConfirmation = false;
            utility.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            utility.moveInputScale = 1;
            utility.aimType = AISkillDriver.AimType.AtMoveTarget;
            utility.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Use Special bomb - Ricochet Slug/Deploy Drone (0-20 distance)
            AISkillDriver special = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            special.customName = "Use Special bomb";
            special.skillSlot = SkillSlot.Special;
            special.requireSkillReady = true;
            special.minDistance = 0;
            special.maxDistance = 20;
            special.selectionRequiresTargetLoS = false;
            special.selectionRequiresOnGround = false;
            special.selectionRequiresAimTarget = false;
            special.maxTimesSelected = -1;
            special.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            special.activationRequiresTargetLoS = false;
            special.activationRequiresAimTargetLoS = false;
            special.activationRequiresAimConfirmation = false;
            special.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            special.moveInputScale = 1;
            special.aimType = AISkillDriver.AimType.AtMoveTarget;
            special.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Chase - Default movement behavior (unlimited distance)
            AISkillDriver chase = gameObject.AddComponent<AISkillDriver>() as AISkillDriver;
            chase.customName = "Chase";
            chase.skillSlot = SkillSlot.None;
            chase.requireSkillReady = false;
            chase.minDistance = 0;
            chase.maxDistance = float.PositiveInfinity;
            chase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chase.activationRequiresTargetLoS = false;
            chase.activationRequiresAimTargetLoS = false;
            chase.activationRequiresAimConfirmation = false;
            chase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chase.moveInputScale = 1;
            chase.aimType = AISkillDriver.AimType.AtMoveTarget;
            chase.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            // Set up skill chain priorities for proper AI behavior
            // Primary (shotgun) has highest priority for close combat
            primary.nextHighPriorityOverride = secondary;
            secondary.nextHighPriorityOverride = utility;
            utility.nextHighPriorityOverride = special;
            special.nextHighPriorityOverride = chase;

            // Add default skills for fallback behavior
            AddDefaultSkills(gameObject, ai, 10);
        }
    }
}
