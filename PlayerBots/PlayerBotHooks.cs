using HG;
using MonoMod.Cil;
using RoR2;
using RoR2.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using static On.RoR2.CharacterAI.BaseAI.Target;

namespace PlayerBots
{
    class PlayerBotHooks
    {
        // Track which Seekers have revived this stage (per-Seeker restriction)
        private static Dictionary<UnityEngine.Networking.NetworkInstanceId, int> seekerReviveStages = new Dictionary<UnityEngine.Networking.NetworkInstanceId, int>();
        public static void AddHooks()
        {
            // Ugh.
            On.RoR2.CharacterAI.BaseAI.OnBodyLost += (orig, self, body) =>
            {
                if (self.name.Equals("PlayerBot"))
                {
                    // Reset player bot state when body is lost so errors dont pop up
                    self.stateMachine.SetNextState(EntityStateCatalog.InstantiateState(ref self.scanState));
                    return;
                }
                orig(self, body);
            };

            // Random fix to make captains spawnable without errors in PlayerMode, theres probably a better way of doing this too
            On.RoR2.CaptainDefenseMatrixController.OnServerMasterSummonGlobal += (orig, self, summonReport) =>
            {
                if (self.GetFieldValue<CharacterBody>("characterBody") == null)
                {
                    return;
                }
                orig(self, summonReport);
            };

            // Maybe there is a better way to do this
            if (PlayerBotManager.ShowNameplates.Value && !PlayerBotManager.PlayerMode.Value)
            {
                IL.RoR2.TeamComponent.SetupIndicator += il =>
                {
                    ILCursor c = new ILCursor(il);
                    c.GotoNext(x => x.MatchCallvirt<CharacterBody>("get_isPlayerControlled"));
                    bool isPlayerBot = false;
                    c.EmitDelegate<Func<CharacterBody, CharacterBody>>(x =>
                    {
                        isPlayerBot = x.master.name.Equals("PlayerBot");
                        return x;
                    }
                    );
                    c.Index += 1;
                    c.EmitDelegate<Func<bool, bool>>(x =>
                    {
                        if (isPlayerBot) return true;
                        return x;
                    }
                    );
                };
            }

            if (!PlayerBotManager.PlayerMode.Value && PlayerBotManager.AutoPurchaseItems.Value)
            {
                // Give bots money
                On.RoR2.TeamManager.GiveTeamMoney_TeamIndex_uint += (orig, self, teamIndex, money) =>
                {
                    orig(self, teamIndex, money);

                    if (PlayerBotManager.playerbots.Count > 0)
                    {
                        int num = Run.instance ? Run.instance.livingPlayerCount : 0;
                        if (num != 0)
                        {
                            money = (uint)Mathf.CeilToInt(money / (float)num);
                        }
                        foreach (GameObject playerbot in PlayerBotManager.playerbots)
                        {
                            if (!playerbot)
                            {
                                continue;
                            }
                            CharacterMaster master = playerbot.GetComponent<CharacterMaster>();
                            if (master && !master.IsDeadAndOutOfLivesServer() && master.teamIndex == teamIndex)
                            {
                                master.GiveMoney(money);
                            }
                        }
                    }
                };
            }

            if (PlayerBotManager.AutoPurchaseItems.Value)
            {
                On.RoR2.Run.BeginStage += (orig, self) =>
                {
                    foreach (GameObject playerbot in PlayerBotManager.playerbots.ToArray())
                    {
                        if (!playerbot)
                        {
                            PlayerBotManager.playerbots.Remove(playerbot);
                            continue;
                        }

                        ItemManager itemManager = playerbot.GetComponent<ItemManager>();
                        if (itemManager)
                        {
                            itemManager.ResetPurchases();
                            itemManager.master.money = 0;
                        }
                    }
                    orig(self);
                };
            }

            On.RoR2.Stage.Start += (orig, self) =>
            {
                var ret = orig(self);
                if (NetworkServer.active)
                {
                    if (PlayerBotManager.PlayerMode.Value)
                    {
                        foreach (GameObject playerbot in PlayerBotManager.playerbots.ToArray())
                        {
                            if (!playerbot)
                            {
                                PlayerBotManager.playerbots.Remove(playerbot);
                                continue;
                            }

                            CharacterMaster master = playerbot.GetComponent<CharacterMaster>();
                            if (master)
                            {
                                Stage.instance.RespawnCharacter(master);
                            }
                        }
                    }
                    // Spawn starting bots
                    if (Run.instance.stageClearCount == 0)
                    {
                        if (PlayerBotManager.InitialRandomBots.Value > 0)
                        {
                            PlayerBotManager.SpawnRandomPlayerbots(NetworkUser.readOnlyInstancesList[0].master, PlayerBotManager.InitialRandomBots.Value);
                        }
                        for (int randomSurvivorsIndex = 0; randomSurvivorsIndex < PlayerBotManager.InitialBots.Length; randomSurvivorsIndex++)
                        {
                            if (PlayerBotManager.InitialBots[randomSurvivorsIndex].Value > 0)
                            {
                                PlayerBotManager.SpawnPlayerbots(NetworkUser.readOnlyInstancesList[0].master, PlayerBotManager.RandomSurvivorsList[randomSurvivorsIndex], PlayerBotManager.InitialBots[randomSurvivorsIndex].Value);
                            }
                        }
                    }
                }
                return ret;
            };

            // Fix custom targets
            On.RoR2.CharacterAI.BaseAI.Target.GetBullseyePosition += Hook_GetBullseyePosition;

            On.RoR2.BossGroup.DropRewards += (orig, self) =>
            {
                bool shouldFilterBots = ShouldFilterBotsForBossGroup(self);
                if (shouldFilterBots)
                {
                    DropRewardsExcludingBots(self);
                    return;
                }

                orig(self);
            };

            On.RoR2.ShrineColossusAccessBehavior.OnInteraction += (orig, self, interactor) =>
            {
                orig(self, interactor);
                ApplyShrineOfShapingToBots(self);
            };

            // Player mode
            if (PlayerBotManager.PlayerMode.Value)
            {
                On.RoR2.SceneDirector.PlaceTeleporter += (orig, self) =>
                {
                    if (PlayerBotManager.DontScaleInteractables.Value)
                    {
                        int count = PlayerCharacterMasterController.instances.Count((PlayerCharacterMasterController v) => v.networkUser);
                        float num = 0.5f + (float)count * 0.5f;
                        ClassicStageInfo component = SceneInfo.instance.GetComponent<ClassicStageInfo>();
                        int credit = (int)((float)component.sceneDirectorInteractibleCredits * num);
                        if (component.bonusInteractibleCreditObjects != null)
                        {
                            for (int i = 0; i < component.bonusInteractibleCreditObjects.Length; i++)
                            {
                                ClassicStageInfo.BonusInteractibleCreditObject bonusInteractibleCreditObject = component.bonusInteractibleCreditObjects[i];
                                if (bonusInteractibleCreditObject.objectThatGrantsPointsIfEnabled.activeSelf)
                                {
                                    credit += bonusInteractibleCreditObject.points;
                                }
                            }
                        }
                        self.interactableCredit = credit;
                    }
                    else if (Run.instance.stageClearCount == 0 && PlayerBotManager.GetInitialBotCount() > 0)
                    {
                        int count = PlayerCharacterMasterController.instances.Count((PlayerCharacterMasterController v) => v.networkUser);
                        count += PlayerBotManager.GetInitialBotCount();
                        float num = 0.5f + (float)count * 0.5f;
                        ClassicStageInfo component = SceneInfo.instance.GetComponent<ClassicStageInfo>();
                        int credit = (int)((float)component.sceneDirectorInteractibleCredits * num);
                        if (component.bonusInteractibleCreditObjects != null)
                        {
                            for (int i = 0; i < component.bonusInteractibleCreditObjects.Length; i++)
                            {
                                ClassicStageInfo.BonusInteractibleCreditObject bonusInteractibleCreditObject = component.bonusInteractibleCreditObjects[i];
                                if (bonusInteractibleCreditObject.objectThatGrantsPointsIfEnabled.activeSelf)
                                {
                                    credit += bonusInteractibleCreditObject.points;
                                }
                            }
                        }
                        self.interactableCredit = credit;
                    }

                    orig(self);
                };

                // Entitlements. Required for dlc survivors. TODO: Find a better way
                On.RoR2.ExpansionManagement.ExpansionRequirementComponent.PlayerCanUseBody += (orig, self, master) =>
                {
                    if (master.name.Equals("PlayerBot"))
                    {
                        return true;
                    }
                    return orig(self, master);
                };

                // Required for bots to even move, maybe switch to il later
                On.RoR2.PlayerCharacterMasterController.Update += (orig, self) =>
                {
                    if (self.name.Equals("PlayerBot"))
                    {
                        //self.InvokeMethod("SetBody", new object[] { self.master.GetBodyObject() });
                        return;
                    }
                    orig(self);
                };

                try
                {
                    Type ilAllyCardManager = typeof(IL.RoR2.UI.AllyCardManager);
                    EventInfo populateEvent = ilAllyCardManager.GetEvent("PopulateCharacterDataSet", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (populateEvent != null)
                    {
                        MonoMod.Cil.ILContext.Manipulator manipulator = il =>
                {
                    ILCursor c = new ILCursor(il);
                    c.GotoNext(x => x.MatchCallvirt<CharacterMaster>("get_playerCharacterMasterController"));
                    c.Index += 2;
                    c.EmitDelegate<Func<bool, bool>>(x => false);
                };

                        MethodInfo addMethod = populateEvent.GetAddMethod(true); // true for non-public
                        addMethod.Invoke(null, new object[] { manipulator });
                    }
                }
                catch (Exception e)
                {
                    PlayerBots.PlayerBotManager.BotLogger.LogError("Failed to apply AllyCardManager hook: " + e);
                }

                // Spectator fix
                On.RoR2.CameraRigControllerSpectateControls.CanUserSpectateBody += (orig, viewer, body) =>
                {
                    return body.isPlayerControlled || orig(viewer, body);
                };

                // Dont end game on dying
                if (PlayerBotManager.ContinueAfterDeath.Value)
                {
                    IL.RoR2.Stage.Update += il =>
                    {
                        ILCursor c = new ILCursor(il);
                        c.GotoNext(x => x.MatchCallvirt<PlayerCharacterMasterController>("get_isConnected"));
                        c.Index += 1;
                        c.EmitDelegate<Func<bool, bool>>(x => true);
                    };
                }
            }

            // --- Seeker revive PlayerBots hook ---
            // Patch SeekerController.UnlockGateEffects to also revive PlayerBots
            // Restrict to once per stage
            On.RoR2.SeekerController.UnlockGateEffects += (orig, self, chakraGate) =>
            {
                orig(self, chakraGate);

                // Only run on server
                if (!UnityEngine.Networking.NetworkServer.active)
                    return;

                // Only trigger if the Seeker is a player or a bot
                var seekerBody = self.GetComponent<CharacterBody>();
                if (seekerBody == null || seekerBody.master == null)
                    return;

                // Only trigger on full chakra (gate 7, same as vanilla logic)
                if (chakraGate < 7)
                    return;

                // Restrict each Seeker to once per stage
                int currentStage = Run.instance ? Run.instance.stageClearCount : -1;
                var seekerNetId = self.GetComponent<UnityEngine.Networking.NetworkIdentity>()?.netId ?? UnityEngine.Networking.NetworkInstanceId.Invalid;
                
                // Check if this Seeker has already revived this stage
                if (seekerReviveStages.TryGetValue(seekerNetId, out int lastReviveStage) && lastReviveStage == currentStage)
                    return;
                
                // Mark this Seeker as having revived this stage
                seekerReviveStages[seekerNetId] = currentStage;

                // Find all dead PlayerBots (not already revived)
                foreach (var botObj in PlayerBotManager.playerbots.ToArray())
                {
                    if (!botObj) continue;
                    var master = botObj.GetComponent<CharacterMaster>();
                    if (master == null) continue;
                    // Revive all dead PlayerBots (each Seeker can revive all dead bots once per stage)
                    if (master.IsDeadAndOutOfLivesServer())
                    {
                        Vector3 pos = master.deathFootPosition;
                        // Try to find a safe position near the Seeker
                        if (seekerBody)
                        {
                            pos = seekerBody.footPosition + new Vector3(
                                UnityEngine.Random.Range(-2f, 2f),
                                0.5f,
                                UnityEngine.Random.Range(-2f, 2f));
                        }
                        master.Respawn(pos, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f), true);
                        var body = master.GetBody();
                        if (body)
                        {
                            body.AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
                            foreach (var esm in body.GetComponents<EntityStateMachine>())
                                esm.initialStateType = esm.mainStateType;
                            var effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/TeleporterHealNovaPulse");
                            if (effect)
                                EffectManager.SpawnEffect(effect, new EffectData { origin = pos, rotation = body.transform.rotation }, true);
                        }
                    }
                }
            };
        }

