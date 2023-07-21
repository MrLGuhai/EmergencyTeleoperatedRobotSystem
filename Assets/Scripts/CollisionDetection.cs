using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject[] Sphere;
    public GameObject[] Cylinder;
    public Rigidbody rb; // cube 的刚体组件
    public GameObject panel;
    public LimitMovement limit;// 限制运动的开关
    public GameObject except;
    public GameObject cube;
    private bool ensure=true;
    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < Sphere.Length; i++)
        {
            if (other.gameObject == Sphere[i])
            {
                ensure = false;
                break;        
            }
        }
        for (int j = 0; j < Cylinder.Length; j++)
        {
            if (other.gameObject == Cylinder[j])
            {
                ensure = false;
                break;
            }
        }
        if (ensure==true&&other.gameObject != except&&other.gameObject!=cube)
        {
            Debug.Log("A collided with " + other.gameObject.name);
            limit.limitMovement = true; // 触发限制运动
            panel.SetActive(true);
        }

    }

    void FixedUpdate()
    {
        if (limit.limitMovement)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition; // 限制 cube 的位置运动
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None; // 取消限制
        }
    }
}
