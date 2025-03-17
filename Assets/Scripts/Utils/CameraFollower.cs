using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.1f;

    private Transform _playerTransform;
    
    [Inject]
    public void Construct(PlayerBehaviour player)
    {
        _playerTransform = player.transform;
    }

    private void FixedUpdate()
    {
        if (_playerTransform == null)
        {
            return;
        }

        Vector3 velocity = Vector3.zero;
        Vector3 targetPosition = _playerTransform.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
    }
}
