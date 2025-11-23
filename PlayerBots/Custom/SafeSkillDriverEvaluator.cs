using System;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.Custom
{
    /// <summary>
    /// A safety wrapper around BaseAI to prevent null reference exceptions in skill driver evaluation.
    /// This addresses the compatibility issue with TeammateRevival where skill drivers might not be properly initialized.
    /// </summary>
    public class SafeSkillDriverEvaluator : MonoBehaviour
    {
        private BaseAI ai;
        private CharacterMaster master;
        private float evaluationTimer = 0f;
        private const float EvaluationInterval = 0.1f; // Check every 100ms

        private void Start()
        {
            ai = GetComponent<BaseAI>();
            master = GetComponent<CharacterMaster>();
        }

        private void FixedUpdate()
        {
            if (ai == null || master == null) return;

            evaluationTimer += Time.fixedDeltaTime;
            if (evaluationTimer >= EvaluationInterval)
            {
                evaluationTimer = 0f;
                CheckAndFixSkillDrivers();
            }
        }

        private void CheckAndFixSkillDrivers()
        {
            // Skip if compatibility is disabled
            if (!PlayerBotManager.EnableTeammateRevivalCompatibility.Value) return;

            try
            {
                // Always get fresh skill drivers from the GameObject to avoid null reference issues
                AISkillDriver[] currentDrivers = GetComponents<AISkillDriver>();
                
                if (currentDrivers == null || currentDrivers.Length == 0)
                {
                    return;
                }
                
                // Filter out null drivers
                var validDrivers = new System.Collections.Generic.List<AISkillDriver>();
                
                for (int i = 0; i < currentDrivers.Length; i++)
                {
                    if (currentDrivers[i] != null)
                    {
                        validDrivers.Add(currentDrivers[i]);
                    }
                }
                
                // Check current skill drivers for issues
                bool needsRefresh = false;
                if (ai.skillDrivers == null || ai.skillDrivers.Length == 0)
                {
                    needsRefresh = true;
                }
                else
                {
                    // Check if any skill drivers in the array are null
                    for (int i = 0; i < ai.skillDrivers.Length; i++)
                    {
                        if (ai.skillDrivers[i] == null)
                        {
                            needsRefresh = true;
                            break;
                        }
                    }
                }
                
                if (needsRefresh && validDrivers.Count > 0)
                {
                    ai.ReplaceSkillDrivers(validDrivers.ToArray());
                    
                    // Force immediate re-evaluation
                    ai.SetFieldValue("skillDriverEvaluation", default(BaseAI.SkillDriverEvaluation));
                    ai.SetFieldValue("skillDriverUpdateTimer", 0f);
                }
            }
            catch (System.Exception)
            {
                // Silently handle errors to prevent spam
            }
        }
    }
}