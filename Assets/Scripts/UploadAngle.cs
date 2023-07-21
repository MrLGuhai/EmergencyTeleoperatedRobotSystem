using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class UploadAngle : MonoBehaviour
{
    public TextMeshPro textMeshPro1;
    public TextMeshPro textMeshPro2;
    public TextMeshPro textMeshPro3;
    public TextMeshPro textMeshPro4;
    public TextMeshPro textMeshPro5;
    public TextMeshPro textMeshPro6;

    public GameObject[] axles;//存储带有axles标签的物体
    private int[] angles;//存储机械臂角度

    private int[] prevAngle;//存储上一次的角度

    private float x1, x2, x3, x4, x5, x6;


    void Start()
    {
       

        // 初始化角度信息数组
        angles = new int[axles.Length];
        prevAngle = new int[axles.Length];

        x1 = x2 = x3 = x4 = x5 = x6 = 0;
    }
    public Byte[] GetAngleData()
    {

        Vector3 angle1 = axles[0].transform.localRotation.eulerAngles;
        angles[0] = (int)(angle1.z);
        if (angle1.z > 180)
        {
            angles[0] -= 360;
        }
        angles[0] = 0-angles[0];

        x2 = Vector3.SignedAngle(axles[0].transform.up, axles[1].transform.up, axles[0].transform.right);
        angles[1] = (int)x2;
        angles[1] = 0 - angles[1];
        x3 = Vector3.SignedAngle(axles[1].transform.up, axles[2].transform.up, axles[1].transform.right);
        angles[2] = (int)x3;
        angles[2] = 0 - angles[2];
        x4 = Vector3.SignedAngle(axles[2].transform.up, axles[3].transform.up, axles[2].transform.right);
        angles[3] = (int)x4;
        angles[3] = 0 - angles[3];


        Vector3 angle = axles[4].transform.localRotation.eulerAngles;
        angles[4] = (int)(angle.z);
        if (angle.z > 180)
        {
            angles[4] -= 360;
        }
        angles[4] = 0 - angles[4];

        x6 = Vector3.SignedAngle(axles[4].transform.right, axles[5].transform.right, -axles[4].transform.forward);
        angles[5] = (int)x6;
        angles[5] = 0 - angles[5];

        textMeshPro1.text = axles[0].name + " angle: " + angles[0].ToString();
        textMeshPro2.text = axles[1].name + " angle: " + angles[1].ToString();
        textMeshPro3.text = axles[2].name + " angle: " + angles[2].ToString();
        textMeshPro4.text = axles[3].name + " angle: " + angles[3].ToString();
        textMeshPro5.text = axles[4].name + " angle: " + angles[4].ToString();
        textMeshPro6.text = axles[5].name + " angle: " + angles[5].ToString();


        Int32[] temp = new Int32[6];

        temp[0] = (int)(angles[0] * 100);
        temp[1] = (int)(angles[1] * 100);
        temp[2] = (int)(angles[2] * 100);
        temp[3] = (int)(angles[3] * 100);
        temp[4] = (int)(angles[4] * 100);
        temp[5] = (int)(angles[5] * 100);
        //?? 100 ????? int????????????? ??λ????λ

        //FE FE 0F 22 00 00 00 00 00 00 00 00 00 00 00 00 1E FA
        //0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17

        Byte[] commDatabuff = new Byte[18];//存储指令的数据部分
        commDatabuff[0] = 0xFE;//
        commDatabuff[1] = 0xFE;//
        commDatabuff[2] = 0x0F;//
        commDatabuff[3] = 0x22;//

        int n = 3;
        //char[] databuff = new char[18];
        int angletemp = 0;
        for (int i = 1; i <= 6; i++)//获取输入框中的舵机角度值，并填充到commDatabuff数组中
        {
            switch (i)
            {
                case 1: angletemp = getAngleNum(angles[0]); break;
                case 2: angletemp = getAngleNum(angles[1]); break;
                case 3: angletemp = getAngleNum(angles[2]); break;
                case 4: angletemp = getAngleNum(angles[3]); break;
                case 5: angletemp = getAngleNum(angles[4]); break;
                case 6: angletemp = getAngleNum(angles[5]); break;
                default: break;
            }
            commDatabuff[i + n] = (Byte)(angletemp / 256);
            commDatabuff[i + n + 1] = (Byte)(angletemp % 256);
            n++;
        }

        commDatabuff[16] = 0x32;//
        commDatabuff[17] = 0xFA;//

        for (int i = 0; i < angles.Length; i++)
        {
            prevAngle[i] = angles[i];
        }
        return commDatabuff;
    }

    private int getAngleNum(int ret)
    {
        if (ret <= -155)//其实可以到160
        {
            ret = -155;
        }
        else if (ret >= 155)
        {
            ret = 155;
        }

        ret *= 100;
        if (ret < 0)
        {
            ret = ret + 65535;
        }

        return ret;
    }
}