        public static bool Hook_GetBullseyePosition(orig_GetBullseyePosition orig, global::RoR2.CharacterAI.BaseAI.Target self, out Vector3 position)
        {
            orig(self, out position);
            return true;
        }

        private static bool ShouldFilterBotsForBossGroup(BossGroup bossGroup)
        {
            if (!NetworkServer.active || bossGroup == null)
            {
                return false;
            }

            if (PlayerBotManager.playerbots.Count == 0)
            {
                return false;
            }

            TeleporterInteraction teleporter = TeleporterInteraction.instance;
            return teleporter && teleporter.bossGroup == bossGroup;
        }

        private static void DropRewardsExcludingBots(BossGroup bossGroup)
        {
            if (!bossGroup)
            {
                return;
            }

            Run run = Run.instance;
            if (!run)
            {
                Debug.LogError("No valid run instance!");
                return;
            }

            Xoroshiro128Plus rng = bossGroup.GetFieldValue<Xoroshiro128Plus>("rng");
            if (rng == null)
            {
                Debug.LogError("RNG is null!");
                return;
            }

            int participatingPlayerCount = GetNonBotParticipatingPlayerCount();
            if (participatingPlayerCount == 0)
            {
                return;
            }

            if (bossGroup.dropPosition)
            {
                UniquePickup basePickup = UniquePickup.none;
                if (bossGroup.dropTable)
                {
                    basePickup = bossGroup.dropTable.GeneratePickup(rng);
                }
                else
                {
                    List<PickupIndex> list = bossGroup.forceTier3Reward ? run.availableTier3DropList : run.availableTier2DropList;
                    if (list != null && list.Count > 0)
                    {
                        basePickup = new UniquePickup(rng.NextElementUniform(list));
                    }
                }

                int dropCount = 1 + bossGroup.bonusRewardCount;
                if (bossGroup.scaleRewardsByPlayerCount)
                {
                    dropCount *= participatingPlayerCount;
                }

                if (dropCount <= 0)
                {
                    return;
                }

                float angle = 360f / dropCount;
                Vector3 vector = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
                Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
                List<UniquePickup> bossDrops = bossGroup.GetFieldValue<List<UniquePickup>>("bossDrops");
                List<PickupDropTable> bossDropTables = bossGroup.GetFieldValue<List<PickupDropTable>>("bossDropTables");
                bool hasBossDrops = bossDrops != null && bossDrops.Count > 0;
                bool hasBossTables = bossDropTables != null && bossDropTables.Count > 0;

                for (int i = 0; i < dropCount; i++)
                {
                    UniquePickup pickup = basePickup;
                    if (bossDrops != null && (hasBossDrops || hasBossTables) && rng.nextNormalizedFloat <= bossGroup.bossDropChance)
                    {
                        if (hasBossTables)
                        {
                            PickupDropTable pickupDropTable = rng.NextElementUniform(bossDropTables);
                            if (pickupDropTable != null)
                            {
                                pickup = pickupDropTable.GeneratePickup(rng);
                            }
                        }
                        else
                        {
                            pickup = rng.NextElementUniform(bossDrops);
                        }
                    }

                    PickupDropletController.CreatePickupDroplet(pickup, bossGroup.dropPosition.position, vector);
                    vector = quaternion * vector;
                }
            }
            else
            {
                Debug.LogWarning("dropPosition not set for BossGroup! No item will be spawned.");
            }
        }

