using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInfoState : MonoBehaviour
{
    public GameObject StatusBar;
    private bool state;
    private void Start()
    {
        state = true;
    }

    public void updateState()
    {
        if (state == true)
        {
            state = false;
        }
        else
        {
            state = true;
        }
        StatusBar.SetActive(state);
    }
}
