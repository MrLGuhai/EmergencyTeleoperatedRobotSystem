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
using Newtonsoft.Json;

namespace Cas
{
    public class Replayer : MonoBehaviour
    {

        private PlyLoader loader;
        private int taskNum = 0;
        private string taskName;
        private string taskDescription;
        private string taskDate;
        private string taskStart;
        private string taskEnd;
        private string taskDuration;
        private string prefixPath;
        public Shader shader = Shader.Find("Mixed Reality Toolkit/Dashed Ray");
        public GameObject car;

        void Start()
        {
            loader = new PlyLoader();

            //TODO: 服务器端自动创建多层文件夹，subtasks下有 task-1, task-2, task-3, ... 文件夹，
            // 例如task-1为重建场景文件夹，里面有scene.ply文件，若为等待任务或其他不需要文件数据的任务，则不生成文件夹
        }

        public void play(string jsonFilePath)
        {
            // 读取JSON文件
            string json = File.ReadAllText(jsonFilePath);
            // 将JSON字符串反序列化为对象
            ReplayData replayData = JsonConvert.DeserializeObject<ReplayData>(json);

            taskName = replayData.name;
            taskDescription = replayData.description;
            taskDate = replayData.date;
            taskStart = replayData.start;
            taskEnd = replayData.end;
            taskDuration = replayData.duration;

            // 打印对象的属性
            Debug.Log(replayData.name);
            Debug.Log(replayData.description);
            Debug.Log(replayData.date);
            Debug.Log(replayData.start);
            Debug.Log(replayData.end);
            Debug.Log(replayData.duration);

            foreach (Subtask subtask in replayData.subtasks)
            {
                taskNum++;
                switch (subtask.type)
                {
                    case SubtaskType.Wait:
                        waitByMilliseconds(subtask.wait_time);
                        break;
                    case SubtaskType.Reconstruct:
                        destoryChildren();
                        playScence(subtask.scene_file);
                        break;
                    case SubtaskType.MoveBot:
                        playBotLocation(subtask.bot_location, subtask.bot_move_sequence);
                        break;
                    case SubtaskType.ExecuteArm:
                        playArmAngles(subtask.arm_angles);
                        break;
                    case SubtaskType.ControlGripper:
                        playGripperStatus(subtask.gripper_status);
                        break;
                    default:
                        Debug.Log("回放失败，未知的子任务类型!" + subtask.type);
                        break;
                }
            }
        }

        private string getFilepath(string fileName)
        {
            return Application.dataPath + "/" + taskName + "/subtasks/task-" + taskNum + "/" + fileName;
        }

        private void waitByMilliseconds(double? milliseconds)
        {
            Debug.Log("等待" + milliseconds + "毫秒");
            Thread.Sleep((int)milliseconds);
        }


        private void destoryChildren()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        private void playScence(string sceneFile)
        {
            Debug.Log("重建 " + sceneFile + " 场景");
            string filePath = getFilepath(sceneFile);

            // 使用PlyLoader读取.ply 文件
            PlyLoader loader = new PlyLoader();
            UnityEngine.Mesh[] mesh = loader.load(filePath);
            for (int i = 0; i != mesh.Length; ++i)
            {
                GameObject obj = new GameObject();
                obj.transform.parent = transform;   //设为子物体
                mesh[i].name = obj.name = "mesh" + i;
                MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh[i];
                MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
                Material material = new Material(shader);  //设置材质Shader
                meshRenderer.material = material;
            }
        }


        private void playBotLocation(float[] location, int[] moveSequence)
        {
            int seqFlag = moveSequence[0];
            if (seqFlag == 1)   //先移动
            {
               
            }
            else if (seqFlag == 0)
            {

            }

            car.transform.position = new Vector3(location[0], location[1], location[2]);
        }
        private IEnumerator SmoothMove(GameObject target, Vector3 targetPosition, float duration)
        {
            float time = 0;
            Vector3 startPosition = target.transform.position;

            while (time < duration)
            {
                target.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            target.transform.position = targetPosition;
        }

        private delegate void seqExecuDelegate(string message);
        private void executeMoveSequence(seqExecuDelegate oddSeqExecu, seqExecuDelegate evenSeqExecu, int[] moveSequence)
        {
            for (int i = 0; i < moveSequence.Length; i++)
            {
                if (i % 2 == 1)
                {
                    oddSeqExecu(moveSequence[i].ToString());
                }
                else
                { 
                    evenSeqExecu(moveSequence[i].ToString());
                }
            }
        }

        private void playArmAngles(int[] armAngles)
        {
            Debug.Log("执行机械臂的角度为 " + armAngles);
        }

        private void playGripperStatus(int? status)
        {
            Debug.Log("执行夹爪的状态为 " + status);
        }

    }


    public enum SubtaskType
    {
        Wait,
        Reconstruct,
        MoveBot,
        ExecuteArm,
        ControlGripper,
    }

    public class Subtask
    {
        public SubtaskType type { get; set; }
        public double? wait_time { get; set; }
        public string? scene_file { get; set; }
        public float[]? bot_location { get; set; }
        public int[]? bot_move_sequence { get; set; }
        public int[]? arm_angles { get; set; }
        public int? gripper_status { get; set; }
    }

    public class ReplayData
    {
        public string name { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string duration { get; set; }
        public List<Subtask> subtasks { get; set; }
    }
}
