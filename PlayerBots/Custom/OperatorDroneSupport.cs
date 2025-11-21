using System.Collections.Generic;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerBots.Custom
{
    class OperatorDroneSupport : MonoBehaviour
    {
        private const float MinDelaySeconds = 120f;
        private const float MaxDelaySeconds = 240f;
        private const int MaxDronesPerStage = 2;
        private const float RetryDelaySeconds = 5f;
        private const float PaymentRetrySeconds = 60f;
        private const int DefaultBaseDroneCost = 200;
        private const int AdditionalDroneCostPerStage = 100;

        private CharacterMaster master;
        private bool stageGrantPending;
        private float grantReadyStopwatch;
        private int dronesSpawnedThisStage;

        private static WeightedSelection<DroneDef> droneSelection;
        private static Dictionary<DroneDef, int> droneBaseCosts;

        private enum DroneGrantResult
        {
            Success,
            RetryGeneral,
            RetryInsufficientFunds
        }

        private void Awake()
        {
            master = GetComponent<CharacterMaster>();
            Stage.onServerStageBegin += HandleServerStageBegin;

            // Ensure we still grant drones if this component is attached mid-stage.
            if (NetworkServer.active && Run.instance)
            {
                BeginStageWindow();
            }
        }

        private void OnDestroy()
        {
            Stage.onServerStageBegin -= HandleServerStageBegin;
        }

        private void HandleServerStageBegin(Stage stage)
        {
            if (!NetworkServer.active || !Run.instance)
            {
                return;
            }

            BeginStageWindow();
        }

        private void BeginStageWindow()
        {
            dronesSpawnedThisStage = 0;
            stageGrantPending = false;
            if (MaxDronesPerStage > 0)
            {
                ScheduleNextGrantWindow();
            }
        }

        private void ScheduleNextGrantWindow()
        {
            if (!Run.instance)
            {
                stageGrantPending = false;
                return;
            }

            grantReadyStopwatch = Run.instance.GetRunStopwatch() + UnityEngine.Random.Range(MinDelaySeconds, MaxDelaySeconds);
            stageGrantPending = true;
        }

        private void ScheduleRetry()
        {
            if (!Run.instance)
            {
                stageGrantPending = false;
                return;
            }

            grantReadyStopwatch = Run.instance.GetRunStopwatch() + RetryDelaySeconds;
            stageGrantPending = true;
        }

        private void FixedUpdate()
        {
            if (!NetworkServer.active || !stageGrantPending || !Run.instance)
            {
                return;
            }

            if (PlayerBotManager.EnableDroneSupport != null && !PlayerBotManager.EnableDroneSupport.Value)
            {
                return;
            }

            if (Run.instance.GetRunStopwatch() < grantReadyStopwatch)
            {
                return;
            }

            if (!master || master.IsDeadAndOutOfLivesServer())
            {
                grantReadyStopwatch = Run.instance.GetRunStopwatch() + RetryDelaySeconds;
                return;
            }

            CharacterBody ownerBody = master.GetBody();
            if (!ownerBody)
            {
                grantReadyStopwatch = Run.instance.GetRunStopwatch() + RetryDelaySeconds;
                return;
            }

            DroneGrantResult result = TryGrantDrone(ownerBody);
            switch (result)
            {
                case DroneGrantResult.Success:
                    if (dronesSpawnedThisStage >= MaxDronesPerStage)
                    {
                        stageGrantPending = false;
                    }
                    else
                    {
                        ScheduleNextGrantWindow();
                    }
                    break;
                case DroneGrantResult.RetryInsufficientFunds:
                    SchedulePaymentRetry();
                    break;
                default:
                    ScheduleRetry();
                    break;
            }
        }

        private DroneGrantResult TryGrantDrone(CharacterBody ownerBody)
        {
            DroneDef droneDef = SelectDrone();
            if (droneDef == null)
            {
                return DroneGrantResult.RetryGeneral;
            }

            uint cost = GetDroneCost(droneDef);
            if (!master || master.money < cost)
            {
                return DroneGrantResult.RetryInsufficientFunds;
            }

            master.money -= cost;
            if (TrySpawnDrone(droneDef, ownerBody))
            {
                dronesSpawnedThisStage++;
                return DroneGrantResult.Success;
            }

            master.money += cost;
            return DroneGrantResult.RetryGeneral;
        }

        private static DroneDef SelectDrone()
        {
            if (droneSelection == null)
            {
                droneSelection = BuildDroneSelection();
            }

            if (droneSelection == null || droneSelection.Count == 0)
            {
                return null;
            }

            return droneSelection.Evaluate(UnityEngine.Random.value);
        }

        private void SchedulePaymentRetry()
        {
            if (!Run.instance)
            {
                stageGrantPending = false;
                return;
            }

            grantReadyStopwatch = Run.instance.GetRunStopwatch() + PaymentRetrySeconds;
            stageGrantPending = true;
        }

        private uint GetDroneCost(DroneDef droneDef)
        {
            int baseCost = DefaultBaseDroneCost;
            if (droneBaseCosts != null && droneDef != null && droneBaseCosts.TryGetValue(droneDef, out int mappedCost))
            {
                baseCost = mappedCost;
            }

            int stageBonus = Run.instance ? Run.instance.stageClearCount * AdditionalDroneCostPerStage : 0;
            int finalCost = Mathf.Max(baseCost + stageBonus, 0);

            if (Run.instance)
            {
                return (uint)Mathf.Max(Run.instance.GetDifficultyScaledCost(finalCost), 0);
            }

            return (uint)Mathf.Max(finalCost, 0);
        }

        private static WeightedSelection<DroneDef> BuildDroneSelection()
        {
            WeightedSelection<DroneDef> selection = new WeightedSelection<DroneDef>();
            droneBaseCosts = new Dictionary<DroneDef, int>();

            AddGroup(selection, 80f, 40,
                RoR2Content.DroneDefs.Drone1,
                RoR2Content.DroneDefs.Drone2);
            AddGroup(selection, 20f, 100,
                DLC3Content.DroneDefs.HaulerDrone,
                DLC3Content.DroneDefs.JunkDrone,
                RoR2Content.DroneDefs.FlameDrone,
                RoR2Content.DroneDefs.MissileDrone,
                DLC3Content.DroneDefs.CleanupDrone);
            AddGroup(selection, 10f, 100,
                RoR2Content.DroneDefs.EmergencyDrone,
                DLC3Content.DroneDefs.JailerDrone,
                DLC3Content.DroneDefs.RechargeDrone,
                DLC3Content.DroneDefs.BombardmentDrone,
                DLC3Content.DroneDefs.BomberDrone);
            AddGroup(selection, 1f, 350, RoR2Content.DroneDefs.MegaDrone);

            return selection.Count > 0 ? selection : null;
        }

        private static void AddGroup(WeightedSelection<DroneDef> selection, float groupWeight, int baseCost, params DroneDef[] defs)
        {
            if (selection == null || defs == null || defs.Length == 0)
            {
                return;
            }

            int validCount = 0;
            for (int i = 0; i < defs.Length; i++)
            {
                if (defs[i] != null && defs[i].masterPrefab)
                {
                    validCount++;
                }
            }

            if (validCount == 0)
            {
                return;
            }

            float perDroneWeight = groupWeight / validCount;
            for (int i = 0; i < defs.Length; i++)
            {
                DroneDef def = defs[i];
                if (def != null && def.masterPrefab)
                {
                    selection.AddChoice(def, perDroneWeight);
                    droneBaseCosts[def] = baseCost;
                }
            }
        }

        private bool TrySpawnDrone(DroneDef droneDef, CharacterBody ownerBody)
        {
            if (droneDef == null || !droneDef.masterPrefab || ownerBody == null)
            {
                return false;
            }

            Vector3 offset = UnityEngine.Random.insideUnitSphere;
            offset.y = 0f;
            if (offset == Vector3.zero)
            {
                offset = Vector3.forward;
            }
            offset = offset.normalized * 5f;
            Vector3 spawnPosition = ownerBody.corePosition + offset;

            CharacterMaster summoned = new MasterSummon
            {
                masterPrefab = droneDef.masterPrefab,
                position = spawnPosition,
                rotation = Quaternion.identity,
                summonerBodyObject = ownerBody.gameObject,
                teamIndexOverride = TeamIndex.Player,
                ignoreTeamMemberLimit = true,
                useAmbientLevel = true,
                enablePrintController = true
            }.Perform();

            return summoned != null;
        }
    }
}

