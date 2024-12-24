using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class NPCGenManager : Singleton<NPCGenManager>
    {
        [SerializeField] private NPC_Manager _manager;

        public void NPCGenTimeUp()
        {
            if (_manager.NPC_GenMaxTime >= 5)
            {
                _manager.NPC_GenMaxTime *= 0.85f;
            }
            else
            {
                _manager.NPC_GenMaxTime = 5;
            }

        }
    }

}
