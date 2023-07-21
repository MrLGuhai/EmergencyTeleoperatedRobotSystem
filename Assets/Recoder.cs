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
using System.Collections;
using UnityEditor;

public class Recoder
{
    private String floderPath;

    public string ipAddress = Config.IP;
    public int ipPort = 5001;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] recvBuffer;
    private List<byte> receivedMeshData;
    private List<Cas.Proto.Mesh> receivedMeshMessage;

    public static void Main()
    {
        try
        {
            Recoder recoder = new Recoder();
            recoder.client = new TcpClient();
            Debug.Log("连接服务器中...");
            recoder.client.Connect(IPAddress.Parse(recoder.ipAddress), recoder.ipPort);
            recoder.client.ReceiveBufferSize = 20971520;
            recoder.stream = recoder.client.GetStream();
            Debug.Log("连接服务器成功！");

            recoder.recvBuffer = new byte[recoder.client.ReceiveBufferSize];
            recoder.receivedMeshData = new List<byte>();
            recoder.receivedMeshMessage = new List<Cas.Proto.Mesh>();
            Thread receiveThread = new Thread(new ThreadStart(recoder.ReceiveMessage));
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
        List<byte> bufferList = new List<byte>();
        while (stream.CanRead)
        {
            int bytesRead;
            while ((bytesRead = stream.Read(recvBuffer, 0, client.ReceiveBufferSize)) > 0)
            {
                Debug.Log("client.ReceiveBufferSize : " + client.ReceiveBufferSize);
                Debug.Log("bytesRead = " + bytesRead);
                bufferList.AddRange(recvBuffer.Take(bytesRead));
                try
                {
                    MemoryStream protoStream = new MemoryStream(recvBuffer, 0, bytesRead);
                    DataMessage dataMessage = DataMessage.Parser.ParseFrom(protoStream);
                    Debug.Log("接收到数据类型：" + dataMessage.Type);
                    switch (dataMessage.Type)
                    {
                        case DataMessage.Types.Type.Mesh:
                            Debug.Log("接收到Mesh数据");
                            receivedMeshMessage.Add(dataMessage.Mesh);
                            break;
                        case DataMessage.Types.Type.ExitMesh:
                            DataMessage meshMessage = DataMessage.Parser.ParseFrom(bufferList.ToArray());
                            receivedMeshMessage.Add(meshMessage.Mesh);
                            Debug.Log("接收到Exit_Mesh指令，结束此次接收！");
                            bufferList.Clear();
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
    }

    public void saveBotLocation(Vector3 location, int[] moveSequence)
    {


    }

    public void saveArmAngles()
    {

    }

    public void saveGripperStatus(bool status)
    {
        // String filePath = getFilePath(path);
    }


    private void writeToFile()
    {


    }

    private String getCurrentTime(String format)
    {
        //获取当前时间
        DateTime now = DateTime.Now;
        //转换为字符串： yymmddhhmmss
        return now.ToString(format);
    }

    private String getFilePath(String path, String suffix)
    {
        return floderPath + "/" + path + "/" + getCurrentTime("HHmmss") + suffix;

    }
}
