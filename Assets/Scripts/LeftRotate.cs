using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRotate : MonoBehaviour
{
    public GameObject Sphere6;//��е��ĩ��
    public RootMotion.FinalIK.CCDIK ccdik;

    public void Rotate()
    {
        Debug.Log("˳ʱ����ת");
        ccdik.enabled = false;

        Sphere6.transform.rotation *= Quaternion.Euler(0, 30, 0);
        //Sphere6.transform.rotation *= Quaternion.AngleAxis(-30, Vector3.up);
        //ccdik.enabled = true;
    }
}
