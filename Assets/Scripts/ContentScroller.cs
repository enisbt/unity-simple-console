using UnityEngine;

public class ContentScroller : MonoBehaviour
{
    [SerializeField] RectTransform scrollRect;
    [SerializeField] RectTransform contentRect;

    public void AdjustContent()
    {
        if (contentRect.rect.height < scrollRect.rect.height)
        {
            // Move scroll rect content to bottom when content rect is smaller than scroll rect
            contentRect.pivot = new Vector2(contentRect.pivot.x, 0);
        }
        else
        {
            contentRect.pivot = new Vector2(contentRect.pivot.x, 0.5f);
            float posY = (contentRect.rect.height - scrollRect.rect.height) / 2;
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, posY);
        }
    }
}
