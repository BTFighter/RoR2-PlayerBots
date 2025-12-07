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
            if (ai != null)
            {
                ai.neverRetaliateFriendlies = true;
                ai.aimVectorDampTime = 0.05f;
                ai.aimVectorMaxSpeed = 3600f;
            }

            // Repossess Slam - Primary skill for close range combat (like SuffocateSlam)
            AISkillDriver repossessSlam = gameObject.AddComponent<AISkillDriver>();
            repossessSlam.customName = "RepossessSlam";
            repossessSlam.skillSlot = SkillSlot.Primary;
            repossessSlam.requireSkillReady = true;
            repossessSlam.requireEquipmentReady = false;
            repossessSlam.moveTargetType = AISkillDriver.TargetType.Custom;
            repossessSlam.minDistance = 0f;
            repossessSlam.maxDistance = float.PositiveInfinity;
            repossessSlam.selectionRequiresTargetLoS = false;
            repossessSlam.activationRequiresTargetLoS = false;
            repossessSlam.activationRequiresAimConfirmation = false;
            repossessSlam.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            repossessSlam.aimType = AISkillDriver.AimType.AtMoveTarget;
            repossessSlam.ignoreNodeGraph = false;
            repossessSlam.noRepeat = false;
            repossessSlam.shouldSprint = false;
            repossessSlam.shouldFireEquipment = false;
            repossessSlam.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            repossessSlam.driverUpdateTimerOverride = 1f;

            // Repossess Throw - Utility skill for throwing bagged entities (like EmptyBag)
            AISkillDriver repossessThrow = gameObject.AddComponent<AISkillDriver>();
            repossessThrow.customName = "RepossessThrow";
            repossessThrow.skillSlot = SkillSlot.Utility;
            repossessThrow.requireSkillReady = true;
            repossessThrow.requireEquipmentReady = false;
            repossessThrow.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            repossessThrow.minDistance = 10f;
            repossessThrow.maxDistance = 40f;
            repossessThrow.selectionRequiresTargetLoS = true;
            repossessThrow.activationRequiresTargetLoS = true;
            repossessThrow.activationRequiresAimConfirmation = true;
            repossessThrow.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            repossessThrow.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            repossessThrow.ignoreNodeGraph = false;
            repossessThrow.noRepeat = true;
            repossessThrow.shouldSprint = false;
            repossessThrow.shouldFireEquipment = false;
            repossessThrow.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            repossessThrow.driverUpdateTimerOverride = 0.1f;

            // Tornado Slam End - Primary skill for tornado finish (like BluntForceTornado)
            AISkillDriver tornadoSlamEnd = gameObject.AddComponent<AISkillDriver>();
            tornadoSlamEnd.customName = "TornadoSlamEnd";
            tornadoSlamEnd.skillSlot = SkillSlot.Primary;
            tornadoSlamEnd.requireSkillReady = true;
            tornadoSlamEnd.requireEquipmentReady = false;
            tornadoSlamEnd.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            tornadoSlamEnd.minDistance = 0f;
            tornadoSlamEnd.maxDistance = 5f;
            tornadoSlamEnd.selectionRequiresTargetLoS = false;
            tornadoSlamEnd.activationRequiresTargetLoS = false;
            tornadoSlamEnd.activationRequiresAimConfirmation = false;
            tornadoSlamEnd.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            tornadoSlamEnd.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            tornadoSlamEnd.ignoreNodeGraph = true;
            tornadoSlamEnd.noRepeat = false;
            tornadoSlamEnd.shouldSprint = false;
            tornadoSlamEnd.shouldFireEquipment = false;
            tornadoSlamEnd.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Salvage - Special skill for healing/support
            AISkillDriver salvage = gameObject.AddComponent<AISkillDriver>();
            salvage.customName = "Salvage";
            salvage.skillSlot = SkillSlot.Special;
            salvage.requireSkillReady = true;
            salvage.requireEquipmentReady = false;
            salvage.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            salvage.minDistance = 0f;
            salvage.maxDistance = float.PositiveInfinity;
            salvage.selectionRequiresTargetLoS = false;
            salvage.activationRequiresTargetLoS = false;
            salvage.activationRequiresAimConfirmation = false;
            salvage.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            salvage.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            salvage.ignoreNodeGraph = false;
            salvage.noRepeat = false;
            salvage.shouldSprint = false;
            salvage.shouldFireEquipment = false;
            salvage.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            salvage.driverUpdateTimerOverride = 1.5f;

            // Repossess - Main utility skill for capturing enemies
            AISkillDriver repossess = gameObject.AddComponent<AISkillDriver>();
            repossess.customName = "Repossess";
            repossess.skillSlot = SkillSlot.Utility;
            repossess.requireSkillReady = true;
            repossess.requireEquipmentReady = false;
            repossess.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            repossess.minDistance = 0f;
            repossess.maxDistance = 20f;
            repossess.selectionRequiresTargetLoS = true;
            repossess.activationRequiresTargetLoS = true;
            repossess.activationRequiresAimConfirmation = true;
            repossess.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            repossess.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            repossess.ignoreNodeGraph = false;
            repossess.noRepeat = false;
            repossess.shouldSprint = false;
            repossess.shouldFireEquipment = false;
            repossess.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            repossess.driverUpdateTimerOverride = 0.5f;

            // Tornado Slam - Utility skill for AoE (like TornadoSlam)
            AISkillDriver tornadoSlam = gameObject.AddComponent<AISkillDriver>();
            tornadoSlam.customName = "TornadoSlam";
            tornadoSlam.skillSlot = SkillSlot.Utility;
            tornadoSlam.requireSkillReady = true;
            tornadoSlam.requireEquipmentReady = false;
            tornadoSlam.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            tornadoSlam.minDistance = 20f;
            tornadoSlam.maxDistance = 100f;
            tornadoSlam.selectionRequiresTargetLoS = true;
            tornadoSlam.activationRequiresTargetLoS = true;
            tornadoSlam.activationRequiresAimConfirmation = true;
            tornadoSlam.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            tornadoSlam.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            tornadoSlam.ignoreNodeGraph = true;
            tornadoSlam.noRepeat = true;
            tornadoSlam.shouldSprint = false;
            tornadoSlam.shouldFireEquipment = false;
            tornadoSlam.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            tornadoSlam.driverUpdateTimerOverride = 1f;

            // Cleanup - Secondary skill for area damage (like Cleanup)
            AISkillDriver cleanup = gameObject.AddComponent<AISkillDriver>();
            cleanup.customName = "Cleanup";
            cleanup.skillSlot = SkillSlot.Secondary;
            cleanup.requireSkillReady = true;
            cleanup.requireEquipmentReady = false;
            cleanup.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            cleanup.minDistance = 5f;
            cleanup.maxDistance = 50f;
            cleanup.selectionRequiresTargetLoS = true;
            cleanup.activationRequiresTargetLoS = true;
            cleanup.activationRequiresAimConfirmation = true;
            cleanup.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            cleanup.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            cleanup.ignoreNodeGraph = true;
            cleanup.noRepeat = true;
            cleanup.shouldSprint = false;
            cleanup.shouldFireEquipment = false;
            cleanup.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            cleanup.driverUpdateTimerOverride = 0.1f;

            // Spawn Cube - Secondary skill for summoning (like JunkCube)
            AISkillDriver spawnCube = gameObject.AddComponent<AISkillDriver>();
            spawnCube.customName = "SpawnCube";
            spawnCube.skillSlot = SkillSlot.Secondary;
            spawnCube.requireSkillReady = true;
            spawnCube.requireEquipmentReady = false;
            spawnCube.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            spawnCube.minDistance = 5f;
            spawnCube.maxDistance = 60f;
            spawnCube.selectionRequiresTargetLoS = true;
            spawnCube.activationRequiresTargetLoS = true;
            spawnCube.activationRequiresAimConfirmation = false;
            spawnCube.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            spawnCube.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            spawnCube.ignoreNodeGraph = true;
            spawnCube.noRepeat = true;
            spawnCube.shouldSprint = false;
            spawnCube.shouldFireEquipment = false;
            spawnCube.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            spawnCube.driverUpdateTimerOverride = 0.5f;

            // Primary Strafe - Basic primary attack when in close range
            AISkillDriver primaryStrafe = gameObject.AddComponent<AISkillDriver>();
            primaryStrafe.customName = "PrimaryStrafe";
            primaryStrafe.skillSlot = SkillSlot.Primary;
            primaryStrafe.requireSkillReady = true;
            primaryStrafe.requireEquipmentReady = false;
            primaryStrafe.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryStrafe.minDistance = 0f;
            primaryStrafe.maxDistance = 5f;
            primaryStrafe.selectionRequiresTargetLoS = false;
            primaryStrafe.activationRequiresTargetLoS = false;
            primaryStrafe.activationRequiresAimConfirmation = false;
            primaryStrafe.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            primaryStrafe.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryStrafe.ignoreNodeGraph = true;
            primaryStrafe.noRepeat = false;
            primaryStrafe.shouldSprint = false;
            primaryStrafe.shouldFireEquipment = false;
            primaryStrafe.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryStrafe.driverUpdateTimerOverride = 0.5f;

            // Primary Chase - Basic primary attack when chasing enemies
            AISkillDriver primaryChase = gameObject.AddComponent<AISkillDriver>();
            primaryChase.customName = "PrimaryChase";
            primaryChase.skillSlot = SkillSlot.Primary;
            primaryChase.requireSkillReady = true;
            primaryChase.requireEquipmentReady = false;
            primaryChase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryChase.minDistance = 0f;
            primaryChase.maxDistance = 10f;
            primaryChase.selectionRequiresTargetLoS = false;
            primaryChase.activationRequiresTargetLoS = false;
            primaryChase.activationRequiresAimConfirmation = false;
            primaryChase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primaryChase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryChase.ignoreNodeGraph = true;
            primaryChase.noRepeat = false;
            primaryChase.shouldSprint = false;
            primaryChase.shouldFireEquipment = false;
            primaryChase.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryChase.driverUpdateTimerOverride = 0.5f;

            // Salvage Owner - Special skill when following leader
            AISkillDriver salvageOwner = gameObject.AddComponent<AISkillDriver>();
            salvageOwner.customName = "SalvageOwner";
            salvageOwner.skillSlot = SkillSlot.Special;
            salvageOwner.requireSkillReady = true;
            salvageOwner.requireEquipmentReady = false;
            salvageOwner.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            salvageOwner.minDistance = 0f;
            salvageOwner.maxDistance = float.PositiveInfinity;
            salvageOwner.selectionRequiresTargetLoS = false;
            salvageOwner.activationRequiresTargetLoS = false;
            salvageOwner.activationRequiresAimConfirmation = false;
            salvageOwner.aimType = AISkillDriver.AimType.AtCurrentLeader;
            salvageOwner.ignoreNodeGraph = false;
            salvageOwner.noRepeat = false;
            salvageOwner.shouldSprint = false;
            salvageOwner.shouldFireEquipment = false;
            salvageOwner.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Strafing - Movement behavior for mid-range combat
            AISkillDriver strafing = gameObject.AddComponent<AISkillDriver>();
            strafing.customName = "Strafing";
            strafing.skillSlot = SkillSlot.None;
            strafing.requireSkillReady = false;
            strafing.requireEquipmentReady = false;
            strafing.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafing.minDistance = 20f;
            strafing.maxDistance = 60f;
            strafing.selectionRequiresTargetLoS = true;
            strafing.activationRequiresTargetLoS = true;
            strafing.activationRequiresAimConfirmation = false;
            strafing.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            strafing.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            strafing.ignoreNodeGraph = false;
            strafing.noRepeat = true;
            strafing.shouldSprint = true;
            strafing.shouldFireEquipment = false;
            strafing.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            strafing.driverUpdateTimerOverride = 0.5f;

            // Sprint Chase - Aggressive pursuit behavior
            AISkillDriver sprintChase = gameObject.AddComponent<AISkillDriver>();
            sprintChase.customName = "SprintChase";
            sprintChase.skillSlot = SkillSlot.None;
            sprintChase.requireSkillReady = false;
            sprintChase.requireEquipmentReady = false;
            sprintChase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            sprintChase.minDistance = 0f;
            sprintChase.maxDistance = 400f;
            sprintChase.selectionRequiresTargetLoS = false;
            sprintChase.activationRequiresTargetLoS = false;
            sprintChase.activationRequiresAimConfirmation = false;
            sprintChase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            sprintChase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            sprintChase.ignoreNodeGraph = false;
            sprintChase.noRepeat = false;
            sprintChase.shouldSprint = true;
            sprintChase.shouldFireEquipment = false;
            sprintChase.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            // Follow Owner - Following behavior
            AISkillDriver followOwner = gameObject.AddComponent<AISkillDriver>();
            followOwner.customName = "FollowOwner";
            followOwner.skillSlot = SkillSlot.None;
            followOwner.requireSkillReady = false;
            followOwner.requireEquipmentReady = false;
            followOwner.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            followOwner.minDistance = 15f;
            followOwner.maxDistance = float.PositiveInfinity;
            followOwner.selectionRequiresTargetLoS = false;
            followOwner.activationRequiresTargetLoS = false;
            followOwner.activationRequiresAimConfirmation = false;
            followOwner.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            followOwner.aimType = AISkillDriver.AimType.AtCurrentLeader;
            followOwner.ignoreNodeGraph = false;
            followOwner.noRepeat = false;
            followOwner.shouldSprint = true;
            followOwner.shouldFireEquipment = false;
            followOwner.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            // Idle Near Owner - Staying close to leader
            AISkillDriver idleNearOwner = gameObject.AddComponent<AISkillDriver>();
            idleNearOwner.customName = "IdleNearOwner";
            idleNearOwner.skillSlot = SkillSlot.None;
            idleNearOwner.requireSkillReady = false;
            idleNearOwner.requireEquipmentReady = false;
            idleNearOwner.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            idleNearOwner.minDistance = 0f;
            idleNearOwner.maxDistance = 15f;
            idleNearOwner.selectionRequiresTargetLoS = false;
            idleNearOwner.activationRequiresTargetLoS = false;
            idleNearOwner.activationRequiresAimConfirmation = false;
            idleNearOwner.movementType = AISkillDriver.MovementType.Stop;
            idleNearOwner.aimType = AISkillDriver.AimType.AtCurrentLeader;
            idleNearOwner.ignoreNodeGraph = false;
            idleNearOwner.noRepeat = false;
            idleNearOwner.shouldSprint = false;
            idleNearOwner.shouldFireEquipment = false;
            idleNearOwner.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            // Sprint Chase Low Priority - Fallback aggressive behavior
            AISkillDriver sprintChaseLowPriority = gameObject.AddComponent<AISkillDriver>();
            sprintChaseLowPriority.customName = "SprintChaseLowPriority";
            sprintChaseLowPriority.skillSlot = SkillSlot.None;
            sprintChaseLowPriority.requireSkillReady = false;
            sprintChaseLowPriority.requireEquipmentReady = false;
            sprintChaseLowPriority.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            sprintChaseLowPriority.minDistance = 0f;
            sprintChaseLowPriority.maxDistance = float.PositiveInfinity;
            sprintChaseLowPriority.selectionRequiresTargetLoS = false;
            sprintChaseLowPriority.activationRequiresTargetLoS = false;
            sprintChaseLowPriority.activationRequiresAimConfirmation = false;
            sprintChaseLowPriority.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            sprintChaseLowPriority.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            sprintChaseLowPriority.ignoreNodeGraph = false;
            sprintChaseLowPriority.noRepeat = false;
            sprintChaseLowPriority.shouldSprint = true;
            sprintChaseLowPriority.shouldFireEquipment = false;
            sprintChaseLowPriority.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            // Hit Cube - Primary skill when cubes are available
            AISkillDriver hitCube = gameObject.AddComponent<AISkillDriver>();
            hitCube.customName = "HitCube";
            hitCube.skillSlot = SkillSlot.Primary;
            hitCube.requireSkillReady = true;
            hitCube.requireEquipmentReady = false;
            hitCube.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            hitCube.minDistance = 0f;
            hitCube.maxDistance = 60f;
            hitCube.selectionRequiresTargetLoS = true;
            hitCube.activationRequiresTargetLoS = true;
            hitCube.activationRequiresAimConfirmation = false;
            hitCube.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            hitCube.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            hitCube.ignoreNodeGraph = false;
            hitCube.noRepeat = false;
            hitCube.shouldSprint = false;
            hitCube.shouldFireEquipment = false;
            hitCube.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            hitCube.driverUpdateTimerOverride = 1f;

            // Release Input - Input release behavior
            AISkillDriver releaseInput = gameObject.AddComponent<AISkillDriver>();
            releaseInput.customName = "ReleaseInput";
            releaseInput.skillSlot = SkillSlot.None;
            releaseInput.requireSkillReady = false;
            releaseInput.requireEquipmentReady = false;
            releaseInput.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            releaseInput.minDistance = 0f;
            releaseInput.maxDistance = float.PositiveInfinity;
            releaseInput.selectionRequiresTargetLoS = false;
            releaseInput.activationRequiresTargetLoS = false;
            releaseInput.activationRequiresAimConfirmation = false;
            releaseInput.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            releaseInput.ignoreNodeGraph = false;
            releaseInput.noRepeat = false;
            releaseInput.shouldSprint = false;
            releaseInput.shouldFireEquipment = false;
            releaseInput.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            releaseInput.driverUpdateTimerOverride = 0.4f;

            // Set up skill driver priorities using nextHighPriorityOverride (ImprovedSurvivorAI approach)
            cleanup.nextHighPriorityOverride = releaseInput;
            repossessThrow.nextHighPriorityOverride = releaseInput;
            repossess.nextHighPriorityOverride = repossessSlam;
            spawnCube.nextHighPriorityOverride = hitCube;

            // Add default skills as fallback
            AddDefaultSkills(gameObject, ai, 0);
        }

        public override void OnBodyChange()
        {
            base.OnBodyChange();
            // No additional setup needed for ImprovedSurvivorAI approach
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            // ImprovedSurvivorAI approach doesn't require complex runtime updates
        }
    }
}
