using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    [SerializeField] 
    protected float Speed;

    protected Vector3 Direction;

    public virtual void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public abstract void Move();
    public abstract void Rotate();
}
