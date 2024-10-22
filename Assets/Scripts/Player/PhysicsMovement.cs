using UnityEngine;

public class PhysicsMovement : MonoBehaviour, IMovement
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _speed;

    private Vector3 _direction;

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _direction * _speed * Time.fixedDeltaTime);
    }

    public void Rotate()
    {
        if (_direction != Vector3.zero)
        {
            _rigidbody.rotation = Quaternion.LookRotation(_direction);
        }
    }
}
