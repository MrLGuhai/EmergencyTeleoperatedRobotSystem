using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;                //反编译
/// <summary>
/// 获取Unity面板中的物体的旋转值
/// </summary>
public class GetInspectorRotationValue : MonoBehaviour
{
	public static GetInspectorRotationValue Instance;
	private void Awake()
	{
		Instance = this;                            //单例模式
	}
	public Vector3 GetInspectorRotationValueMethod(Transform transform)
	{
		// 获取原生值
		System.Type transformType = transform.GetType();
		PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
		object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
		MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
		object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
		//Debug.Log("反射调用GetLocalEulerAngles方法获得的值：" + value.ToString());
		string temp = value.ToString();
		//将字符串第一个和最后一个去掉
		temp = temp.Remove(0, 1);
		temp = temp.Remove(temp.Length - 1, 1);
		//用‘，’号分割
		string[] tempVector3;
		tempVector3 = temp.Split(',');
		//将分割好的数据传给Vector3
		Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
		return vector3;
	}
}