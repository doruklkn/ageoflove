using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}