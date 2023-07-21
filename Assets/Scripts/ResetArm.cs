using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetArm : MonoBehaviour
{
    public GameObject Sphere1;
    public GameObject Sphere2;
    public GameObject Sphere3;
    public GameObject Sphere4;
    public GameObject Sphere5;
    public GameObject Sphere6;
    public GameObject ClampingJaw;
    public GameObject Target;
    public MoveWithinCircle moveWithinCircle;//重置时让限制target的脚本不启用

    public RootMotion.FinalIK.CCDIK ccdik;

    // private Quaternion[] rotations;//用于记录机械臂的原始角度
    private Vector3 targetPosition;//用于记录target相对于CarCar的位置偏移量
    private Quaternion targetRotation;  //用于记录target相对于CarCar的旋转角度偏移量
    void Start()
    {
        //记录target相对于车的位置和角度
        // targetPosition = Target.transform.position - GameObject.FindGameObjectWithTag("CarCar").transform.position;
        targetPosition = Target.transform.localPosition;
        targetRotation = Target.transform.localRotation * Quaternion.Inverse(GameObject.FindGameObjectWithTag("CarCar").transform.localRotation);
    }
    public void Reset()
    {
        ccdik.enabled = true;
        //每次调用Reset方法重置机械臂时，让机械臂的角度为原始值，target的位置和角度为原始值
        moveWithinCircle.enabled = false;

        Sphere1.transform.localEulerAngles = new Vector3(0, 0, 0);
        Sphere2.transform.localEulerAngles = new Vector3(0, 0, 0);
        Sphere3.transform.localEulerAngles = new Vector3(0, 0, 0);
        Sphere4.transform.localEulerAngles = new Vector3(0, 0, 0);
        Sphere5.transform.localEulerAngles = new Vector3(0, 0, 0);
        Sphere6.transform.localEulerAngles = new Vector3(0, 0, 0);
        ClampingJaw.transform.localEulerAngles = new Vector3(0, 0, 180);

        // Target.transform.position= targetPosition + GameObject.FindGameObjectWithTag("CarCar").transform.position;
        Target.transform.localPosition = targetPosition;
        Target.transform.localRotation = targetRotation*GameObject.FindGameObjectWithTag("CarCar").transform.localRotation;
        // Target.transform.localRotation = GameObject.FindGameObjectWithTag("CarCar").transform.localRotation* targetRotation;

        moveWithinCircle.enabled = true;
    }
}
