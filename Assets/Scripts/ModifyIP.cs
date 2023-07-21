using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModifyIP : MonoBehaviour
{
    public GameObject modifyMenu;
    public InputField input;
    private string newIP;
   public void modify()
    {
        newIP = input.text;
        Debug.Log("��IP:" + Config.IP);
        Config.IP = newIP;
        Debug.Log("��IP:" + Config.IP);
        modifyMenu.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
}
