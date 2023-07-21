//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class loginclass : MonoBehaviour
//{
//    //进入前变量
//    public InputField username, password;
//    //public TextMeshProUGUI reminderText;
//    public int errorsNum;
//    //public Button loginButton;
//    //public GameObject hallSetUI, loginUI;
//    //进入后变量
//    public static string myUsername;

//    public void Register()
//    {
//        if (PlayerPrefs.GetString(username.text) == "")
//        {
//            PlayerPrefs.SetString(username.text, username.text);
//            PlayerPrefs.SetString(username.text + "password", password.text);
//            Debug.Log("注册成功!");
//            //reminderText.text = "注册成功！";
//        }
//        else
//        {
//            Debug.Log("用户已存在!");
//            //reminderText.text = "用户已存在";
//        }

//    }
//    //private void Recovery()
//    //{
//    //    loginButton.interactable = true;
//    //}
//    public void Login()
//    {
//        if (PlayerPrefs.GetString(username.text) != "")
//        {
//            if (PlayerPrefs.GetString(username.text + "password") == password.text)
//            {
//                Debug.Log("登陆成功!");
//                //reminderText.text = "登录成功";

//                //myUsername = username.text;
//                //hallSetUI.SetActive(true);
//                //loginUI.SetActive(false);
//                //SceneManager.LoadScene(1);
//            }
//            else
//            {
//                Debug.Log("登陆失败!请检查后重新登陆!");
//                //reminderText.text = "密码错误";
//                //errorsNum++;
//                //if (errorsNum >= 3)
//                //{
//                //    reminderText.text = "连续错误3次，请30秒后再试！";
//                //    loginButton.interactable = false;
//                //    Invoke("Recovery", 5);
//                //    errorsNum = 0;
//                //}
//            }
//        }
//        else
//        {
//            //reminderText.text = "账号不存在";
//        }
//    }
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;


public class loginclass : MonoBehaviour
{
    public static string LoginingUser;
    //进入前变量
    public InputField username, password,confirmPassword;
    //public int errorsNum;
    //进入后变量
    public static string myUsername;

    public SetMenuUnActiveSceneActive Menu;
    private string filePath;
    public TextMeshProUGUI FeedbackText; // TextMeshPro组件


    private void Start()
    {
        // 创建文件路径
        filePath = Path.Combine(Application.persistentDataPath, "timer.txt");

        // 如果文件不存在，则创建一个新文件
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    /*public void Register()
    {
        if (username.text == "")
        {
            Debug.Log("用户名不能为空!");
            FeedbackText.text = "用户名不能为空!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        if (password.text == "")
        {
            Debug.Log("密码不能为空!");
            FeedbackText.text = "密码不能为空!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        if (confirmPassword.text == "")
        {
            FeedbackText.text = "确认密码不能为空!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        if (password.text != confirmPassword.text)
        {
            FeedbackText.text = "两次输入密码不一致!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        // 读取文件中的数据
        string[] lines = File.ReadAllLines(filePath);

        // 判断用户名是否已经存在
        foreach (string line in lines)
        {
            if (line.Split(':')[0] == username.text)
            {
                Debug.Log("用户已存在!");
                FeedbackText.text = "用户已存在!";
                username.text = "";
                password.text = "";
                confirmPassword.text = "";
                return;
            }
        }

        // 将用户名和密码写入文件
        string userInfo = username.text + ":" + password.text + Environment.NewLine;
        File.AppendAllText(filePath, userInfo);
        Debug.Log("注册成功!");
        //FeedbackText.text = "注册成功!";//不好控制注册后跳转登录界面后点返回到主界面再注册时，注册页面的反馈文本框没清除的问题
        LoginingUser = username.text;// 记录当前登录的用户名
        username.text = "";
        password.text = "";
        confirmPassword.text = "";
        Menu.SetActivateObject();
    }*/

    public void Login()
    {
        if (username.text == ""|| password.text == "")
        {
            Debug.Log("用户名不能为空!");
            FeedbackText.text = "用户名或密码不能为空!";
            username.text = "";
            password.text = "";
            return;
        }
        if(username.text == "admin" && password.text == "123")
        {
            string Time = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + string.Format("{0:D2}:{1:D2}", DateTime.Now.Hour, DateTime.Now.Minute);
            string userInfo = Time + Environment.NewLine;
            File.AppendAllText(filePath, userInfo);
            Menu.SetActivateObject();
        }

        // 读取文件中的数据
            /*string[] lines = File.ReadAllLines(filePath);

            // 查找匹配的用户名和密码
            foreach (string line in lines)
            {
                string[] userInfo = line.Split(':');
                if (userInfo[0] == username.text && userInfo[1] == password.text)
                {
                    Debug.Log("登录成功!");
                    FeedbackText.text = "登录成功!";
                    LoginingUser = username.text;// 记录当前登录的用户名
                    username.text = "";
                    password.text = "";
                    Menu.SetActivateObject();
                    return;
                }
            }*/

            /*Debug.Log("登录失败!请检查后重新登陆!");
            FeedbackText.text = "登录失败!请检查后重新登录!";
            username.text = "";
            password.text = "";*/
    }

    public static string GetCurrentUsername()
    {
        return LoginingUser;
    }
}

