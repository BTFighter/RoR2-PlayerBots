using EntityStates;
using EntityStates.AI.Walker;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace PlayerBots.Custom
{
    class PlayerBotBaseAI : BaseAI
    {
        // Personal space variables
        private float personalSpaceRadius = 3f;
        private float personalSpaceCheckInterval = 0.2f;
        private float lastPersonalSpaceCheck = 0f;
        private float avoidanceForce = 5f;

        PlayerBotBaseAI()
        {
            this.scanState = new SerializableEntityStateType(typeof(Wander));
            this.fullVision = true;
            this.aimVectorDampTime = .0005f;
            this.aimVectorMaxSpeed = 18000f; // Keeping the high aim speed as requested
            this.enemyAttentionDuration = 3f;
            this.selectedSkilldriverName = "";
            this.neverRetaliateFriendlies = true;
        }

        public override void OnBodyDeath(CharacterBody characterBody)
        {
            if (this.body)
            {
                int num = UnityEngine.Random.Range(0, 37);
                string baseToken = "PLAYER_DEATH_QUOTE_" + num;
                Chat.SendBroadcastChat(new Chat.PlayerDeathChatMessage
                {
                    subjectAsCharacterBody = this.body,
                    baseToken = baseToken,
                    paramTokens = new string[]
                        {
                        this.master.name
                        }
                });
            }
        }

        // Handle personal space checks
        public void Update()
        {
            // Check personal space
            CheckPersonalSpace();
        }

        public void CheckPersonalSpace()
        {
            // Only check periodically to avoid performance issues
            if (Time.time < lastPersonalSpaceCheck + personalSpaceCheckInterval) return;
            lastPersonalSpaceCheck = Time.time;

            if (this.body == null) return;

            // Get all player bots
            var playerBots = PlayerBotManager.playerbots;
            if (playerBots == null || playerBots.Count <= 1) return;

            Vector3 myPosition = this.body.transform.position;
            Vector3 avoidanceDirection = Vector3.zero;
            int avoidanceCount = 0;

            // Check distance to other bots
            foreach (GameObject bot in playerBots)
            {
                if (bot == null || bot == this.gameObject) continue;

                CharacterBody botBody = bot.GetComponent<CharacterBody>();
                if (botBody == null) continue;

                float distance = Vector3.Distance(myPosition, botBody.transform.position);
                
                // If too close, calculate avoidance direction
                if (distance < personalSpaceRadius)
                {
                    Vector3 direction = (myPosition - botBody.transform.position).normalized;
                    float force = 1f - (distance / personalSpaceRadius); // Stronger force when closer
                    avoidanceDirection += direction * force;
                    avoidanceCount++;
                }
            }

            // If we need to avoid other bots
            if (avoidanceCount > 0)
            {
                // Normalize and apply avoidance force
                avoidanceDirection /= avoidanceCount;
                avoidanceDirection.Normalize();

                // Create a temporary target to move away from other bots
                if (this.customTarget?.gameObject == null || this.customTarget.gameObject.name != "AvoidanceTarget")
                {
                    this.customTarget.gameObject = new GameObject("AvoidanceTarget");
                }

                // Position the avoidance target in the direction we want to move
                Vector3 targetPosition = myPosition + avoidanceDirection * avoidanceForce;
                this.customTarget.gameObject.transform.position = targetPosition;

                // Set a timer to clear the avoidance target
                Invoke("ClearAvoidanceTarget", 0.5f);
            }
        }

        private void ClearAvoidanceTarget()
        {
            if (this.customTarget?.gameObject?.name == "AvoidanceTarget")
            {
                this.customTarget.gameObject = null;
            }
        }
    }
}
