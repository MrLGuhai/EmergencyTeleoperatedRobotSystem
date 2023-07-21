using UnityEngine;
using System.Linq;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;



using System.IO;
//using Google.Protobuf;

public class network : MonoBehaviour
{


    private String myRobotIP = null;//存储下位机IP
    private int myRobotPort = 0;//存储下位机端口号
    private TcpClient mySocket = null;//存储Socket通讯
    private NetworkStream networkStream = null;

    //private Byte currPackNum = 0;//当前的包序号,下一次发送要使用的包序号。

    public TcpClient MySocket { get => mySocket; set => mySocket = value; }
    public string MyRobotIP { get => myRobotIP; set => myRobotIP = value; }
    public int MyRobotPort { get => myRobotPort; set => myRobotPort = value; }



    byte[] data;



    // Start is called before the first frame update
    void Start()
    {

        if (initConfig("192.168.1.102", 5001) == 0)
        {
            Debug.Log("断开");
            Debug.Log("Mesg:" + "已连接。");
        }
        else
        {
            Debug.Log("连接");
            Debug.Log("Mesg:" + "连接下位机失败。");
        }








        












    }

    // Update is called once per frame
    void Update()
    {
        
    }





    /*
         *@function:配置与下位机通信的IP和端口号，每次连接都需要重新初始化。
         *调用该函数后，会启动TCP连接，并发送START帧并验证。
         *@param:下位机的IP和Port。
         *@return：通信建立成功返回0,TCP连接失败返回-1，START帧无响应返回-2。
         */
    public int initConfig(String ip, int port)
    {

        if (MySocket != null)
        {
            MySocket.Close();
        }
        MySocket = new TcpClient();

        Debug.Log("aaaa");
        MySocket.ConnectAsync(ip, port);//异步创建TCP连接，非阻塞
        Debug.Log("bbbb");
        for (int i = 0; i < 6; i++)//最长等待3s
        {
            if (MySocket.Connected)
            {
                Debug.Log("连接成功");
                break;
            }
            Thread.Sleep(500);
        }

        Debug.Log("cccc");
        if (!MySocket.Connected)
        {
            MySocket.Close();
            Debug.Log("连接失败");
            return -1;//连接失败
        }
        Debug.Log("dddd");
        networkStream = MySocket.GetStream();

        return 0;
    }














}
