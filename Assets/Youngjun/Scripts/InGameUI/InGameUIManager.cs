using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using InnerSight_Seti;
using InnerSight_Kys;
using Noah;

namespace MyVRSample
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject left_rayObject;
        [SerializeField] private GameObject right_rayObject;

        public static InGameUIManager instance;

        //public GameObject gameMenu;
        public InventoryManager InventoryManager { get; private set; }
        public GameObject inventory;
        public Transform head;

        public float xOffset = 0f;
        public float yOffset = 1.36f;
        public float distance = 1.5f;

        public InputActionProperty showBtn;
        public InputActionProperty invenBtn;

        // Drop UI
        //public SnapTurnProvider snapTurn;
        //public ContinuousTurnProvider continuousTurn;
        //public Dropdown dropDown;


        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            InventoryManager = GetComponent<InventoryManager>();
        }

        // Update is called once per frame
        void Update()
        {
            //if (showBtn.action.WasPressedThisFrame() && gameMenu != null)
            //{
            //    Toggle();
            //}
            if (invenBtn.action.WasPressedThisFrame() && inventory != null && !ResetManager.Instance.IsReset)
            {
                Toggle_Inven();
            }
            if (InventoryManager.IsOpenInventory)
            {
                left_rayObject.SetActive(true);
                right_rayObject.SetActive(true);
            }

        }

        //void Toggle()
        //{
        //    gameMenu.SetActive(!gameMenu.activeSelf);

        //    // show set
        //    if(gameMenu.activeSelf)
        //    {
        //        gameMenu.transform.position = head.position + new Vector3(head.forward.x, yOffset, head.forward.z).normalized * distance;
                
        //        gameMenu.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));

        //        gameMenu.transform.forward *= -1;
        //    }
        //}

        void Toggle_Inven()
        {
            AudioManager.Instance.Play("Throw");

            //inventory.SetActive(!inventory.activeSelf);
            InventoryManager.ShowItem(InventoryManager.IsOpenInventory = !InventoryManager.IsOpenInventory);


            // show set
            if (inventory.activeSelf)
            {

                left_rayObject.SetActive(false);
                right_rayObject.SetActive(false);

                inventory.transform.position = head.position + new Vector3(head.forward.x + xOffset, yOffset, head.forward.z).normalized * distance;

                inventory.transform.LookAt(new Vector3(head.position.x, inventory.transform.position.y, head.position.z));

                inventory.transform.forward *= -1;
            }
        }

        //public void SetTurnTypeIndex(int index)
        //{
        //    switch (index)
        //    { 
        //        case 0:
        //            continuousTurn.enabled = true;
        //            snapTurn.enabled = false;
        //            break;
        //        case 1:
        //            continuousTurn.enabled = false;
        //            snapTurn.enabled = true;
        //            break;


        //    }
        //}

        public void QuitBtn(GameObject ui)
        {
            AudioManager.Instance.Play("Throw");

            ui.SetActive(false);
        }
    }
}
