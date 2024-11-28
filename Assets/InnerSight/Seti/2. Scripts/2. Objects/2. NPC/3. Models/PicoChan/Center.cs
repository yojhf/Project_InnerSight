using UnityEngine;

namespace WOAT
{
    public class Center : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(this.transform.position, new(5, 2, 7.5f));
        }
    }
}