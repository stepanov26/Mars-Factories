using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickInput : MonoBehaviour, IInput, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickHandle;
    private Vector2 _joystickPosition;
    private float _joystickRadius;

    public Vector3 MoveDirection { get; private set; }

    private void Start()
    {
        _joystickBackground.gameObject.SetActive(false);
        _joystickRadius = _joystickBackground.sizeDelta.x / 2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _joystickBackground.gameObject.SetActive(true);
        _joystickPosition = eventData.position;
        _joystickBackground.position = _joystickPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickBackground.gameObject.SetActive(false);
        MoveDirection = Vector3.zero;
        _joystickHandle.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - _joystickPosition;
        float distance = Mathf.Clamp(direction.magnitude, 0, _joystickRadius);
        var inputDirection = direction.normalized * (distance / _joystickRadius);

        _joystickHandle.position = _joystickPosition + (inputDirection * _joystickRadius);
        MoveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }
}
