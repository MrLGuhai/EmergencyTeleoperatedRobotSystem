using UnityEngine;

public class MoveWithinCircle : MonoBehaviour
{
    public GameObject centerObject;
    public float radius = 3f;

    public void Start()
    {
    }

    private void Update()
    {
        Vector3 centerPosition = centerObject.transform.position;
        Vector3 currentPosition = transform.position;

        float distance = Vector3.Distance(centerPosition, currentPosition);

        if (distance > radius)
        {
            Vector3 direction = (currentPosition - centerPosition).normalized;
            Vector3 targetPosition = centerPosition + (direction * radius);
            transform.position = targetPosition;
        }
        
    }
}
