using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;

    private bool isMoving;

    void Update()
    {
        if (!isMoving || target == null) return;

        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        transform.position = newPos;
    }

    public void SetMoving()
    {
        isMoving = true;
    }
}
