using MyFPS;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace MyVRSample
{
    public class FireBulletOnActivate : MonoBehaviour
    {
        public GameObject bullet;
        public Transform firePoint;

        public float bulletSpeed = 20f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

            grabInteractable.activated.AddListener(FireBullet);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FireBullet(ActivateEventArgs args)
        {
            GameObject _bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

            _bullet.GetComponent<Rigidbody>().linearVelocity = firePoint.forward * bulletSpeed;

            Destroy(_bullet, 3f);
        }
    }
}