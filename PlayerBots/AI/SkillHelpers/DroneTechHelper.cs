﻿using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.AI.SkillHelpers
{
    [SkillHelperSurvivor("DroneTechBody")]
    class DroneTechHelper : AiSkillHelper
    {
        public override void InjectSkills(GameObject gameObject, BaseAI ai)
        {
            if (ai != null)
            {
                ai.neverRetaliateFriendlies = true;
                ai.aimVectorDampTime = 0.05f;
                ai.aimVectorMaxSpeed = 720f;
            }

            // Special - Main special ability
            AISkillDriver special = gameObject.AddComponent<AISkillDriver>();
            special.customName = "Special";
            special.skillSlot = SkillSlot.Special;
            special.requireSkillReady = true;
            special.requireEquipmentReady = false;
            special.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            special.minDistance = 0f;
            special.maxDistance = 100f;
            special.selectionRequiresTargetLoS = true;
            special.activationRequiresTargetLoS = true;
            special.activationRequiresAimConfirmation = true;
            special.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            special.aimType = AISkillDriver.AimType.MoveDirection;
            special.ignoreNodeGraph = false;
            special.noRepeat = true;
            special.shouldSprint = true;
            special.shouldFireEquipment = false;
            special.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Ascent Protocol - Utility skill for mobility
            AISkillDriver ascentProtocol = gameObject.AddComponent<AISkillDriver>();
            ascentProtocol.customName = "AscentProtocol";
            ascentProtocol.skillSlot = SkillSlot.Utility;
            ascentProtocol.requireSkillReady = true;
            ascentProtocol.requireEquipmentReady = false;
            ascentProtocol.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            ascentProtocol.minDistance = 0f;
            ascentProtocol.maxDistance = 100f;
            ascentProtocol.selectionRequiresTargetLoS = true;
            ascentProtocol.activationRequiresTargetLoS = true;
            ascentProtocol.activationRequiresAimConfirmation = false;
            ascentProtocol.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            ascentProtocol.aimType = AISkillDriver.AimType.MoveDirection;
            ascentProtocol.ignoreNodeGraph = false;
            ascentProtocol.noRepeat = true;
            ascentProtocol.shouldSprint = true;
            ascentProtocol.shouldFireEquipment = false;
            ascentProtocol.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Command Doc - Secondary skill for healing
            AISkillDriver commandDoc = gameObject.AddComponent<AISkillDriver>();
            commandDoc.customName = "CommandDoc";
            commandDoc.skillSlot = SkillSlot.Secondary;
            commandDoc.requireSkillReady = true;
            commandDoc.requireEquipmentReady = false;
            commandDoc.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            commandDoc.minDistance = 0f;
            commandDoc.maxDistance = 250f;
            commandDoc.selectionRequiresTargetLoS = true;
            commandDoc.activationRequiresTargetLoS = true;
            commandDoc.activationRequiresAimConfirmation = false;
            commandDoc.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            commandDoc.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            commandDoc.ignoreNodeGraph = false;
            commandDoc.noRepeat = false;
            commandDoc.shouldSprint = false;
            commandDoc.shouldFireEquipment = false;
            commandDoc.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Command Healing Drone - Secondary skill for healing drones
            AISkillDriver commandHealingDrone = gameObject.AddComponent<AISkillDriver>();
            commandHealingDrone.customName = "CommandHealingDrone";
            commandHealingDrone.skillSlot = SkillSlot.Secondary;
            commandHealingDrone.requireSkillReady = true;
            commandHealingDrone.requireEquipmentReady = false;
            commandHealingDrone.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            commandHealingDrone.minDistance = 0f;
            commandHealingDrone.maxDistance = 250f;
            commandHealingDrone.selectionRequiresTargetLoS = true;
            commandHealingDrone.activationRequiresTargetLoS = true;
            commandHealingDrone.activationRequiresAimConfirmation = false;
            commandHealingDrone.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            commandHealingDrone.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            commandHealingDrone.ignoreNodeGraph = false;
            commandHealingDrone.noRepeat = false;
            commandHealingDrone.shouldSprint = false;
            commandHealingDrone.shouldFireEquipment = false;
            commandHealingDrone.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Command Barrier Drone - Secondary skill for barrier drones
            AISkillDriver commandBarrierDrone = gameObject.AddComponent<AISkillDriver>();
            commandBarrierDrone.customName = "CommandBarrierDrone";
            commandBarrierDrone.skillSlot = SkillSlot.Secondary;
            commandBarrierDrone.requireSkillReady = true;
            commandBarrierDrone.requireEquipmentReady = false;
            commandBarrierDrone.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            commandBarrierDrone.minDistance = 0f;
            commandBarrierDrone.maxDistance = 100f;
            commandBarrierDrone.selectionRequiresTargetLoS = true;
            commandBarrierDrone.activationRequiresTargetLoS = true;
            commandBarrierDrone.activationRequiresAimConfirmation = false;
            commandBarrierDrone.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            commandBarrierDrone.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            commandBarrierDrone.ignoreNodeGraph = false;
            commandBarrierDrone.noRepeat = false;
            commandBarrierDrone.shouldSprint = false;
            commandBarrierDrone.shouldFireEquipment = false;
            commandBarrierDrone.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Command Emergency Drone - Secondary skill for emergency healing
            AISkillDriver commandEmergencyDrone = gameObject.AddComponent<AISkillDriver>();
            commandEmergencyDrone.customName = "CommandEmergencyDrone";
            commandEmergencyDrone.skillSlot = SkillSlot.Secondary;
            commandEmergencyDrone.requireSkillReady = true;
            commandEmergencyDrone.requireEquipmentReady = false;
            commandEmergencyDrone.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            commandEmergencyDrone.minDistance = 0f;
            commandEmergencyDrone.maxDistance = float.PositiveInfinity;
            commandEmergencyDrone.selectionRequiresTargetLoS = false;
            commandEmergencyDrone.activationRequiresTargetLoS = false;
            commandEmergencyDrone.activationRequiresAimConfirmation = false;
            commandEmergencyDrone.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            commandEmergencyDrone.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            commandEmergencyDrone.ignoreNodeGraph = false;
            commandEmergencyDrone.noRepeat = false;
            commandEmergencyDrone.shouldSprint = false;
            commandEmergencyDrone.shouldFireEquipment = false;
            commandEmergencyDrone.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            commandEmergencyDrone.maxUserHealthFraction = 0.6f;

            // Command Emergency Drone Friendly - Secondary skill for friendly healing
            AISkillDriver commandEmergencyDroneFriendly = gameObject.AddComponent<AISkillDriver>();
            commandEmergencyDroneFriendly.customName = "CommandEmergencyDroneFriendly";
            commandEmergencyDroneFriendly.skillSlot = SkillSlot.Secondary;
            commandEmergencyDroneFriendly.requireSkillReady = true;
            commandEmergencyDroneFriendly.requireEquipmentReady = false;
            commandEmergencyDroneFriendly.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            commandEmergencyDroneFriendly.minDistance = 0f;
            commandEmergencyDroneFriendly.maxDistance = 50f;
            commandEmergencyDroneFriendly.selectionRequiresTargetLoS = false;
            commandEmergencyDroneFriendly.activationRequiresTargetLoS = false;
            commandEmergencyDroneFriendly.activationRequiresAimConfirmation = false;
            commandEmergencyDroneFriendly.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            commandEmergencyDroneFriendly.aimType = AISkillDriver.AimType.AtMoveTarget;
            commandEmergencyDroneFriendly.ignoreNodeGraph = false;
            commandEmergencyDroneFriendly.noRepeat = false;
            commandEmergencyDroneFriendly.shouldSprint = false;
            commandEmergencyDroneFriendly.shouldFireEquipment = false;
            commandEmergencyDroneFriendly.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            commandEmergencyDroneFriendly.maxTargetHealthFraction = 0.6f;

            // Command Drone - Secondary skill for standard drones
            AISkillDriver commandDrone = gameObject.AddComponent<AISkillDriver>();
            commandDrone.customName = "CommandDrone";
            commandDrone.skillSlot = SkillSlot.Secondary;
            commandDrone.requireSkillReady = true;
            commandDrone.requireEquipmentReady = false;
            commandDrone.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            commandDrone.minDistance = 0f;
            commandDrone.maxDistance = 100f;
            commandDrone.selectionRequiresTargetLoS = true;
            commandDrone.activationRequiresTargetLoS = true;
            commandDrone.activationRequiresAimConfirmation = true;
            commandDrone.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            commandDrone.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            commandDrone.ignoreNodeGraph = false;
            commandDrone.noRepeat = false;
            commandDrone.shouldSprint = false;
            commandDrone.shouldFireEquipment = false;
            commandDrone.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            // Ram Drone - Secondary skill for ram drones
            AISkillDriver ramDrone = gameObject.AddComponent<AISkillDriver>();
            ramDrone.customName = "RamDrone";
            ramDrone.skillSlot = SkillSlot.Secondary;
            ramDrone.requireSkillReady = true;
            ramDrone.requireEquipmentReady = false;
            ramDrone.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            ramDrone.minDistance = 0f;
            ramDrone.maxDistance = 150f;
            ramDrone.selectionRequiresTargetLoS = true;
            ramDrone.activationRequiresTargetLoS = true;
            ramDrone.activationRequiresAimConfirmation = true;
            ramDrone.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            ramDrone.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            ramDrone.ignoreNodeGraph = false;
            ramDrone.noRepeat = true;
            ramDrone.shouldSprint = false;
            ramDrone.shouldFireEquipment = false;
            ramDrone.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            ramDrone.driverUpdateTimerOverride = 1f;

            // Firewall - Utility skill for shield formation
            AISkillDriver firewall = gameObject.AddComponent<AISkillDriver>();
            firewall.customName = "Firewall";
            firewall.skillSlot = SkillSlot.Utility;
            firewall.requireSkillReady = true;
            firewall.requireEquipmentReady = false;
            firewall.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            firewall.minDistance = 0f;
            firewall.maxDistance = 100f;
            firewall.selectionRequiresTargetLoS = true;
            firewall.activationRequiresTargetLoS = true;
            firewall.activationRequiresAimConfirmation = false;
            firewall.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            firewall.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            firewall.ignoreNodeGraph = false;
            firewall.noRepeat = true;
            firewall.shouldSprint = false;
            firewall.shouldFireEquipment = false;
            firewall.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            firewall.driverUpdateTimerOverride = 1.5f;

            // Primary Retreat - Primary skill for defensive combat
            AISkillDriver primaryRetreat = gameObject.AddComponent<AISkillDriver>();
            primaryRetreat.customName = "PrimaryRetreat";
            primaryRetreat.skillSlot = SkillSlot.Primary;
            primaryRetreat.requireSkillReady = true;
            primaryRetreat.requireEquipmentReady = false;
            primaryRetreat.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryRetreat.minDistance = 0f;
            primaryRetreat.maxDistance = 20f;
            primaryRetreat.selectionRequiresTargetLoS = true;
            primaryRetreat.activationRequiresTargetLoS = true;
            primaryRetreat.activationRequiresAimConfirmation = true;
            primaryRetreat.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            primaryRetreat.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryRetreat.ignoreNodeGraph = false;
            primaryRetreat.noRepeat = false;
            primaryRetreat.shouldSprint = false;
            primaryRetreat.shouldFireEquipment = false;
            primaryRetreat.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryRetreat.driverUpdateTimerOverride = 0.5f;

            // Primary Strafe - Primary skill for strafing combat
            AISkillDriver primaryStrafe = gameObject.AddComponent<AISkillDriver>();
            primaryStrafe.customName = "PrimaryStrafe";
            primaryStrafe.skillSlot = SkillSlot.Primary;
            primaryStrafe.requireSkillReady = true;
            primaryStrafe.requireEquipmentReady = false;
            primaryStrafe.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryStrafe.minDistance = 0f;
            primaryStrafe.maxDistance = 60f;
            primaryStrafe.selectionRequiresTargetLoS = true;
            primaryStrafe.activationRequiresTargetLoS = true;
            primaryStrafe.activationRequiresAimConfirmation = true;
            primaryStrafe.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            primaryStrafe.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryStrafe.ignoreNodeGraph = false;
            primaryStrafe.noRepeat = false;
            primaryStrafe.shouldSprint = false;
            primaryStrafe.shouldFireEquipment = false;
            primaryStrafe.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryStrafe.driverUpdateTimerOverride = 0.5f;

            // Primary Chase - Primary skill for chasing enemies
            AISkillDriver primaryChase = gameObject.AddComponent<AISkillDriver>();
            primaryChase.customName = "PrimaryChase";
            primaryChase.skillSlot = SkillSlot.Primary;
            primaryChase.requireSkillReady = true;
            primaryChase.requireEquipmentReady = false;
            primaryChase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryChase.minDistance = 0f;
            primaryChase.maxDistance = 250f;
            primaryChase.selectionRequiresTargetLoS = true;
            primaryChase.activationRequiresTargetLoS = true;
            primaryChase.activationRequiresAimConfirmation = true;
            primaryChase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primaryChase.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryChase.ignoreNodeGraph = false;
            primaryChase.noRepeat = false;
            primaryChase.shouldSprint = false;
            primaryChase.shouldFireEquipment = false;
            primaryChase.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            primaryChase.driverUpdateTimerOverride = 0.5f;

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
            releaseInput.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            releaseInput.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            releaseInput.ignoreNodeGraph = false;
            releaseInput.noRepeat = false;
            releaseInput.shouldSprint = false;
            releaseInput.shouldFireEquipment = false;
            releaseInput.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            // Set up skill driver priorities using nextHighPriorityOverride (ImprovedSurvivorAI approach)
            ramDrone.nextHighPriorityOverride = releaseInput;

            // Add default skills as fallback
            AddDefaultSkills(gameObject, ai, 20);
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