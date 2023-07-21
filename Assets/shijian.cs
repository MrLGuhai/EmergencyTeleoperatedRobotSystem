using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shijian : MonoBehaviour
{

    float countTime = 0f;
    public GameObject qian;
    public GameObject hou;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
        if (countTime > 3.0f)
        {
            qian.SetActive(false);
            hou.SetActive(true);

        }
    }

}
