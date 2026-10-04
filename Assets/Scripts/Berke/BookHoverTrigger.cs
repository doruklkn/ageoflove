using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Açılacak Panel")]
    public GameObject bookHintPanel;

    [Header("Kitap Durum Spriteları")]
    public Sprite closedBookSprite;
    public Sprite openBookSprite;

    private Image triggerImage;

    private void Awake()
    {
        triggerImage = GetComponent<Image>();

        if (closedBookSprite != null && triggerImage != null)
        {
            triggerImage.sprite = closedBookSprite;
        }

        if (bookHintPanel != null)
        {
            bookHintPanel.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (triggerImage != null && openBookSprite != null)
        {
            triggerImage.sprite = openBookSprite;
        }

        if (bookHintPanel != null)
        {
            bookHintPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (triggerImage != null && closedBookSprite != null)
        {
            triggerImage.sprite = closedBookSprite;
        }

        if (bookHintPanel != null)
        {
            bookHintPanel.SetActive(false);
        }
    }
}