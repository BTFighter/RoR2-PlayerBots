using PlayerBots.Custom;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI
{
    abstract class AiSkillHelper
    {
        public PlayerBotController controller { get; set; }

        // Default distances and thresholds
        protected float defaultMinDistance = 15f;
        protected float defaultMaxDistance = 40f;
        protected float bossMinDistance = 10f;
        protected float bossMaxDistance = 30f;
        protected float groupMinDistance = 20f;
        protected float groupMaxDistance = 40f;
        protected float lowHealthThreshold = 0.3f;
        protected float targetLowHealthThreshold = 0.5f;
        protected float targetVeryLowHealthThreshold = 0.1f;

        // Pathfinding settings
        protected float pathUpdateInterval = 0.25f;
        protected float strafeDistance = 8f;
        protected float jumpHeight = 3f;
        protected float ledgeCheckDistance = 2f;
        protected float obstacleAvoidanceRadius = 2f;
        protected float groupFormationRadius = 5f;
        protected float groupFormationAngle = 45f;

        // Events
        public virtual void OnBodyChange()
        {
            if (controller?.body)
            {
                OptimizeMovement(controller.body.gameObject);
            }
        }
        public virtual void OnFixedUpdate()
        {
            if (controller?.body)
            {
                OptimizeMovement(controller.body.gameObject);
            }
        }
        protected virtual void OptimizeMovement(GameObject gameObject)
        {
            var baseAI = gameObject.GetComponent<BaseAI>();
            if (!baseAI) return;

            // Set up optimized movement parameters
            baseAI.aimVectorDampTime = 0.0005f;
            baseAI.aimVectorMaxSpeed = 18000f;
            baseAI.enemyAttentionDuration = 3f;
            baseAI.fullVision = true;
        }

        // Skills
        public abstract void InjectSkills(GameObject gameObject, BaseAI ai);

        protected AISkillDriver CreateImprovedSkillDriver(
            GameObject gameObject,
            string customName,
            SkillSlot skillSlot,
            float minDistance = 0f,
            float maxDistance = 50f,
            bool requireSkillReady = true,
            bool selectionRequiresTargetLoS = true,
            bool activationRequiresTargetLoS = true,
            bool activationRequiresAimConfirmation = true,
            AISkillDriver.MovementType movementType = AISkillDriver.MovementType.StrafeMovetarget,
            AISkillDriver.AimType aimType = AISkillDriver.AimType.AtMoveTarget,
            bool ignoreNodeGraph = false,
            bool resetCurrentEnemyOnNextDriverSelection = false,
            bool noRepeat = false,
            bool shouldSprint = false,
            AISkillDriver.ButtonPressType buttonPressType = AISkillDriver.ButtonPressType.TapContinuous,
            float maxUserHealthFraction = 1f,
            float minUserHealthFraction = 0f,
            float maxTargetHealthFraction = 1f,
            float minTargetHealthFraction = 0f)
        {
            AISkillDriver skill = gameObject.AddComponent<AISkillDriver>();
            skill.customName = customName;
            skill.skillSlot = skillSlot;
            skill.requireSkillReady = requireSkillReady;
            skill.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill.minDistance = minDistance;
            skill.maxDistance = maxDistance;
            skill.maxUserHealthFraction = maxUserHealthFraction;
            skill.minUserHealthFraction = minUserHealthFraction;
            skill.maxTargetHealthFraction = maxTargetHealthFraction;
            skill.minTargetHealthFraction = minTargetHealthFraction;
            skill.selectionRequiresTargetLoS = selectionRequiresTargetLoS;
            skill.activationRequiresTargetLoS = activationRequiresTargetLoS;
            skill.activationRequiresAimConfirmation = activationRequiresAimConfirmation;
            skill.movementType = movementType;
            skill.aimType = aimType;
            skill.ignoreNodeGraph = ignoreNodeGraph;
            skill.resetCurrentEnemyOnNextDriverSelection = resetCurrentEnemyOnNextDriverSelection;
            skill.noRepeat = noRepeat;
            skill.shouldSprint = shouldSprint;
            skill.buttonPressType = buttonPressType;
            skill.driverUpdateTimerOverride = pathUpdateInterval;
            return skill;
        }

        protected AISkillDriver CreateDefensiveSkillDriver(
            GameObject gameObject,
            string customName,
            SkillSlot skillSlot,
            float minDistance = 5f,
            float maxDistance = 20f)
        {
            return CreateImprovedSkillDriver(
                gameObject,
                customName,
                skillSlot,
                minDistance,
                maxDistance,
                true,
                true,
                false,
                false,
                AISkillDriver.MovementType.FleeMoveTarget,
                AISkillDriver.AimType.MoveDirection,
                false,
                false,
                false,
                true,
                AISkillDriver.ButtonPressType.TapContinuous,
                lowHealthThreshold,
                0f,
                1f,
                0f
            );
        }

        protected AISkillDriver CreateOffensiveSkillDriver(
            GameObject gameObject,
            string customName,
            SkillSlot skillSlot,
            float minDistance = 5f,
            float maxDistance = 50f)
        {
            return CreateImprovedSkillDriver(
                gameObject,
                customName,
                skillSlot,
                minDistance,
                maxDistance,
                true,
                true,
                true,
                true,
                AISkillDriver.MovementType.StrafeMovetarget,
                AISkillDriver.AimType.AtMoveTarget,
                false,
                false,
                false,
                false,
                AISkillDriver.ButtonPressType.TapContinuous,
                1f,
                0f,
                targetLowHealthThreshold,
                targetVeryLowHealthThreshold
            );
        }

        protected AISkillDriver CreateBossSkillDriver(
            GameObject gameObject,
            string customName,
            SkillSlot skillSlot,
            float minDistance = 10f,
            float maxDistance = 30f)
        {
            return CreateImprovedSkillDriver(
                gameObject,
                customName,
                skillSlot,
                minDistance,
                maxDistance,
                true,
                true,
                true,
                true,
                AISkillDriver.MovementType.StrafeMovetarget,
                AISkillDriver.AimType.AtMoveTarget,
                false,
                false,
                false,
                false,
                AISkillDriver.ButtonPressType.TapContinuous
            );
        }

        protected AISkillDriver CreateGroupSkillDriver(
            GameObject gameObject,
            string customName,
            SkillSlot skillSlot = SkillSlot.None,
            float minDistance = 20f,
            float maxDistance = 40f)
        {
            AISkillDriver skill = gameObject.AddComponent<AISkillDriver>();
            skill.customName = customName;
            skill.skillSlot = skillSlot;
            skill.requireSkillReady = false;
            skill.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            skill.minDistance = minDistance;
            skill.maxDistance = maxDistance;
            skill.selectionRequiresTargetLoS = false;
            skill.activationRequiresTargetLoS = false;
            skill.activationRequiresAimConfirmation = false;
            skill.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill.ignoreNodeGraph = false;
            skill.resetCurrentEnemyOnNextDriverSelection = false;
            skill.noRepeat = false;
            skill.shouldSprint = true;
            skill.driverUpdateTimerOverride = pathUpdateInterval;
            return skill;
        }

        protected AISkillDriver CreateChaseSkillDriver(
            GameObject gameObject,
            string customName,
            float minDistance = 10f,
            float maxDistance = 60f)
        {
            AISkillDriver skill = gameObject.AddComponent<AISkillDriver>();
            skill.customName = customName;
            skill.skillSlot = SkillSlot.None;
            skill.requireSkillReady = false;
            skill.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            skill.minDistance = minDistance;
            skill.maxDistance = maxDistance;
            skill.selectionRequiresTargetLoS = true;
            skill.activationRequiresTargetLoS = true;
            skill.activationRequiresAimConfirmation = false;
            skill.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill.ignoreNodeGraph = false;
            skill.resetCurrentEnemyOnNextDriverSelection = false;
            skill.noRepeat = false;
            skill.shouldSprint = true;
            skill.driverUpdateTimerOverride = pathUpdateInterval;
            return skill;
        }

        protected AISkillDriver CreateLeashSkillDriver(
            GameObject gameObject,
            string customName,
            float minDistance = 60f)
        {
            AISkillDriver skill = gameObject.AddComponent<AISkillDriver>();
            skill.customName = customName;
            skill.skillSlot = SkillSlot.None;
            skill.requireSkillReady = false;
            skill.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            skill.minDistance = minDistance;
            skill.maxDistance = float.PositiveInfinity;
            skill.selectionRequiresTargetLoS = false;
            skill.activationRequiresTargetLoS = false;
            skill.activationRequiresAimConfirmation = false;
            skill.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            skill.aimType = AISkillDriver.AimType.AtMoveTarget;
            skill.ignoreNodeGraph = false;
            skill.resetCurrentEnemyOnNextDriverSelection = true;
            skill.driverUpdateTimerOverride = pathUpdateInterval;
            skill.noRepeat = false;
            skill.shouldSprint = true;
            return skill;
        }

        public void AddDefaultSkills(GameObject gameObject, BaseAI ai, float minDistanceFromEnemy)
        {
            // Custom target leash
            CreateLeashSkillDriver(gameObject, "CustomTargetLeash", 5f);

            // Return to owner leash
            CreateLeashSkillDriver(gameObject, "ReturnToOwnerLeash", 60f);

            // Chase enemy
            CreateChaseSkillDriver(gameObject, "ChaseEnemy", minDistanceFromEnemy, float.PositiveInfinity);

            // Return to leader
            CreateGroupSkillDriver(gameObject, "ReturnToLeader", SkillSlot.None, 15f, float.PositiveInfinity);

            // Wait near leader
            CreateGroupSkillDriver(gameObject, "WaitNearLeader", SkillSlot.None, 5f, float.PositiveInfinity);
        }
    }
}

