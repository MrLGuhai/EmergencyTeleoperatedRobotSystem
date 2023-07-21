using ProtoBuf;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Cas.Proto;

public class MainBuckup : MonoBehaviour
{
    private GameObject[] axles;
    private float timer = 0.0f;
    private float interval = 1.0f; // 每1秒发送一次
    //private float[] angles; // 存储物体角度信息的数组
    private int[] angles;

    //public  int id1 = GameObject.Find("test").GetComponent<aaa>().id;

    //Debug.Log(fw);

    private String myRobotIP = null;//存储下位机IP
    private int myRobotPort = 0;//存储下位机端口号
    private TcpClient mySocket = null;//存储Socket通讯
    private NetworkStream networkStream = null;

    //private Byte currPackNum = 0;//当前的包序号,下一次发送要使用的包序号。

    public TcpClient MySocket { get => mySocket; set => mySocket = value; }
    public string MyRobotIP { get => myRobotIP; set => myRobotIP = value; }
    public int MyRobotPort { get => myRobotPort; set => myRobotPort = value; }

    public string ipAddress = "192.168.1.99";
    public int ipPort = 5001;

    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer;
    private List<byte> receivedMeshData;

    private List<Cas.Proto.Mesh> receivedMeshMessage;

    private Vector3 receivedSoundSourceData;
    private bool isKeepReading = true;
    private bool isDisplayedMesh = false;
    private Thread receiveThread;

    void Start()
    {
        // 在 Start 函数中获取带有 "Axle" 标签的物体并按照 Hierarchy 中的顺序排序
        axles = GameObject.FindGameObjectsWithTag("Axle").OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
        // 初始化角度信息数组
        angles = new int[axles.Length];
        //创建一个TcpClient对象，并连接到服务器
        try
        {
            client = new TcpClient();
            Debug.Log("连接服务器中...");
            client.Connect(IPAddress.Parse(ipAddress), ipPort);
            Debug.Log("连接服务器成功！");

            receivedMeshData = new List<byte>();

            receivedMeshMessage = new List<Cas.Proto.Mesh>();

            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            Debug.Log("启动线程接收数据");
        }
        catch (Exception e)
        {
            Debug.Log("连接服务器失败！");
            Debug.Log(e.Message);
        }
    }

