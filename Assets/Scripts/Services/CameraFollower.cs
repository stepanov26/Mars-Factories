using UnityEngine;
using Zenject;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private Transform _playerTransform;

    [Inject]
    public void Construct(PlayerBehaviour player)
    {
        _playerTransform = player.transform;
    }

    private void LateUpdate()
    {
        if (_playerTransform == null) return;

        Vector3 targetPosition = _playerTransform.position + _offset;
        transform.position = targetPosition;
    }
}
