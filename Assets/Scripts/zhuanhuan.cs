using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class zhuanhuan : MonoBehaviour
{
    float countTime = 0f;
    public GameObject anniu1;
    public GameObject anniu2;
    public GameObject anniu3;
    public GameObject anniu4;
    public GameObject jiqiren;

    int count = -1;
    string str;
    string er;
    string san;

    public Text ShowText;

    private string filePath;
    private bool displayFlag = true;

    private void Start()
    {
        /*anniu1.SetActive(false);
        anniu2.SetActive(false);
        anniu3.SetActive(false);
        anniu4.SetActive(false);*/

        // �����ļ�·��
        filePath = Path.Combine(Application.persistentDataPath, "timer.txt");

        // ����ļ������ڣ��򴴽�һ�����ļ�
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }



    public void button1()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(true);
        anniu2.SetActive(false);
        anniu3.SetActive(false);
        anniu4.SetActive(false);
    }

    public void button2()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(false);
        anniu2.SetActive(true);
        anniu3.SetActive(false);
        anniu4.SetActive(false);
    }

    public void button3()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(false);
        anniu2.SetActive(false);
        anniu3.SetActive(true);
        anniu4.SetActive(false);
    }

    public void button4()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(false);
        anniu2.SetActive(false);
        anniu3.SetActive(false);
        anniu4.SetActive(true);

        xianshi();


    }

    void Update()
    {
        countTime += Time.deltaTime;
        if (countTime > 20.41f)
        {
            jiqiren.SetActive(false);

        }

    }

    public void xianshi()
    {
        count++;

        string[] lines = File.ReadAllLines(filePath);
        int x = lines.Length - 1;

        string ziti = lines[x];


        if (count == 0)
        {
            if (x == 0)
            {
                str = "���û�admin��" + ziti + "��¼��ϵͳ";
            }
            if (x == 1)
            {
                er = lines[x - 1];
                str = "���û�admin��" + ziti + "��¼��ϵͳ" + ShowText.text.Replace("\\n", "\n") + "���û�admin��" + er + "��¼��ϵͳ";
            }
            if (x >= 2)
            {
                er = lines[x - 1];
                san = lines[x - 2];
                str = "���û�admin��" + ziti + "��¼��ϵͳ" + ShowText.text.Replace("\\n", "\n") + "���û�admin��" + er + "��¼��ϵͳ" + ShowText.text.Replace("\\n", "\n") + "���û�admin��" + san + "��¼��ϵͳ";

            }
        }
        ShowText.text = str;
    }

}