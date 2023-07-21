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
using Cas;
using Microsoft.MixedReality.Toolkit;
using System.Collections;
using Cas.Extensions;
using Microsoft.MixedReality.Toolkit.Subsystems;
using TMPro;

public class Main : MonoBehaviour
{
    public UploadAngle uploadAngle;
    public ETRSTextToSpeech TextToSpeech;

    private string ipAddress = Config.IP;
    private int ipPort = Config.port;
    private TcpClient client;
    private NetworkStream stream;
    private List<List<byte>> bufferArraryList;
    private int bufferArraryListCount = 0;
    private byte[] recvBuffer;
    private List<byte> receivedMeshData;
    private List<Cas.Proto.Mesh> receivedMeshMessage;
    private Vector3 receivedSoundSourceData;
    private bool isKeepReading = true;
    private bool isDisplayedMesh = false;
    private Thread receiveThread;
    private Thread analyzeThread;
    public GameObject kinect;
    private bool gripperStatus = false;
    private String meshShader = "Mixed Reality Toolkit/Dashed Ray";
    public TextMeshProUGUI menuTime;
    public TextMeshProUGUI menuTemp;
    public TextMeshProUGUI menuHumi;
    public TextMeshProUGUI menuVar;
    public TextMeshProUGUI menuBattery;
    public TextMeshProUGUI menuGripperStatus;

    private float tempText = 0;
    private float humiText = 0;
    private string gripperStatusText = "开";

    private int MeshRotationCorrection=0;

    void Start()
    {

        fixMeshPosition();

        UpdateLocation updateLocation = new UpdateLocation();
        uploadAngle = GetComponent<UploadAngle>();

        StartCoroutine(updateTime());
        bufferArraryList = new List<List<byte>>();
        //创建一个TcpClient对象，并连接到服务器
        try
        {
            client = new TcpClient();
            Debug.Log("连接服务器中...");
            Debug.Log("IP：" + Config.IP);
            Debug.Log("IPAddress:" + ipAddress);
            client.Connect(IPAddress.Parse(ipAddress), ipPort);
            client.ReceiveBufferSize = 102400;
            stream = client.GetStream();
            Debug.Log("连接服务器成功！");

            recvBuffer = new byte[client.ReceiveBufferSize];
            receivedMeshData = new List<byte>();
            receivedMeshMessage = new List<Cas.Proto.Mesh>();
            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            Debug.Log("启动线程接收数据");

            StartCoroutine(UploadAngleDataCoroutine());
        }
        catch (Exception e)
        {
            Debug.Log("连接服务器失败！");
            Debug.Log(e.Message);
        }

    }

    private void Update()
    {
        if (isDisplayedMesh == true)
        {
            RenderMesh();
            // 记录当前位置
            isDisplayedMesh = false;
        }
        updateMenuText();
    }

    IEnumerator updateTime()
    {
        while (true)
        {
            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;
            menuTime.text = hours + ":" + minutes;
            yield return null;
        }
    }

    private byte[] ReadBytes(NetworkStream stream, int bytesToRead)
    {
        byte[] buffer = new byte[bytesToRead];
        int bytesRead = 0;
        while (bytesRead < bytesToRead)
        {
            int read = stream.Read(buffer, bytesRead, bytesToRead - bytesRead);
            if (read == 0)
            {
                throw new EndOfStreamException();
            }
            bytesRead += read;
        }
        return buffer;
    }


