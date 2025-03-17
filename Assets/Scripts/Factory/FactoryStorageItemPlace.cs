using UnityEngine;

public class FactoryStorageItemPlace
{
    public Transform Position { get; private set; }
    public ResourceItem Resource { get; private set; }

    public FactoryStorageItemPlace(Transform position)
    {
        Position = position;
    }

    public void SetResource(ResourceItem value)
    {
        Resource = value;

        if (Resource == null)
        {
            return;
        }
            
        Resource.transform.parent = Position;
        Resource.MoveTo(Position.position, Position.rotation);
    }
}