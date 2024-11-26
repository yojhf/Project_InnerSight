using UnityEngine;

namespace InnerSight_Seti
{
    public abstract class Interactive : MonoBehaviour
    {
        protected Player player;
        protected Transform headRoot;

        protected virtual void Start()
        {
            player = FindFirstObjectByType<Player>();
            headRoot = player.transform.Find("Head_Root");
        }

        protected virtual void LateUpdate()
        {
            AttentionPlayer();
        }

        protected virtual void AttentionPlayer()
        {
            Vector3 offset = headRoot.TransformDirection(new(0.2f, 0, -1.5f));
            Vector3 direction = (this.transform.position - (headRoot.position + offset)).normalized;

            this.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}