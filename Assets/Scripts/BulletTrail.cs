using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public float speed;
    public float destroyDelay;
    
    public Vector3 EndPoint { get; set; }

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position == EndPoint) return;
        
        transform.position = Vector3.MoveTowards(transform.position, EndPoint, Time.deltaTime * speed);
        if (transform.position == EndPoint)
        {
            Invoke(nameof(Destroy), destroyDelay);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}