    private void ReceiveMessage()
    {
        while (stream.CanRead)
        {
            byte[] lengthBytes = ReadBytes(stream, sizeof(int));
            int length = BitConverter.ToInt32(lengthBytes, 0);
            byte[] messageBytes = ReadBytes(stream, length);

            try
            {
                MemoryStream protoStream = new MemoryStream(messageBytes, 0, length);
                DataMessage dataMessage = DataMessage.Parser.ParseFrom(protoStream);
                switch (dataMessage.Type)
                {
                    case DataMessage.Types.Type.Mesh:
                        Debug.Log("接收到Mesh数据");
                        receivedMeshMessage.Add(dataMessage.Mesh);
                        break;
                    case DataMessage.Types.Type.ExitMesh:
                        isDisplayedMesh = true;
                        Debug.Log("接收到Exit_Mesh指令，结束此次接收！");
                        break;
                    case DataMessage.Types.Type.TempAndHumi:
                        //Debug.Log("温度：" + dataMessage.TempAndHumi.Temp.ToString() + "°" + "湿度：" + dataMessage.TempAndHumi.Humi.ToString() + "%");
                        tempText = dataMessage.TempAndHumi.Humi;
                        humiText = dataMessage.TempAndHumi.Temp;
                        break;
                    default:
                        Debug.Log("未知数据类型");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    private void RenderMesh()
    {
        fixMeshPosition();
        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;
        Material material = new Material(Shader.Find(meshShader));
        GetComponent<MeshRenderer>().material = material;

        //清空Mesh
        GetComponent<MeshFilter>().mesh.Clear();

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
        mesh.vertices = vertices;   // 顶点
        mesh.triangles = triangles; // 三角面片索引
        mesh.colors = colors;       // 颜色
        // FIXME: 记录第二次重建时小车的朝向，然后
        GetComponent<MeshFilter>().transform.rotation = Quaternion.Euler(180, 180+MeshRotationCorrection, 0);
        
        //FIXME:
        updateMenuVar("重建场景完毕！", 3);
    }

    // 面板重建场景按钮
    public void reconstrcuture()
    {
        //FIXME:
        updateMenuVar("重建场景中，请勿操作...");

        // 清空接收到的面片数据
        receivedMeshMessage.Clear();
        // 发送舵机旋转消息回服务器
        DataMessage dataMessage = new DataMessage();
        dataMessage.Type = DataMessage.Types.Type.BotMotor;
        dataMessage.BotMotor = new BotMotor();
        dataMessage.BotMotor.Angle = 0;
        client.sendMessage(dataMessage);
        Debug.Log("发送相机旋转消息回服务器");
    }

    public void fixMeshPosition()
    {
        transform.position = kinect.transform.position;
    }

    // 面板上传机器人位置按钮
    public void postBotLocation()
    {
        DataMessage dataMessage = new DataMessage();
        dataMessage.Type = DataMessage.Types.Type.BotCar;
        dataMessage.BotCar = new BotCar();
        UpdateLocation updateLocation = GetComponent<UpdateLocation>();
        dataMessage.BotCar.MoveSequence.AddRange(updateLocation.UpdatePosition());
        MeshRotationCorrection = updateLocation.GetSceneUpdateAngle();
        client.sendMessage(dataMessage);
        updateMenuVar("上传位置成功，机器人开始移动...", 3);
        Debug.Log("发送机器人位置消息回服务器");
    }

    // 面板控制夹爪按钮
    public void controlGripper()
    {
        DataMessage dataMessage = new DataMessage();
        dataMessage.Type = DataMessage.Types.Type.BotGripper;
        dataMessage.BotGripper = new BotGripper();
        dataMessage.BotGripper.Status = gripperStatus = gripperStatus ? false : true;
        gripperStatusText = gripperStatus ? "开" : "关";

        client.sendMessage(dataMessage);
        updateMenuVar("发送夹爪状态成功", 3);
        Debug.Log("发送夹爪消息回服务器");
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

    IEnumerator UploadAngleDataCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // 等待0.5秒
            Byte[] commDatabuff;//存储指令的数据部分
            commDatabuff = uploadAngle.GetAngleData();

            DataMessage dataMessage = new DataMessage();
            dataMessage.Type = DataMessage.Types.Type.BotArm;
            dataMessage.BotArm = new BotArm();
            // 将commDatabuff类型改成ByteString
            dataMessage.BotArm.DataBuffer = ByteString.CopyFrom(commDatabuff);

            client.sendMessage(dataMessage);
        }
    }

    private void updateMenuText()
    {
        menuHumi.text = tempText.ToString() + "%";
        menuTemp.text = humiText.ToString() + "°";
        menuGripperStatus.text = gripperStatusText;

    }

    private void updateMenuVar(string text)
    {
        menuVar.text = text;
        TextToSpeech.TextToSpeech(text);
    }

    private void updateMenuVar(string text, int continueTimeS)
    {
        menuVar.text = text;
        TextToSpeech.TextToSpeech(text);
        //持续 continueTimeS 秒
        StartCoroutine(continueMenuVarTime(continueTimeS));
    }

    IEnumerator continueMenuVarTime(int continueTimeS)
    {
        yield return new WaitForSeconds(continueTimeS);
        menuVar.text = "";
    }

}
