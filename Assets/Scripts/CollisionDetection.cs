using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject[] Sphere;
    public GameObject[] Cylinder;
    public Rigidbody rb; // cube �ĸ������
    public GameObject panel;
    public LimitMovement limit;// �����˶��Ŀ���
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
            limit.limitMovement = true; // ���������˶�
            panel.SetActive(true);
        }

    }

    void FixedUpdate()
    {
        if (limit.limitMovement)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition; // ���� cube ��λ���˶�
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None; // ȡ������
        }
    }
}
