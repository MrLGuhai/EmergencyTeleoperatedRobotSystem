using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintTextInfo : MonoBehaviour
{
    public TextMeshPro Text;

    public void PrinfText()
    {
        Debug.Log(Text.text);
    }
}
