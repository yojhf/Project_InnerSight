using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    public static class NPC_State
    {
        public static string IsEntered = "IsEntered";
        public static string Distance = "Distance";
        public static string SelfDistance = "SelfDistance";
    }

    /// <summary>
    /// NPC ������ �θ� Ŭ����
    /// </summary>
    /// 1. ���ο��� �ٰ����� UI ����
    /// 2. UI �ȳ��� ���� Ű �Է��� �ϸ� �ŷ� ���� - �ŷ��� UI
    /// 3. ������ �Ǹ� ���� ��ǰ ���, ������ ���� ���� ��ǰ ���
    /// 4. ������ �����ϰ� ���� ��ư�� ������ �Һ� �� ������ ���� �̷� ����
    /// 5. ���� ù �ŷ��� ��ΰ�(���ప+���Ͽ�), ����
    /// 6. �ŷ� ��!
    public abstract class NPC_Merchant : NPC
    {
        // �ʵ�
        #region Variables
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        #endregion

        // �޼���
        #region Methods
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        #endregion
    }
}