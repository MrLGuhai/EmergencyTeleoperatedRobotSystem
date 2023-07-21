using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClearTextMeshPro : MonoBehaviour
{
    public InputField username, password, confirmPassword;
    public TextMeshProUGUI FeedbackText; // TextMeshPro×é¼þ
    public void LoginClear()
    {
        username.text = "";
        password.text = "";
        FeedbackText.text = "";
    }

    public void SignupClear()
    {
        username.text = "";
        password.text = "";
        confirmPassword.text = "";
        FeedbackText.text = "";
    }

}
