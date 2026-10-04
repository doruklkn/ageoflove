using UnityEngine;

public class MeteObstacle : MonoBehaviour
{
    public float fallSpeed = 5f;
    public bool jumpable = true;    // kapalıysa üstünden zıplanamaz
    public float weight = 1f;       // ne kadar sık çıksın (zıplanamayana 0.3 gibi düşük ver)
    public float spinSpeed = 120f;  // saniyede kaç derece döner (0 = dönmez)
    public bool randomSpin = true;  // true: her engel rastgele yönde ve hızda döner

    float spin;

    void Start()
    {
        spin = spinSpeed;
        if (randomSpin)
            spin = spinSpeed * Random.Range(0.5f, 1.5f) * (Random.value < 0.5f ? -1f : 1f);

        // rastgele açıyla doğsun (dönmeyecek engellerde bunu silebilirsin)
        if (spinSpeed != 0f)
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, spin * Time.deltaTime);

        if (transform.position.y < -9f) Destroy(gameObject);
    }
}