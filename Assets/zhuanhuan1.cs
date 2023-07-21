using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class zhuanhuan1 : MonoBehaviour
{
    float countTime = 0f;
    public GameObject anniu1;
    public GameObject anniu2;
    public GameObject anniu3;
    public GameObject anniu4;

    int count = -1;
    string str;
    string er;
    string san;

    public Text ShowText;

    private string filePath;

    private void Start()
    {

        /*anniu1.SetActive(false);
        anniu2.SetActive(false);
        anniu3.SetActive(false);
        anniu4.SetActive(false);*/

        // 创建文件路径
        filePath = Path.Combine(Application.persistentDataPath, "timer.txt");

        // 如果文件不存在，则创建一个新文件
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }


    public void button11()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(true);
        anniu2.SetActive(false);
        anniu3.SetActive(false);
        anniu4.SetActive(false);
    }

    public void button22()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(false);
        anniu2.SetActive(true);
        anniu3.SetActive(false);
        anniu4.SetActive(false);
    }

    public void button33()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(false);
        anniu2.SetActive(false);
        anniu3.SetActive(true);
        anniu4.SetActive(false);
    }

    public void button44()
    {
        //NearMenu3x1.SetActive(false);
        anniu1.SetActive(false);
        anniu2.SetActive(false);
        anniu3.SetActive(false);
        anniu4.SetActive(true);

        xianshi();


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
                str = "·用户admin于" + ziti + "登录本系统";
            }
            if (x == 1)
            {
                er = lines[x - 1];
                str = "·用户admin于" + ziti + "登录本系统" + ShowText.text.Replace("\\n", "\n") + "·用户admin于" + er + "登录本系统";
            }
            if (x >= 2)
            {
                er = lines[x - 1];
                san = lines[x - 2];
                str = "·用户admin于" + ziti + "登录本系统" + ShowText.text.Replace("\\n", "\n") + "·用户admin于" + er + "登录本系统" + ShowText.text.Replace("\\n", "\n") + "·用户admin于" + san + "登录本系统";

            }
        }
        ShowText.text = str;
    }

}