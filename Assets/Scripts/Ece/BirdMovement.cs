using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}