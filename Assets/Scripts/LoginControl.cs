using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginControl : MonoBehaviour
{
    public GameObject Login;
    public GameObject Main;
    // Start is called before the first frame update
    void Start()
    {
        if (Config.LoginActive)
        {
            Login.SetActive(true);
        }
        else
        {
            Login.SetActive(false);
            Main.SetActive(true);
        }
    }

}
