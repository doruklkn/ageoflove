using UnityEngine;
using System.Collections;

public class BirdFlight : MonoBehaviour
{
    public float targetX = 0.6f;
    public float targetY = 2.4f;
    public float flightSpeed = 2f;

    public GameObject endImage;
    public BirdOutroTransition outroTransition;

    private Animator animator;
    private bool hasStopped = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        if (endImage != null)
        {
            endImage.SetActive(false);
        }
    }

    void Update()
    {
        if (hasStopped)
            return;

        Vector3 targetPosition = new Vector3(
            targetX,
            targetY,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            flightSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            hasStopped = true;

            if (animator != null)
            {
                animator.enabled = false;
            }

            StartCoroutine(ShowEndImage());
        }
    }

    IEnumerator ShowEndImage()
    {
        yield return new WaitForSeconds(0.5f);

        if (endImage != null)
        {
            endImage.SetActive(true);
        }

        if (outroTransition != null)
        {
            outroTransition.StartTransition();
        }
    }
}