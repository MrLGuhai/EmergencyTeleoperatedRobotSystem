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


    private String myRobotIP = null;//�洢��λ��IP
    private int myRobotPort = 0;//�洢��λ���˿ں�
    private TcpClient mySocket = null;//�洢SocketͨѶ
    private NetworkStream networkStream = null;

    //private Byte currPackNum = 0;//��ǰ�İ����,��һ�η���Ҫʹ�õİ���š�

    public TcpClient MySocket { get => mySocket; set => mySocket = value; }
    public string MyRobotIP { get => myRobotIP; set => myRobotIP = value; }
    public int MyRobotPort { get => myRobotPort; set => myRobotPort = value; }



    byte[] data;



    // Start is called before the first frame update
    void Start()
    {

        if (initConfig("192.168.1.102", 5001) == 0)
        {
            Debug.Log("�Ͽ�");
            Debug.Log("Mesg:" + "�����ӡ�");
        }
        else
        {
            Debug.Log("����");
            Debug.Log("Mesg:" + "������λ��ʧ�ܡ�");
        }








        












    }

    // Update is called once per frame
    void Update()
    {
        
    }





    /*
         *@function:��������λ��ͨ�ŵ�IP�Ͷ˿ںţ�ÿ�����Ӷ���Ҫ���³�ʼ����
         *���øú����󣬻�����TCP���ӣ�������START֡����֤��
         *@param:��λ����IP��Port��
         *@return��ͨ�Ž����ɹ�����0,TCP����ʧ�ܷ���-1��START֡����Ӧ����-2��
         */
    public int initConfig(String ip, int port)
    {

        if (MySocket != null)
        {
            MySocket.Close();
        }
        MySocket = new TcpClient();

        Debug.Log("aaaa");
        MySocket.ConnectAsync(ip, port);//�첽����TCP���ӣ�������
        Debug.Log("bbbb");
        for (int i = 0; i < 6; i++)//��ȴ�3s
        {
            if (MySocket.Connected)
            {
                Debug.Log("���ӳɹ�");
                break;
            }
            Thread.Sleep(500);
        }

        Debug.Log("cccc");
        if (!MySocket.Connected)
        {
            MySocket.Close();
            Debug.Log("����ʧ��");
            return -1;//����ʧ��
        }
        Debug.Log("dddd");
        networkStream = MySocket.GetStream();

        return 0;
    }














}
