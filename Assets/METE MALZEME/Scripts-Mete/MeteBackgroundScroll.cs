using UnityEngine;

public class MeteBackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 3f;   // saniyede kaç birim aşağı kayar

    float height;
    float startY;
    Transform copy;

    void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        startY = transform.position.y;

        // kendisinin bir kopyasını tam üstüne koyar, boşluk kalmasın
        GameObject c = Instantiate(gameObject, transform.position + Vector3.up * height, transform.rotation, transform.parent);
        c.GetComponent<MeteBackgroundScroll>().enabled = false;
        copy = c.transform;
    }

    void Update()
    {
        float d = scrollSpeed * Time.deltaTime;
        Move(transform, d);
        Move(copy, d);
    }

    void Move(Transform t, float d)
    {
        t.position += Vector3.down * d;
        if (t.position.y <= startY - height)
            t.position += Vector3.up * height * 2f;
    }
}