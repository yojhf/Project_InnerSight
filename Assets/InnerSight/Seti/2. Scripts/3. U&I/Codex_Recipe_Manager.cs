using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// ����-���չ� UI
    /// </summary>
    /// 0. ���������� ����� ������ ����Ʈ? ��ųʸ�<������, �Ҹ���>
    /// 1. �ռ����� �� �ƿ�ǲ�� ��� �ν��� ���ΰ�? �κ��丮�� ���� ��
    /// 2. ������DB���� ���ҿ� �������� �д� �����? ID���� 2000�� ���� �� ����� ������
    /// 3. true = ������, false = ???
    /// 4. UI
    public class Codex_Recipe_Manager : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // �ܼ� �ʵ�
        private const int identifier = 2000;    // (itemID - identifier >= 0)�� �����۸� �д´�

        // ����-������ UI
        [SerializeField]
        private GameObject UI_Codex_Recipe;
        private List<GameObject> unIdentified_Elements = new();
        private List<GameObject> unIdentified_Elixirs = new();

        // Ŭ����
        [SerializeField]
        private ItemDatabase itemDatabase;
        private InventoryManager inventoryManager;
        #endregion

        // �Ӽ�
        public Dictionary<ItemKey, ItemValueRecipe> CodexRecipe { get; private set; } = new();

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            //unIdentified_Elements

            inventoryManager = GetComponentInParent<InventoryManager>();
            Initialize();
        }
        #endregion

        // �޼���
        #region Methods
        // �ʱ�ȭ - ������DB�κ��� ���ҿ� �������� �о�� ���� ��ųʸ��� ����
        private void Initialize()
        {
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    ItemValueRecipe valueRecipe = new()
                    {
                        codexIndex = i,
                        codexDefine = false
                    };
                    CodexRecipe.Add(itemDatabase.itemList[i], valueRecipe);
                }
            }
        }
        #endregion
    }
}