//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class loginclass : MonoBehaviour
//{
//    //����ǰ����
//    public InputField username, password;
//    //public TextMeshProUGUI reminderText;
//    public int errorsNum;
//    //public Button loginButton;
//    //public GameObject hallSetUI, loginUI;
//    //��������
//    public static string myUsername;

//    public void Register()
//    {
//        if (PlayerPrefs.GetString(username.text) == "")
//        {
//            PlayerPrefs.SetString(username.text, username.text);
//            PlayerPrefs.SetString(username.text + "password", password.text);
//            Debug.Log("ע��ɹ�!");
//            //reminderText.text = "ע��ɹ���";
//        }
//        else
//        {
//            Debug.Log("�û��Ѵ���!");
//            //reminderText.text = "�û��Ѵ���";
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
//                Debug.Log("��½�ɹ�!");
//                //reminderText.text = "��¼�ɹ�";

//                //myUsername = username.text;
//                //hallSetUI.SetActive(true);
//                //loginUI.SetActive(false);
//                //SceneManager.LoadScene(1);
//            }
//            else
//            {
//                Debug.Log("��½ʧ��!��������µ�½!");
//                //reminderText.text = "�������";
//                //errorsNum++;
//                //if (errorsNum >= 3)
//                //{
//                //    reminderText.text = "��������3�Σ���30������ԣ�";
//                //    loginButton.interactable = false;
//                //    Invoke("Recovery", 5);
//                //    errorsNum = 0;
//                //}
//            }
//        }
//        else
//        {
//            //reminderText.text = "�˺Ų�����";
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
    //����ǰ����
    public InputField username, password,confirmPassword;
    //public int errorsNum;
    //��������
    public static string myUsername;

    public SetMenuUnActiveSceneActive Menu;
    private string filePath;
    public TextMeshProUGUI FeedbackText; // TextMeshPro���


    private void Start()
    {
        // �����ļ�·��
        filePath = Path.Combine(Application.persistentDataPath, "timer.txt");

        // ����ļ������ڣ��򴴽�һ�����ļ�
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    /*public void Register()
    {
        if (username.text == "")
        {
            Debug.Log("�û�������Ϊ��!");
            FeedbackText.text = "�û�������Ϊ��!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        if (password.text == "")
        {
            Debug.Log("���벻��Ϊ��!");
            FeedbackText.text = "���벻��Ϊ��!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        if (confirmPassword.text == "")
        {
            FeedbackText.text = "ȷ�����벻��Ϊ��!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        if (password.text != confirmPassword.text)
        {
            FeedbackText.text = "�����������벻һ��!";
            username.text = "";
            password.text = "";
            confirmPassword.text = "";
            return;
        }
        // ��ȡ�ļ��е�����
        string[] lines = File.ReadAllLines(filePath);

        // �ж��û����Ƿ��Ѿ�����
        foreach (string line in lines)
        {
            if (line.Split(':')[0] == username.text)
            {
                Debug.Log("�û��Ѵ���!");
                FeedbackText.text = "�û��Ѵ���!";
                username.text = "";
                password.text = "";
                confirmPassword.text = "";
                return;
            }
        }

        // ���û���������д���ļ�
        string userInfo = username.text + ":" + password.text + Environment.NewLine;
        File.AppendAllText(filePath, userInfo);
        Debug.Log("ע��ɹ�!");
        //FeedbackText.text = "ע��ɹ�!";//���ÿ���ע�����ת��¼�����㷵�ص���������ע��ʱ��ע��ҳ��ķ����ı���û���������
        LoginingUser = username.text;// ��¼��ǰ��¼���û���
        username.text = "";
        password.text = "";
        confirmPassword.text = "";
        Menu.SetActivateObject();
    }*/

    public void Login()
    {
        if (username.text == ""|| password.text == "")
        {
            Debug.Log("�û�������Ϊ��!");
            FeedbackText.text = "�û��������벻��Ϊ��!";
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

        // ��ȡ�ļ��е�����
            /*string[] lines = File.ReadAllLines(filePath);

            // ����ƥ����û���������
            foreach (string line in lines)
            {
                string[] userInfo = line.Split(':');
                if (userInfo[0] == username.text && userInfo[1] == password.text)
                {
                    Debug.Log("��¼�ɹ�!");
                    FeedbackText.text = "��¼�ɹ�!";
                    LoginingUser = username.text;// ��¼��ǰ��¼���û���
                    username.text = "";
                    password.text = "";
                    Menu.SetActivateObject();
                    return;
                }
            }*/

            /*Debug.Log("��¼ʧ��!��������µ�½!");
            FeedbackText.text = "��¼ʧ��!��������µ�¼!";
            username.text = "";
            password.text = "";*/
    }

    public static string GetCurrentUsername()
    {
        return LoginingUser;
    }
}

