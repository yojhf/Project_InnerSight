using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� ���� ������ �����ϴ� Ŭ����
    /// </summary>
    public class Cost_Random : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // �Ϲ� �ʵ�
        private const int identifier = 4000;    // �������� �д´�

        // ������ �����ͺ��̽�
        [SerializeField]
        private ItemDatabase itemDatabase;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        #endregion

        // �޼���
        #region Methods
        // �ʱ�ȭ - ������DB�κ��� �������� �о�� ���� ��ųʸ��� ����
        /*private void Initialize()
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
        }*/
        #endregion
    }
}