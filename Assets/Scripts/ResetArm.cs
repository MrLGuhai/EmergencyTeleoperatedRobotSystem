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
    public MoveWithinCircle moveWithinCircle;//����ʱ������target�Ľű�������

    public RootMotion.FinalIK.CCDIK ccdik;

    // private Quaternion[] rotations;//���ڼ�¼��е�۵�ԭʼ�Ƕ�
    private Vector3 targetPosition;//���ڼ�¼target�����CarCar��λ��ƫ����
    private Quaternion targetRotation;  //���ڼ�¼target�����CarCar����ת�Ƕ�ƫ����
    void Start()
    {
        //��¼target����ڳ���λ�úͽǶ�
        // targetPosition = Target.transform.position - GameObject.FindGameObjectWithTag("CarCar").transform.position;
        targetPosition = Target.transform.localPosition;
        targetRotation = Target.transform.localRotation * Quaternion.Inverse(GameObject.FindGameObjectWithTag("CarCar").transform.localRotation);
    }
    public void Reset()
    {
        ccdik.enabled = true;
        //ÿ�ε���Reset�������û�е��ʱ���û�е�۵ĽǶ�Ϊԭʼֵ��target��λ�úͽǶ�Ϊԭʼֵ
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