        private static int GetNonBotParticipatingPlayerCount()
        {
            int count = 0;
            foreach (PlayerCharacterMasterController controller in PlayerCharacterMasterController.instances)
            {
                if (controller && !IsPlayerBotController(controller))
                {
                    count++;
                }
            }
            return count;
        }

        private static bool IsPlayerBotController(PlayerCharacterMasterController controller)
        {
            if (!controller)
            {
                return false;
            }

            if (controller.name.Equals("PlayerBot", StringComparison.Ordinal))
            {
                return true;
            }

            CharacterMaster master = controller.master;
            return master && master.name.Equals("PlayerBot", StringComparison.Ordinal);
        }

        private static void ApplyShrineOfShapingToBots(ShrineColossusAccessBehavior shrine)
        {
            if (!NetworkServer.active || shrine == null || PlayerBotManager.playerbots.Count == 0)
            {
                return;
            }

            NodeGraph nodeGraph = SceneInfo.instance ? SceneInfo.instance.GetNodeGraph(MapNodeGroup.GraphType.Ground) : null;
            List<NodeGraph.NodeIndex> nodes = nodeGraph?.FindNodesInRange(shrine.gameObject.transform.position, 10f, 30f, HullMask.Human);
            bool canUseNodes = nodes != null && nodes.Count > 0;

            foreach (GameObject bot in PlayerBotManager.playerbots.ToArray())
            {
                if (!bot)
                {
                    PlayerBotManager.playerbots.Remove(bot);
                    continue;
                }

                CharacterMaster master = bot.GetComponent<CharacterMaster>();
                if (!master)
                {
                    continue;
                }

                if (master.IsDeadAndOutOfLivesServer())
                {
                    Vector3 position = shrine.transform.position;
                    if (canUseNodes)
                    {
                        NodeGraph.NodeIndex nodeIndex = nodes[UnityEngine.Random.Range(0, nodes.Count)];
                        nodeGraph.GetNodePosition(nodeIndex, out position);
                    }
                    Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
                    master.Respawn(position, rotation, true);
                    CharacterBody body = master.GetBody();
                    if (body)
                    {
                        body.AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
                        foreach (EntityStateMachine esm in body.GetComponents<EntityStateMachine>())
                        {
                            esm.initialStateType = esm.mainStateType;
                        }
                    }
                    continue;
                }

                CharacterBody activeBody = master.GetBody();
                if (!activeBody || activeBody.healthComponent == null || !activeBody.healthComponent.alive)
                {
                    continue;
                }

                activeBody.AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
                activeBody.AddBuff(DLC2Content.Buffs.ExtraLifeBuff);
            }
        }

    }
}
