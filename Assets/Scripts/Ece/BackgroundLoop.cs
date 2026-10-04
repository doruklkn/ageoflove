using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public float speed = 5f;

    private float width;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Sola hareket
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Kameranın sol kenarı
        float cameraLeft =
            Camera.main.transform.position.x -
            Camera.main.orthographicSize * Camera.main.aspect;

        // Tamamen kameranın arkasına çıktıysa
        if (transform.position.x + width / 2f < cameraLeft)
        {
            MoveToRight();
        }
    }

    void MoveToRight()
    {
        GameObject[] allBackgrounds =
            GameObject.FindGameObjectsWithTag("Background");

        float rightEdge = float.MinValue;

        foreach (GameObject background in allBackgrounds)
        {
            if (background == gameObject)
                continue;

            SpriteRenderer sprite =
                background.GetComponent<SpriteRenderer>();

            float edge =
                background.transform.position.x +
                sprite.bounds.size.x / 2f;

            if (edge > rightEdge)
            {
                rightEdge = edge;
            }
        }

        transform.position = new Vector3(
            rightEdge + width / 2f - 0.1f,
            transform.position.y,
            transform.position.z
        );
    }
}