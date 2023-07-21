using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiaozhuan : MonoBehaviour
{
    public GameObject qian;
    public GameObject hou;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void tiao()
    {
        qian.SetActive(false);
        hou.SetActive(true);
    }
}
