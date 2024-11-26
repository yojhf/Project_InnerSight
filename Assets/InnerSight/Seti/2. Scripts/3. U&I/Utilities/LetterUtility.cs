using UnityEngine;

namespace InnerSight_Seti
{
    public static class LetterUtility
    {
        public static bool HasFinalConsonant(char ch)
        {
            // �ѱ� �����ڵ� ���� ���� �ִ��� Ȯ�� (0xAC00 ~ 0xD7A3)
            if (ch < 0xAC00 || ch > 0xD7A3)
            {
                return false; // �ѱ��� �ƴϸ� ��ħ �������� ����
            }

            // �ѱ� �����ڵ� ���� '��'�� �� ������ ���
            int unicodeValue = ch - 0xAC00;
            int jongseongIndex = unicodeValue % 28; // 28�� ���� �������� ���� (��ħ) �ε���

            return jongseongIndex != 0; // 0�̸� ��ħ�� ���� ���
        }

        public static string GetLastString(string foodName)
        {
            char lastChar = foodName[^1];
            string ending = HasFinalConsonant(lastChar) ? "�̾�" : "��";
            return $"{foodName}{ending}";
        }
    }
}