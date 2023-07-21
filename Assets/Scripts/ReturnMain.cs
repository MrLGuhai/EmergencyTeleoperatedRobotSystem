using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMain : MonoBehaviour
{
    public GoToScene GoToScene;
   public void Return()
    {
        Config.LoginActive = false;
        GoToScene.LoadScene("MenuScene");
    }
}
