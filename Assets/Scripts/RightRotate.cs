using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightRotate : MonoBehaviour
{
    public GameObject Sphere6;//»úÐµ±ÛÄ©¶Ë
    public RootMotion.FinalIK.CCDIK ccdik;

    private void Update()
    {
        //Jaw.transform.Rotate(0, 0, 45 * Time.deltaTime);
    }
    public void Rotate()
    {
        Debug.Log("Ë³Ê±ÕëÐý×ª");
        ccdik.enabled = false;
        //Rotation = Jaw.transform.localRotation;
        //Rotation.z += 45;
        //Jaw.transform.localRotation = Rotation;
        //Sphere6.transform.rotation *= Quaternion.Euler(0, 30, 0);
        //Sphere6.transform.rotation *= Quaternion.AngleAxis(30, Vector3.up);

        Sphere6.transform.rotation *= Quaternion.Euler(0, -30, 0);

        //ccdik.enabled = true;
    }
}
