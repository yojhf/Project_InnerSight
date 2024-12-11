using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class NPCCount : MonoBehaviour
    {
        [SerializeField] private int npc_Count = 0;

        public void ResetNPCCount()
        {
            npc_Count = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            NPC_Customer npc_Customer = other.GetComponent<NPC_Customer>();

            if (npc_Customer != null)
            {
                npc_Count++;

                GameOverManager.Instance.NPCFull(npc_Count);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            NPC_Customer npc_Customer = other.GetComponent<NPC_Customer>();

            if (npc_Customer != null)
            {
                npc_Count--;
            }
        }
    }
}