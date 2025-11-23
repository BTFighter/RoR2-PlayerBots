using System;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.Custom
{
    /// <summary>
    /// Component that handles delayed skill driver initialization for compatibility with TeammateRevival.
    /// This helps ensure that skill drivers are properly set up after any potential interference from other mods.
    /// </summary>
    public class BotSkillDriverInitializer : MonoBehaviour
    {
        private const float InitializationDelay = 0.2f;
        private bool initialized = false;
        private float timer = 0f;

        private void Update()
        {
            if (initialized) return;

            timer += Time.deltaTime;
            if (timer >= InitializationDelay)
            {
                InitializeSkillDrivers();
                initialized = true;
            }
        }

        private void InitializeSkillDrivers()
        {
            var ai = GetComponent<BaseAI>();
            var master = GetComponent<CharacterMaster>();
            
            if (ai == null || master == null) 
            {
                Destroy(this);
                return;
            }

            try
            {
                // Get the actual skill drivers from the GameObject components
                AISkillDriver[] currentDrivers = GetComponents<AISkillDriver>();
                
                if (currentDrivers != null && currentDrivers.Length > 0)
                {
                    // Filter out null drivers and only keep valid ones
                    var validDrivers = new System.Collections.Generic.List<AISkillDriver>();
                    
                    for (int i = 0; i < currentDrivers.Length; i++)
                    {
                        if (currentDrivers[i] != null)
                        {
                            validDrivers.Add(currentDrivers[i]);
                        }
                    }
                    
                    if (validDrivers.Count > 0)
                    {
                        ai.ReplaceSkillDrivers(validDrivers.ToArray());
                    }
                }
                
                // Force re-evaluation
                ai.SetFieldValue("skillDriverEvaluation", default(BaseAI.SkillDriverEvaluation));
                ai.SetFieldValue("targetRefreshTimer", 0f);
                ai.SetFieldValue("skillDriverUpdateTimer", 0f);
                ai.SetBaseAIEnabled(true);
            }
            catch (Exception e)
            {
                // Silently handle errors to prevent spam
            }

            // Clean up this component since it's no longer needed
            Destroy(this);
        }
    }
}