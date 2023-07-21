using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLocation : MonoBehaviour
{
    public OriginalTransform originalTransform;
    private Vector3 oldRotaion;    
    private Vector3 newPosition;
    private Vector3 newRotaion;

    public GameObject car;

    private float OriginX;//ԭʼλ�õ�X
    private float OriginZ;//ԭʼλ�õ�Z
    private float NewX;//��λ�õ�X
    private float NewZ;//��λ�õ�Z

    private float OriginRotationY;//ԭʼλ�õ�Y�����ϽǶ�
    private float NewRotationY;//��λ�õ�Y�����ϽǶ�

    private float displacementX;//С����X�����ϵ�λ��
    private float displacementZ;//С����Z�����ϵ�λ��

    private float speed = 1.8f;//�������´���speed
    private float TimeX;//X�����ƶ���ʱ��
    private float TimeZ;//Z�����ƶ���ʱ��
    private int EndAngle;//���С��Ҫ��ת�ĽǶ�
    public List<float> arrList = new List<float>();//�������еļ���
    private int OldToword;//0��ʾ����x��1��ʾ����-x��2��ʾ����z��3��ʾ����-z
    private int NewToword;//0��ʾ����x��1��ʾ����-x��2��ʾ����z��3��ʾ����-z
    private int EndToward;//�ƶ�����ʱ�ĳ���0��ʾ����x��1��ʾ����-x��2��ʾ����z��3��ʾ����-z

    private int TotalAngel;//ȫ����ת�ĽǶȺ�

    public bool buttonClickable = true;//��ť�Ƿ�ɵ��
    private float waitTime = 3f;

    public void Start()
    {
        oldRotaion = car.transform.rotation.eulerAngles;
        GetOldToward();
    }

    public UpdateLocation()
    {
        // car = GameObject.Find("bottom_car_cube");
        // ��ģ�GameObject.Find() �����ڹ��캯����ʹ�ã�������Unity��Start()��Awake()�Ⱥ�����ʹ��
    }
    IEnumerator ResetButtonClickable()//Э�̣����ڿ��Ʊ���ȴ�waitTime�������ٴΰ��°�ť
    {
        yield return new WaitForSeconds(waitTime); // �ȴ�3��
        buttonClickable = true;
        // ��ԭ��ť����ۣ����罫��ť����ɫ��ԭΪĬ����ɫ
    }

    public List<float> UpdatePosition()
    {
        if (buttonClickable)
        {
            buttonClickable = false;

            arrList.Clear();//��ռ����е�����Ԫ��
            TotalAngel = 0;//��ʼ���ǶȺ�
            EndToward = OldToword;//ÿ���ƶ�ǰEndToword��ԭʼ������һ����
            //Debug.Log("�ϴ�ǰԭʼλ��:" + originalTransform.OriginalPositions);
            //Debug.Log("oldRotation.y:" + oldRotaion.y);
            //Debug.Log("С����ԭʼ����Ϊ:" + OldToword);
            newPosition = car.transform.position;
            newRotaion = car.transform.rotation.eulerAngles;
            //Debug.Log("newRotation.y:" + newRotaion.y);
            //Debug.Log("��ǰλ��:" + newPosition);
           
            OriginX = originalTransform.OriginalPositions.x;
            OriginZ = originalTransform.OriginalPositions.z;
            NewX = newPosition.x;
            NewZ = newPosition.z;

            OriginRotationY = originalTransform.OriginalRotations.y;
            NewRotationY = newRotaion.y;

            displacementX = NewX - OriginX;//����X�����ϵ�λ��
            displacementZ = NewZ - OriginZ;//����Z�����ϵ�λ��
                                           //angleDifferenceY = (NewRotationY - OriginRotationY);//����Y�����ϵ���ת�ǶȲ�ֵ

            //Debug.Log("OriginX:" + OriginX);
            //Debug.Log("OriginZ:" + OriginZ);
            //Debug.Log("NewX:" + NewX);
            //Debug.Log("NewZ:" + NewZ);
            //Debug.Log("displacementX:" + displacementX);
            //Debug.Log("displacementZ:" + displacementZ);

            //�������´����ٶ�ֵ��speed......

            if (OldToword == 0)//���С��������X
            {
                TowardX();
            }
            else if (OldToword == 1)//���С��������-x
            {
                Toward_X();
            }
            else if (OldToword == 2)//���С��������z
            {
                TowardZ();
            }
            else if (OldToword == 3)//���С��������-z
            {
                Toward_Z();
            }

            GetToward();//��ȡ����С���ĳ���
            //Debug.Log("����С����ǰ����Ϊ:" + NewToword);
            if (EndToward != NewToword)//�����ƶ�ʱ����������С������һ��
            {
                if (arrList.Count == 0)
                {
                    arrList.Add(0);//���ûλ�ƣ������򲻶ԣ�����ת
                }
                UpdateToward();//������һ�������Ƕ�
                TotalAngel += EndAngle;
            }
            OldToword = NewToword;//��ԭʼ��������Ϊ��ǰ����
            //Debug.Log("��������:");
            //Debug.Log("���ϳ���Ϊ" + arrList.Count);
            for (int i = 0; i < arrList.Count; i++)
            {
                Debug.Log(arrList[i]);
            }

            //Debug.Log("�ϴ���ԭʼλ��:" + originalTransform.OriginalPositions);

            //�ϴ��������е�������......
            StartCoroutine(ResetButtonClickable());//����Э��
        }
        return arrList;
    }

    private void UpdateToward()
    {
        if (EndToward == 0)//����ʱ����X
        {
            if (NewToword == 1)//�³���Ϊ-X
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת180�ȣ�������X����Ϊ-X");
            }
            else if (NewToword == 2)//�³���ΪZ
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת90�ȣ�������X����ΪZ");
            }
            else if (NewToword == 3)//�³���Ϊ-Z
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת90�ȣ�������X����Ϊ-Z");
            }
        }
        else if (EndToward == 1)//����ʱ����Ϊ-X
        {
            if (NewToword == 0)//�³���ΪX
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת180�ȣ�������-X����ΪX");
            }
            else if (NewToword == 2)//�³���ΪZ
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
               // Debug.Log("С��������ת90�ȣ�������-X����ΪZ");
            }
            else if (NewToword == 3)//�³���Ϊ-Z
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
               // Debug.Log("С��������ת90�ȣ�������-X����Ϊ-Z");
            }
        }
        else if (EndToward == 2)//����ʱ����ΪZ
        {
            if (NewToword == 0)//�³���ΪX
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת90�ȣ�������Z����ΪX");
            }
            else if (NewToword == 1)//�³���Ϊ-X
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
               // Debug.Log("С��������ת90�ȣ�������Z����Ϊ-X");
            }
            else if (NewToword == 3)//�³���Ϊ-Z
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
              // Debug.Log("С��������ת180�ȣ�������Z����Ϊ-Z");
            }
        }
        else if (EndToward == 3)//����ʱ����Ϊ-Z
        {
            if (NewToword == 0)//�³���ΪX
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
               // Debug.Log("С��������ת90�ȣ�������-Z����ΪX");
            }
            else if (NewToword == 1)//�³���Ϊ-X
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת90�ȣ�������-Z����Ϊ-X");
            }
            else if (NewToword == 2)//�³���ΪZ
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
                //Debug.Log("С��������ת180�ȣ�������-Z����ΪZ");
            }
        }
    }

    private void GetOldToward()
    {
        if ((oldRotaion.y % 360 >= 315 && oldRotaion.y % 360 <= 360) || (oldRotaion.y  % 360 >= 0 && oldRotaion.y  % 360 < 45)) //���y�᷽��Ϊ315-45֮�䣬��Ϊ����Z����
        {
           OldToword = 0;
        }
        else if (oldRotaion.y % 360 >= 45 && oldRotaion.y % 360 < 135) //���y�᷽��Ϊ45-135֮�䣬��Ϊ����X����
        {
            OldToword = 3;
        }
        else if (oldRotaion.y + 180 % 360 >= 135 && oldRotaion.y  % 360 < 225)//���y�᷽��Ϊ135-225֮�䣬��Ϊ����-Z����
        {
            OldToword = 1;
        }
        else if (oldRotaion.y % 360 >= 225 && oldRotaion.y  % 360 < 315) //���y�᷽��Ϊ225-315֮�䣬��Ϊ����-Z����
        {
            OldToword = 2;
        }
    }
    private void GetToward()
    {
        if ((newRotaion.y  % 360 >= 315 && newRotaion.y % 360 <= 360) || (newRotaion.y  % 360 >= 0 && newRotaion.y % 360 < 45)) //���y�᷽��Ϊ315-45֮�䣬��Ϊ����-x����
        {
            NewToword = 0;
        }
        else if (newRotaion.y % 360 >= 45 && newRotaion.y  % 360 < 135) //���y�᷽��Ϊ45-135֮�䣬��Ϊ����z����
        {
            NewToword = 3;
        }
        else if (newRotaion.y  % 360 >= 135 && newRotaion.y  % 360 < 225)//���y�᷽��Ϊ135-225֮�䣬��Ϊ����x����
        {
            NewToword = 1;
        }
        else if (newRotaion.y  % 360 >= 225 && newRotaion.y  % 360 < 315) //���y�᷽��Ϊ225-315֮�䣬��Ϊ����-z����
        {
            NewToword = 2;
        }

    }

    private void TowardX()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            //Ҫ��ʵ�ʵľ����ֵת��һ�� 
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С��������ת��Z����������ת��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С��������ת��Z����������ת��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           //Debug.Log("С������ǰ��X����������ת��Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
           // Debug.Log("С������ǰ��X����������ת��Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeZ = displacementZ*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//С������ת
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           // Debug.Log("С��������ת��Ȼ����Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeZ = displacementZ*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//С������ת
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
           // Debug.Log("С��������ת��Ȼ����Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = 0;
            //���ɶ�������
            arrList.Add(0);//С������ת
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С��������ת��Z����Ȼ��������ת��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(1);//С����ֱ��
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
           // Debug.Log("С������ǰ��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        
    }

    private void Toward_X()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           // Debug.Log("С������ǰ��X����������ת��Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
           // Debug.Log("С������ǰ��X����������ת��Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
           // Debug.Log("С��������ת��Z����������ת��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
          //  Debug.Log("С��������ת��Z����������ת��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeZ = displacementZ*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += 90;
            arrList.Add(90);//�ǶȻ������
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           // Debug.Log("С��������ת������Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeZ = displacementZ*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
           // Debug.Log("С��������ת������Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(1);//Ҫ��ֱ��
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С������ǰ��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = 0;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
           // Debug.Log("С��������ת��Z����������ת��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
       
    }

    private void TowardZ()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += 90;
            arrList.Add(90);//�ǶȻ������
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           // Debug.Log("С��������ת��X����������ת��Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С������ǰ��Z����������ת��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           // Debug.Log("С��������ת��X����������ת��Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
           // Debug.Log("С������ǰ��Z����������ת��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeX = 0;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
           // Debug.Log("С��������ת��X����������ת��Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//Ҫ��ֱ��
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
            //Debug.Log("С������ǰZ���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += 90;
            arrList.Add(90);//�ǶȻ������
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С��������ת������ǰ��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
           // Debug.Log("С��������ת������ǰ��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
       
    }

    private void Toward_Z()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С������ǰ��Z����������ת��X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += -90;
            arrList.Add(-90);//�ǶȻ������
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
           // Debug.Log("С��������ת��X����������ת��Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
            //Debug.Log("С������ǰ��Z����������ת��X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//Ҫ����ת
            TotalAngel += 90;
            arrList.Add(90);//�ǶȻ������
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
            //Debug.Log("С��������ת��X����������ת��Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //ȡ����ֵ
            displacementZ = 0 - displacementZ;
            TimeZ = displacementZ*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(1);//С����ǰ��
            arrList.Add(TimeZ);
            EndToward = 3;//�ƶ���С������Ϊ-Z
            //Debug.Log("С������ǰ��Z���������-Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeX = 0;//���뻻�ɶ�������
            TimeZ = displacementZ*100;
            //���ɶ�������
            arrList.Add(0);//С������ת
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//�ƶ���С������ΪZ
            //Debug.Log("С��������ת��X����������ת��Z���������Z");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //ȡ����ֵ
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//С������ת
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 1;//�ƶ���С������Ϊ-X
           // Debug.Log("С��������ת������X���������-X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//���뻻�ɶ�������
            //���ɶ�������
            arrList.Add(0);//С������ת
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 0;//�ƶ���С������ΪX
            //Debug.Log("С��������ת������X���������X");
            //����ԭʼλ��ΪС����ǰλ��
            originalTransform.OriginalPositions = newPosition;
        }
        
    }

    public int GetSceneUpdateAngle()
    { 
        return TotalAngel;
    }
}
