using UnityEngine;
using TMPro;

public class DisplayUsername : MonoBehaviour
{
    public TextMeshProUGUI currentUsernameText; // TextMeshPro���

    private void OnEnable()
    {
        // ��ȡ��ǰ��½���û�������ʾ��TextMeshPro��
        string currentUsername = loginclass.GetCurrentUsername();
        if (currentUsername != null && currentUsernameText != null)
        {
            currentUsernameText.text = "��ǰ��½���û�:"+currentUsername;
        }
    }
}
