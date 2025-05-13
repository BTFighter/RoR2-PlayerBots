using MonoMod.Cil;
using RoR2;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static On.RoR2.CharacterAI.BaseAI.Target;

namespace PlayerBots
{
    class PlayerBotHooks
    {
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

            // Bot sacrifice revival
            if (PlayerBotManager.BotSacrificeRevive.Value)
            {
                On.RoR2.CharacterMaster.OnBodyDeath += (orig, self, body) =>
                {
                    // Only handle player deaths
                    if (self.GetBody()?.isPlayerControlled == true && !self.name.Equals("PlayerBot"))
                    {
                        // Find a living summoned bot
                        var summonedBot = CharacterMaster.readOnlyInstancesList
                            .FirstOrDefault(m => 
                                m.name.Equals("PlayerBot") && 
                                !m.IsDeadAndOutOfLivesServer() && 
                                !m.GetComponent<PlayerCharacterMasterController>() &&
                                // Don't sacrifice Seeker if there are other players/bots alive
                                !(m.GetBody()?.bodyIndex == BodyCatalog.FindBodyIndex("Seeker") && 
                                  (NetworkUser.readOnlyInstancesList.Count(u => !u.master.IsDeadAndOutOfLivesServer()) > 1 ||
                                   CharacterMaster.readOnlyInstancesList.Count(m2 => 
                                       m2.name.Equals("PlayerBot") && 
                                       !m2.IsDeadAndOutOfLivesServer() && 
                                       m2.GetComponent<PlayerCharacterMasterController>() != null) > 0)));

                        if (summonedBot != null)
                        {
                            // Get bot's position before killing it
                            Vector3 botPosition = summonedBot.GetBody().transform.position;
                            Quaternion botRotation = summonedBot.GetBody().transform.rotation;

                            // Kill the bot
                            summonedBot.TrueKill();

                            // Delay respawn by 1 second
                            self.StartCoroutine(DelayedRespawn(self, botPosition, botRotation));
                            return;
                        }
                    }
                    orig(self, body);
                };
            }

            // Spectator fix - moved outside PlayerMode check to work for clients
            On.RoR2.CameraRigControllerSpectateControls.CanUserSpectateBody += (orig, viewer, body) =>
            {
                if (body.master && body.master.name.Equals("PlayerBot"))
                {
                    Debug.Log($"[PlayerBots] CanUserSpectateBody called for PlayerBot: EnablePseudoPlayerMode={PlayerBotManager.EnablePseudoPlayerMode.Value}, PlayerMode={PlayerBotManager.PlayerMode.Value}");
                    if (PlayerBotManager.PlayerMode.Value || PlayerBotManager.EnablePseudoPlayerMode.Value)
                        return true; // Force allow for debugging
                }
                return body.isPlayerControlled || orig(viewer, body);
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

                IL.RoR2.UI.AllyCardManager.PopulateCharacterDataSet += il =>
                {
                    ILCursor c = new ILCursor(il);
                    c.GotoNext(x => x.MatchCallvirt<CharacterMaster>("get_playerCharacterMasterController"));
                    c.Index += 2;
                    c.EmitDelegate<Func<bool, bool>>(x => false);
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
            else if (PlayerBotManager.EnablePseudoPlayerMode.Value)
            {
                // Increment player count for each bot when PlayerMode is off
                On.RoR2.Run.FixedUpdate += (orig, self) =>
                {
                    orig(self);
                    if (!PlayerBotManager.PlayerMode.Value)
                    {
                        // Count only bots that are spawned as summons (not in PlayerMode)
                        int botCount = CharacterMaster.readOnlyInstancesList.Count(m => 
                            m.name.Equals("PlayerBot") && 
                            !m.IsDeadAndOutOfLivesServer() && 
                            !m.GetComponent<PlayerCharacterMasterController>());
                        
                        if (botCount > 0)
                        {
                            self.SetFieldValue("livingPlayerCount", self.GetFieldValue<int>("livingPlayerCount") + botCount);
                        }
                    }
                };
            }
        }

        public static bool Hook_GetBullseyePosition(orig_GetBullseyePosition orig, global::RoR2.CharacterAI.BaseAI.Target self, out Vector3 position)
        {
            orig(self, out position);
            return true;
        }

        private static System.Collections.IEnumerator DelayedRespawn(CharacterMaster master, Vector3 position, Quaternion rotation)
        {
            yield return new WaitForSeconds(1f);
            master.Respawn(position, rotation);
        }

    }
}
