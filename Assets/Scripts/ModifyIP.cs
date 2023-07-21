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
        Debug.Log("¾ÉIP:" + Config.IP);
        Config.IP = newIP;
        Debug.Log("ÐÂIP:" + Config.IP);
        modifyMenu.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
}
