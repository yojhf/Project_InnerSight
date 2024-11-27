using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

namespace MyVRSample
{
    public class InGameUIManager : MonoBehaviour
    {
        public static InGameUIManager instance;

        //public GameObject gameMenu;
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

        }

        // Update is called once per frame
        void Update()
        {
            //if (showBtn.action.WasPressedThisFrame() && gameMenu != null)
            //{
            //    Toggle();
            //}
            if (invenBtn.action.WasPressedThisFrame() && inventory != null)
            {
                Toggle_Inven();
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
            inventory.SetActive(!inventory.activeSelf);

            // show set
            if (inventory.activeSelf)
            {
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
            ui.SetActive(false);
        }
    }
}
