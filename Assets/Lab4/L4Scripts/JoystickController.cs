using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField, Range(0.1f, 1.5f)] private float handleRange = 1f;
    public Vector2 Direction { get; private set; }
    public float Horizontal => Direction.x;
    public float Vertical => Direction.y;

    private Canvas canvas;

    private void Awake()
    {
        if (background == null)
            background = (RectTransform)transform;

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, cam, out Vector2 localPoint))
            return;

        Vector2 radius = background.rect.size / 2f;
        Vector2 offset = localPoint - background.rect.center;
        Vector2 input = new Vector2(offset.x / radius.x, offset.y / radius.y);
        input = Vector2.ClampMagnitude(input, 1f);

        Direction = input;

        if (handle != null)
            handle.anchoredPosition = input * radius * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
