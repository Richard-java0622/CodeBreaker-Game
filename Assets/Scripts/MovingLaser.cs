using UnityEngine;

public class MovingLaser : MonoBehaviour
{
    [SerializeField] protected float moveDistance = 3f;
    [SerializeField] protected float moveSpeed = 2f;

    protected Vector3 startPosition;

    protected void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        transform.position = startPosition + Vector3.left * movement;
    }
}
