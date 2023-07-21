using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDevice : MonoBehaviour
{
    public LimitMovement limit;
    public GameObject panel;

    public void ClickEnter()
    {
        limit.limitMovement = false;
        panel.SetActive(false);
    }
}
