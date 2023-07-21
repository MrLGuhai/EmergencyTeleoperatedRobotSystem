using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public float cameraZoom = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollWheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelValue != 0)
        {
            cameraZoom += scrollWheelValue * 0.1f;
            cameraZoom = Mathf.Clamp(cameraZoom, 0.1f, 10.0f);
            Camera.main.orthographicSize = cameraZoom;
        }
    }
}
