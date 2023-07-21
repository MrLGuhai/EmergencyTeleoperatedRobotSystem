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

    private float OriginX;//原始位置的X
    private float OriginZ;//原始位置的Z
    private float NewX;//新位置的X
    private float NewZ;//新位置的Z

    private float OriginRotationY;//原始位置的Y方向上角度
    private float NewRotationY;//新位置的Y方向上角度

    private float displacementX;//小车在X方向上的位移
    private float displacementZ;//小车在Z方向上的位移

    private float speed = 1.8f;//服务器下传的speed
    private float TimeX;//X方向移动的时间
    private float TimeZ;//Z方向移动的时间
    private int EndAngle;//最后小车要旋转的角度
    public List<float> arrList = new List<float>();//动作序列的集合
    private int OldToword;//0表示朝向x，1表示朝向-x，2表示朝向z，3表示朝向-z
    private int NewToword;//0表示朝向x，1表示朝向-x，2表示朝向z，3表示朝向-z
    private int EndToward;//移动结束时的朝向，0表示朝向x，1表示朝向-x，2表示朝向z，3表示朝向-z

    private int TotalAngel;//全程旋转的角度和

    public bool buttonClickable = true;//按钮是否可点击
    private float waitTime = 3f;

    public void Start()
    {
        oldRotaion = car.transform.rotation.eulerAngles;
        GetOldToward();
    }

    public UpdateLocation()
    {
        // car = GameObject.Find("bottom_car_cube");
        // 错的！GameObject.Find() 不能在构造函数中使用，必须在Unity的Start()、Awake()等函数中使用
    }
    IEnumerator ResetButtonClickable()//协程，用于控制必须等待waitTime秒后才能再次按下按钮
    {
        yield return new WaitForSeconds(waitTime); // 等待3秒
        buttonClickable = true;
        // 还原按钮的外观，例如将按钮的颜色还原为默认颜色
    }

    public List<float> UpdatePosition()
    {
        if (buttonClickable)
        {
            buttonClickable = false;

            arrList.Clear();//清空集合中的所以元素
            TotalAngel = 0;//初始化角度和
            EndToward = OldToword;//每次移动前EndToword和原始朝向是一样的
            //Debug.Log("上传前原始位置:" + originalTransform.OriginalPositions);
            //Debug.Log("oldRotation.y:" + oldRotaion.y);
            //Debug.Log("小车的原始朝向为:" + OldToword);
            newPosition = car.transform.position;
            newRotaion = car.transform.rotation.eulerAngles;
            //Debug.Log("newRotation.y:" + newRotaion.y);
            //Debug.Log("当前位置:" + newPosition);
           
            OriginX = originalTransform.OriginalPositions.x;
            OriginZ = originalTransform.OriginalPositions.z;
            NewX = newPosition.x;
            NewZ = newPosition.z;

            OriginRotationY = originalTransform.OriginalRotations.y;
            NewRotationY = newRotaion.y;

            displacementX = NewX - OriginX;//这里X方向上的位移
            displacementZ = NewZ - OriginZ;//计算Z方向上的位移
                                           //angleDifferenceY = (NewRotationY - OriginRotationY);//计算Y方向上的旋转角度差值

            //Debug.Log("OriginX:" + OriginX);
            //Debug.Log("OriginZ:" + OriginZ);
            //Debug.Log("NewX:" + NewX);
            //Debug.Log("NewZ:" + NewZ);
            //Debug.Log("displacementX:" + displacementX);
            //Debug.Log("displacementZ:" + displacementZ);

            //服务器下传的速度值给speed......

            if (OldToword == 0)//如果小车朝向是X
            {
                TowardX();
            }
            else if (OldToword == 1)//如果小车朝向是-x
            {
                Toward_X();
            }
            else if (OldToword == 2)//如果小车朝向是z
            {
                TowardZ();
            }
            else if (OldToword == 3)//如果小车朝向是-z
            {
                Toward_Z();
            }

            GetToward();//获取虚拟小车的朝向
            //Debug.Log("虚拟小车当前朝向为:" + NewToword);
            if (EndToward != NewToword)//结束移动时朝向与虚拟小车朝向不一致
            {
                if (arrList.Count == 0)
                {
                    arrList.Add(0);//如果没位移，但朝向不对，先旋转
                }
                UpdateToward();//添加最后一个矫正角度
                TotalAngel += EndAngle;
            }
            OldToword = NewToword;//将原始朝向设置为当前朝向
            //Debug.Log("动作序列:");
            //Debug.Log("集合长度为" + arrList.Count);
            for (int i = 0; i < arrList.Count; i++)
            {
                Debug.Log(arrList[i]);
            }

            //Debug.Log("上传后原始位置:" + originalTransform.OriginalPositions);

            //上传动作序列到服务器......
            StartCoroutine(ResetButtonClickable());//开启协程
        }
        return arrList;
    }

    private void UpdateToward()
    {
        if (EndToward == 0)//结束时朝向X
        {
            if (NewToword == 1)//新朝向为-X
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
                //Debug.Log("小车向左旋转180度，朝向由X更新为-X");
            }
            else if (NewToword == 2)//新朝向为Z
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
                //Debug.Log("小车向左旋转90度，朝向由X更新为Z");
            }
            else if (NewToword == 3)//新朝向为-Z
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
                //Debug.Log("小车向右旋转90度，朝向由X更新为-Z");
            }
        }
        else if (EndToward == 1)//结束时朝向为-X
        {
            if (NewToword == 0)//新朝向为X
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
                //Debug.Log("小车向左旋转180度，朝向由-X更新为X");
            }
            else if (NewToword == 2)//新朝向为Z
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
               // Debug.Log("小车向右旋转90度，朝向由-X更新为Z");
            }
            else if (NewToword == 3)//新朝向为-Z
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
               // Debug.Log("小车向左旋转90度，朝向由-X更新为-Z");
            }
        }
        else if (EndToward == 2)//结束时朝向为Z
        {
            if (NewToword == 0)//新朝向为X
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
                //Debug.Log("小车向右旋转90度，朝向由Z更新为X");
            }
            else if (NewToword == 1)//新朝向为-X
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
               // Debug.Log("小车向左旋转90度，朝向由Z更新为-X");
            }
            else if (NewToword == 3)//新朝向为-Z
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
              // Debug.Log("小车向左旋转180度，朝向由Z更新为-Z");
            }
        }
        else if (EndToward == 3)//结束时朝向为-Z
        {
            if (NewToword == 0)//新朝向为X
            {
                EndAngle = 90;
                arrList.Add(EndAngle);
               // Debug.Log("小车向左旋转90度，朝向由-Z更新为X");
            }
            else if (NewToword == 1)//新朝向为-X
            {
                EndAngle = -90;
                arrList.Add(EndAngle);
                //Debug.Log("小车向右旋转90度，朝向由-Z更新为-X");
            }
            else if (NewToword == 2)//新朝向为Z
            {
                EndAngle = 180;
                arrList.Add(EndAngle);
                //Debug.Log("小车向左旋转180度，朝向由-Z更新为Z");
            }
        }
    }

    private void GetOldToward()
    {
        if ((oldRotaion.y % 360 >= 315 && oldRotaion.y % 360 <= 360) || (oldRotaion.y  % 360 >= 0 && oldRotaion.y  % 360 < 45)) //如果y轴方向为315-45之间，认为朝向Z方向
        {
           OldToword = 0;
        }
        else if (oldRotaion.y % 360 >= 45 && oldRotaion.y % 360 < 135) //如果y轴方向为45-135之间，认为朝向X方向
        {
            OldToword = 3;
        }
        else if (oldRotaion.y + 180 % 360 >= 135 && oldRotaion.y  % 360 < 225)//如果y轴方向为135-225之间，认为朝向-Z方向
        {
            OldToword = 1;
        }
        else if (oldRotaion.y % 360 >= 225 && oldRotaion.y  % 360 < 315) //如果y轴方向为225-315之间，认为朝向-Z方向
        {
            OldToword = 2;
        }
    }
    private void GetToward()
    {
        if ((newRotaion.y  % 360 >= 315 && newRotaion.y % 360 <= 360) || (newRotaion.y  % 360 >= 0 && newRotaion.y % 360 < 45)) //如果y轴方向为315-45之间，认为朝向-x方向
        {
            NewToword = 0;
        }
        else if (newRotaion.y % 360 >= 45 && newRotaion.y  % 360 < 135) //如果y轴方向为45-135之间，认为朝向z方向
        {
            NewToword = 3;
        }
        else if (newRotaion.y  % 360 >= 135 && newRotaion.y  % 360 < 225)//如果y轴方向为135-225之间，认为朝向x方向
        {
            NewToword = 1;
        }
        else if (newRotaion.y  % 360 >= 225 && newRotaion.y  % 360 < 315) //如果y轴方向为225-315之间，认为朝向-z方向
        {
            NewToword = 2;
        }

    }

    private void TowardX()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            //要按实际的距离比值转换一下 
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向右转走Z方向，再向右转走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向左转走Z方向，再向左转走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           //Debug.Log("小车先向前走X方向，再向右转走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
           // Debug.Log("小车先向前走X方向，再向左转走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeZ = displacementZ*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//小车先旋转
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           // Debug.Log("小车先向右转，然后走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeZ = displacementZ*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//小车先旋转
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
           // Debug.Log("小车先向左转，然后走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = 0;
            //生成动作序列
            arrList.Add(0);//小车先旋转
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向左转走Z方向，然后再向左转走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(1);//小车先直走
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
           // Debug.Log("小车先向前走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        
    }

    private void Toward_X()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           // Debug.Log("小车先向前走X方向，再向左转走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
           // Debug.Log("小车先向前走X方向，再向右转走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
           // Debug.Log("小车先向左转走Z方向，再向左转走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
          //  Debug.Log("小车先向右转走Z方向，再向右转走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeZ = displacementZ*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += 90;
            arrList.Add(90);//角度会有误差
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           // Debug.Log("小车先向左转，再走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeZ = displacementZ*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
           // Debug.Log("小车先向右转，再走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(1);//要先直走
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向前走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = 0;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
           // Debug.Log("小车先向右转走Z方向，再向右转走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
       
    }

    private void TowardZ()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += 90;
            arrList.Add(90);//角度会有误差
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           // Debug.Log("小车先向左转走X方向，再向左转走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向前走Z方向，再向左转走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           // Debug.Log("小车先向右转走X方向，再向右转走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
           // Debug.Log("小车先向前走Z方向，再向右转走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeX = 0;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
           // Debug.Log("小车先向右转走X方向，再向右转走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//要先直走
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
            //Debug.Log("小车先向前Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += 90;
            arrList.Add(90);//角度会有误差
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向左转，再向前走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
           // Debug.Log("小车先向右转，再向前走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
       
    }

    private void Toward_Z()
    {
        if (displacementX < 0 && displacementZ < 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeZ);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向前走Z方向，再向右转走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ > 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += -90;
            arrList.Add(-90);//角度会有误差
            arrList.Add(TimeX);
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
           // Debug.Log("小车先向右转走X方向，再向右转走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeZ);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
            //Debug.Log("小车先向前走Z方向，再向左转走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ > 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//要先旋转
            TotalAngel += 90;
            arrList.Add(90);//角度会有误差
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
            //Debug.Log("小车先向左转走X方向，再向左转走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ < 0)
        {
            //取绝对值
            displacementZ = 0 - displacementZ;
            TimeZ = displacementZ*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(1);//小车先前进
            arrList.Add(TimeZ);
            EndToward = 3;//移动后小车朝向为-Z
            //Debug.Log("小车先向前走Z方向，最后朝向-Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX == 0 && displacementZ > 0)
        {
            TimeX = 0;//距离换成动作序列
            TimeZ = displacementZ*100;
            //生成动作序列
            arrList.Add(0);//小车先旋转
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeZ);
            EndToward = 2;//移动后小车朝向为Z
            //Debug.Log("小车先向左转走X方向，再向左转走Z方向，最后朝向Z");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX < 0 && displacementZ == 0)
        {
            //取绝对值
            displacementX = 0 - displacementX;
            TimeX = displacementX*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//小车先旋转
            TotalAngel += -90;
            arrList.Add(-90);
            arrList.Add(TimeX);
            EndToward = 1;//移动后小车朝向为-X
           // Debug.Log("小车先向右转，再走X方向，最后朝向-X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        else if (displacementX > 0 && displacementZ == 0)
        {
            TimeX = displacementX*100;//距离换成动作序列
            //生成动作序列
            arrList.Add(0);//小车先旋转
            TotalAngel += 90;
            arrList.Add(90);
            arrList.Add(TimeX);
            EndToward = 0;//移动后小车朝向为X
            //Debug.Log("小车先向左转，再走X方向，最后朝向X");
            //更新原始位置为小车当前位置
            originalTransform.OriginalPositions = newPosition;
        }
        
    }

    public int GetSceneUpdateAngle()
    { 
        return TotalAngel;
    }
}
