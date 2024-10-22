using UnityEngine;

public interface IMovement
{
    void SetDirection(Vector3 Direction);
    void Move();
    void Rotate();
}
