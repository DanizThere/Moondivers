using UnityEngine;

public interface IControllable
{
    void Move(Vector3 dir);
    void Jump();
    void Dodge();
}
