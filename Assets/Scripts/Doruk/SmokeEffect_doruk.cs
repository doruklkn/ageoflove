using UnityEngine;
using System.Collections;

public class SmokeEffect_doruk : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Coroutine smokeCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    public void PlaySmoke()
    {
        // Önceki smoke varsa iptal et
        if (smokeCoroutine != null)
        {
            StopCoroutine(smokeCoroutine);
        }

        // Animasyonu sıfırla
        animator.enabled = false;

        // Görünür yap
        spriteRenderer.enabled = true;

        // Animator'u yeniden başlat
        animator.enabled = true;
        animator.Play("Smoke", 0, 0f);

        // Yeni animasyonun süresini bekle
        smokeCoroutine = StartCoroutine(HideAfterAnimation());
    }

    IEnumerator HideAfterAnimation()
    {
        // Animasyonun süresi kadar bekle
        yield return new WaitForSeconds(0.3f);

        animator.enabled = false;
        spriteRenderer.enabled = false;

        smokeCoroutine = null;
    }
}