    private void ReceiveMessage()
    {
        stream = client.GetStream();
        int bytesRead;
        while (stream.CanRead)
        {
            buffer = new byte[client.ReceiveBufferSize];
            bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);

            Debug.Log("client.ReceiveBufferSize : " + client.ReceiveBufferSize);
            Debug.Log("bytesRead = " + bytesRead);

            if (bytesRead > 0)
            {
                MemoryStream protoStream = new MemoryStream(buffer, 0, bytesRead);
                Cas.Proto.DataMessage dataMessage = Cas.Proto.DataMessage.Parser.ParseFrom(protoStream);

                Debug.Log("接收到数据类型：" + dataMessage.Type);
                switch (dataMessage.Type)
                {
                    case DataMessage.Types.Type.Mesh:
                        Debug.Log("接收到Mesh数据");
                        receivedMeshMessage.Add(dataMessage.Mesh);
                        break;
                    case DataMessage.Types.Type.ExitMesh:
                        Debug.Log("接收到Exit_Mesh指令，结束此次接收！");
                        isKeepReading = false;
                        break;
                    case DataMessage.Types.Type.Other:
                        Debug.Log("其他数据类型");
                        break;
                    default:
                        Debug.Log("未知数据类型");
                        break;
                }
            }
        }
    }

    private void DisplayMesh()
    {
        // byte[] receivedBytes = receivedMeshData.ToArray();

        //创建一个包含Protobuf数据的MemoryStream对象
        // MemoryStream protoStream = new MemoryStream(receivedBytes);
        // Cas.ProtobufNet.Mesh meshMessage = Serializer.Deserialize<Cas.ProtobufNet.Mesh>(protoStream);

        Cas.Proto.Mesh meshMessage = new Cas.Proto.Mesh();
        if (receivedMeshMessage == null)
        {
            Debug.Log("面片数据为空");
            return;
        }
        foreach (Cas.Proto.Mesh item in receivedMeshMessage)
        {
            meshMessage.V1.Add(item.V1);
            meshMessage.V2.Add(item.V2);
            meshMessage.V3.Add(item.V3);
            meshMessage.R.Add(item.R);
            meshMessage.G.Add(item.G);
            meshMessage.B.Add(item.B);

        }


        int vertexCount = meshMessage.V1.Count;
        int tripleVertexCount = vertexCount * 3;

        Vector3[] vertices = new Vector3[tripleVertexCount];
        int[] triangles = new int[tripleVertexCount];
        Color[] colors = new Color[tripleVertexCount];

        for (int i = 0; i < vertexCount; i++)
        {
            vertices[i * 3 + 0].x = meshMessage.V1[i].X;
            vertices[i * 3 + 0].y = meshMessage.V1[i].Y;
            vertices[i * 3 + 0].z = meshMessage.V1[i].Z;

            vertices[i * 3 + 1].x = meshMessage.V2[i].X;
            vertices[i * 3 + 1].y = meshMessage.V2[i].Y;
            vertices[i * 3 + 1].z = meshMessage.V2[i].Z;

            vertices[i * 3 + 2].x = meshMessage.V3[i].X;
            vertices[i * 3 + 2].y = meshMessage.V3[i].Y;
            vertices[i * 3 + 2].z = meshMessage.V3[i].Z;

            triangles[i * 3 + 0] = i * 3 + 0;
            triangles[i * 3 + 1] = i * 3 + 1;
            triangles[i * 3 + 2] = i * 3 + 2;

            colors[3 * i + 0] = new Color(meshMessage.R[i], meshMessage.G[i], meshMessage.B[i], 1);
            colors[3 * i + 1] = new Color(meshMessage.R[i], meshMessage.G[i], meshMessage.B[i], 1);
            colors[3 * i + 2] = new Color(meshMessage.R[i], meshMessage.G[i], meshMessage.B[i], 1);
        }

        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;    // 顶点数超过65535时需要设置此项
        Material mat = new Material(Shader.Find("Unlit/VertexColor"));  // 顶点颜色材质
        GetComponent<MeshFilter>().mesh = mesh;                         // 设置网格
        GetComponent<MeshRenderer>().material = mat;                    // 设置材质

        //获取游戏对象的MeshFilter组件
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        //旋转180度
        meshFilter.transform.rotation = Quaternion.Euler(180, 0, 0);

        mesh.vertices = vertices;   // 顶点
        mesh.triangles = triangles; // 三角面片索引
        mesh.colors = colors;       // 颜色

        Debug.Log("面片数据处理完毕！");
        Debug.Log("VertexCount = : " + vertexCount);
    }

    private void OnApplicationQuit()
    {
        isKeepReading = false;
        if (receiveThread != null)
        {
            receiveThread.Interrupt();
            receiveThread.Abort();
        }
        if (client != null)
        {
            client.Close();
        }
    }


    /*
         *@function:配置与下位机通信的IP和端口号，每次连接都需要重新初始化。
         *调用该函数后，会启动TCP连接，并发送START帧并验证。
         *@param:下位机的IP和Port。
         *@return：通信建立成功返回0,TCP连接失败返回-1，START帧无响应返回-2。
         */
    public int initConfig()
    {
        if (MySocket != null)
        {
            MySocket.Close();
        }
        MySocket = new TcpClient();

        MySocket.ConnectAsync(ipAddress, ipPort);//异步创建TCP连接，非阻塞
        for (int i = 0; i < 6; i++)//最长等待3s
        {
            if (MySocket.Connected)
            {
                Debug.Log("连接成功");
                break;
            }
            Thread.Sleep(500);
        }

        if (!MySocket.Connected)
        {
            MySocket.Close();
            Debug.Log("连接失败");
            return -1;//连接失败
        }

        networkStream = MySocket.GetStream();

        return 0;
    }

    private void Update()
    {
        if (isKeepReading == true)
        {
            return;
        }
        else
        {
            if (isDisplayedMesh == false)
            {
                DisplayMesh();
                isDisplayedMesh = true;
            }
        }

        timer += Time.deltaTime;
        //Debug.Log("Time.deltaTime = "+ Time.deltaTime);
        //Debug.Log("timer = "+ timer);
        //Debug.Log("interval = "+ interval);


        if (timer >= interval)
        {
            timer = 0.0f;

            for (int i = 0; i < axles.Length; i++)
            {
                Vector3 localAngles = axles[i].transform.localRotation.eulerAngles;

                // 判断欧拉角中哪个不为0，将其存储起来
                if (localAngles.x != 0)
                {
                    if (localAngles.x > 180)
                    {
                        angles[i] = (int)localAngles.x - (int)360f;
                    }
                    else
                    {
                        angles[i] = (int)localAngles.x;
                    }
                }
                else if (localAngles.y != 0)
                {
                    if (localAngles.y > 180)
                    {
                        angles[i] = -((int)localAngles.y - (int)360f);
                    }
                    else
                    {
                        angles[i] = -((int)localAngles.y);
                    }
                }
                else if (localAngles.z != 0)
                {
                    if (localAngles.z > 180)
                    {
                        angles[i] = (int)localAngles.z - (int)360f;
                    }
                    else
                    {
                        angles[i] = (int)localAngles.z;
                    }
                }



                //Debug.Log(axles[i].name + " angle: " + angles[i].ToString()); // 打印物体的名称和限制后的角度值，保留小数点后三位


            }//end for


            Debug.Log(axles[0].name + " angle: " + angles[0] + "  " +
                axles[1].name + " angle: " + angles[1] + "  " +
                axles[2].name + " angle: " + angles[2] + "  " +
                axles[3].name + " angle: " + angles[3] + "  " +
                axles[4].name + " angle: " + angles[4] + "  " +
                axles[5].name + " angle: " + angles[5]
                );

            Int32[] temp = new Int32[6];

            temp[0] = (int)(angles[0] * 100);
            temp[1] = (int)(angles[1] * 100);
            temp[2] = (int)(angles[2] * 100);
            temp[3] = (int)(angles[3] * 100);
            temp[4] = (int)(angles[4] * 100);
            temp[5] = (int)(angles[5] * 100);




            //乘 100 ；转成 int；转成十六进制； 高位、低位

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



            commDatabuff[16] = 0x1E;//
            commDatabuff[17] = 0xFA;//







            //int angletemp = 0;

            /*
            for (int i = 0; i < 6; i++)//获取输入框中的舵机角度值，并填充到commDatabuff数组中
            {
                switch (i)
                {
                    case 1: angletemp = getAngleNum(angles[0]); break;
                    case 2: angletemp = getAngleNum(angles[1]); break;
                    case 3: angletemp = getAngleNum(angles[2]); break;
                    case 4: angletemp = getAngleNum(angles[3]); break;
                    case 5: angletemp = getAngleNum(angles[4]); break;
                    case 6: angletemp = getAngleNum(angles[5]); break;
                    default:break;

                }



                commDatabuff[(i - 1) * 2] = (Byte)(angletemp / 256);
                commDatabuff[(i - 1) * 2 + 1] = (Byte)(angletemp % 256);   
            }
            */
            //data part full end

            /*
            Debug.Log(" 1 = " + Convert.ToString(commDatabuff[0], 16) + "  " +
                " 2 = " + Convert.ToString(commDatabuff[1], 16) + "  " +
                " 3 = " + Convert.ToString(commDatabuff[2], 16) + "  " +
                " 4 = " + Convert.ToString(commDatabuff[3], 16) + "  " +
                " 5 = " + Convert.ToString(commDatabuff[4], 16) + "  " +
                " 6 = " + Convert.ToString(commDatabuff[5], 16) + "  " +
                " 7 = " + Convert.ToString(commDatabuff[6], 16) + "  " +
                " 8 = " + Convert.ToString(commDatabuff[7], 16) + "  " +
                " 9 = " + Convert.ToString(commDatabuff[8], 16) + "  " +
                " 10 = " + Convert.ToString(commDatabuff[9], 16) + "  " +
                " 11 = " + Convert.ToString(commDatabuff[10], 16) + "  " +
                " 12 = " + Convert.ToString(commDatabuff[11], 16) + "  " +
                " 13 = " + Convert.ToString(commDatabuff[12], 16) + "  " +
                " 14 = " + Convert.ToString(commDatabuff[13], 16)
                );
            */

            int ret = sentDataPack(commDatabuff);//发送数据包
            if (ret == 0)
            {
                Debug.Log("send data success");
            }
            else
            {
                Debug.Log("send data fail");
            }

            /*
            myCobotCommand.Comm_DataLen = 0x0e;//指令数据部分的长度
            myCobotCommand.Comm_DurationTime = (UInt32)int.Parse(action_dura_time.Text);//动作持续时间
            myCobotCommand.Comm_TimeFrame = myCobotCommand.Comm_TransitionTime = 0x00;
            myCobotCommand.Comm_Type = 0x21;//指令类型
            myCobotCommand.Comm_DataBuff = commDatabuff;//指令的数据部分
            */
        }
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



    /**
         * @function:发送数据包数据
         * @param:完整的数据包数组
         * @return:发送成功返回发送0,发送失败返回-1。
         */
    public int sentDataPack(Byte[] sendbuff)
    {
        int ret = 0;
        string temp = "";
        if (sendbuff == null)
        {
            Debug.Log("sendbuff == null");
            return -1;
        }
        else if (client == null)
        {
            Debug.Log("clent == null");
            return -1;
        }
        else if (!client.Connected)
        {
            Debug.Log("cilent == null");
            return -1;
        }
        else
        {
            Debug.Log("发送的数据：");
            for (int i = 0; i < sendbuff.Length; i++)
            {
                //temp += sendbuff[i];
                temp += Convert.ToString(sendbuff[i], 16) + " ";
            }
            Debug.Log(temp);
            Debug.Log("sendbuff.Length = " + sendbuff.Length);
            stream.Write(sendbuff, 0, sendbuff.Length);//阻塞式写函数

            //networkStream.Read();

            //currPackNum++;
            return ret;
        }

    }
}
