using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField]
    private ResourceType _resourceType;

    public ResourceType ResourceType => _resourceType;
}