using UnityEngine;
using TMPro;

public class DisplayUsername : MonoBehaviour
{
    public TextMeshProUGUI currentUsernameText; // TextMeshPro组件

    private void OnEnable()
    {
        // 获取当前登陆的用户名并显示在TextMeshPro中
        string currentUsername = loginclass.GetCurrentUsername();
        if (currentUsername != null && currentUsernameText != null)
        {
            currentUsernameText.text = "当前登陆的用户:"+currentUsername;
        }
    }
}
