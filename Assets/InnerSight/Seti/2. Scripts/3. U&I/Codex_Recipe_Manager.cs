using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public class Codex_Recipe_Manager : Singleton<Codex_Recipe_Manager>
    {
        // �ʵ�
        #region Variables
        // �ܼ� �ʵ�
        private int firstElement = 0;
        private bool isFirstElement = false;
        private const int identifier = 2000;    // (itemID - identifier >= 0)�� �����۸� �д´�

        // ����-������ UI
        private List<Outputs> outputs = new();
        private List<GameObject> unIdentified = new();

        // Ŭ����
        private PlayerInteraction player;
        [SerializeField]
        private ItemDatabase itemDatabase;
        #endregion

        // �Ӽ�
        public Dictionary<ItemKey, ItemValueRecipe> CodexRecipe { get; private set; } = new();

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            player = FindFirstObjectByType<PlayerInteraction>();
            player.SetCodex(this);

            outputs.AddRange(GetComponentsInChildren<Outputs>());
            foreach (var output in outputs)
            {
                if (output != null)
                    output.GetComponent<Image>().enabled = false;
            }

            Transform undefinedElements = transform.GetChild(0).GetChild(0).GetChild(2);
            Transform undefinedElixirs = transform.GetChild(0).GetChild(0).GetChild(3);
            for (int i = 0; i < 4; i++)
            {
                unIdentified.Add(undefinedElements.GetChild(0).GetChild(i).gameObject);
            }
            for (int i = 0; i < 3; i++)
            {
                unIdentified.Add(undefinedElements.GetChild(1).GetChild(i).gameObject);
            }
            for (int i = 0; i < 4; i++)
            {
                unIdentified.Add(undefinedElixirs.GetChild(0).GetChild(i).gameObject);
            }
            for (int i = 0; i < 4; i++)
            {
                unIdentified.Add(undefinedElixirs.GetChild(1).GetChild(i).gameObject);
            }

            Initialize();
        }
        #endregion

        // �޼���
        #region Methods
        // �������� ȹ���� ������ Ȯ��
        public void IdentifyRecipe(ItemKey itemKey)
        {
            ItemKey elementOrElixir = CollectionUtility.FirstOrNull(CodexRecipe.Keys, key => key.itemID == itemKey.itemID);
            if (elementOrElixir != null)
            {
                CodexRecipe[elementOrElixir].codexDefine = true;

                int i = CodexRecipe[elementOrElixir].codexIndex;
                outputs[i].GetComponent<Image>().enabled = true;
                Destroy(unIdentified[i]);
            }
            return;
        }

        // �ʱ�ȭ - ������DB�κ��� ���ҿ� �������� �о�� ���� ��ųʸ��� ����
        private void Initialize()
        {
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    if (!isFirstElement)
                    {
                        isFirstElement = true;
                        firstElement = i;
                    }

                    ItemValueRecipe valueRecipe = new()
                    {
                        codexIndex = i - firstElement,
                        codexDefine = false
                    };
                    CodexRecipe.Add(itemDatabase.itemList[i], valueRecipe);
                }
            }
        }
        #endregion
    }
}