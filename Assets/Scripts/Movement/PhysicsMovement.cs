using UnityEngine;

public class PhysicsMovement : BaseMovement
{
    [SerializeField]
    private Rigidbody _rigidbody;

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + Direction * Speed * Time.fixedDeltaTime);
    }

    public override void Rotate()
    {
        if (Direction != Vector3.zero)
        {
            _rigidbody.rotation = Quaternion.LookRotation(Direction);
        }
    }
}
