using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalTransform : MonoBehaviour
{
    public Vector3 OriginalPositions;
    public Vector3 OriginalRotations;
    public GameObject OriginalCar;
    // Start is called before the first frame update
    void Start()
    {
        OriginalPositions = OriginalCar.transform.position;
        OriginalRotations = OriginalCar.transform.rotation.eulerAngles;
        Debug.Log(OriginalPositions);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
