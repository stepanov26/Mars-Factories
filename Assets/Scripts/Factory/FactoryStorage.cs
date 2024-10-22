using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FactoryStorage : MonoBehaviour
{
    [SerializeField] private int _capacity;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private StorageType _storageType;
    [SerializeField] private List<Transform> _placeTransforms;

    public ResourceType Resource => _resourceType;
    public StorageType StorageType => _storageType;

    public bool HasEmptyPlace => _capacity > _itemPlaces.Count(x => x.Resource != null);
    public bool HasResource => _itemPlaces.Any(x => x.Resource != null);

    private List<ItemPlace> _itemPlaces;

    private void Awake()
    {
        _itemPlaces = _placeTransforms.Select(x => new ItemPlace(x)).ToList();
    }

    public ResourceItem GetResource()
    {
        var place = _itemPlaces.FindLast(x => x.Resource != null);
        var resource = place.Resource;
        place.SetResource(null);
        return resource;
    }

    public void AddResource(ResourceItem resource)
    {
        var emptyPlace = _itemPlaces.Find(x => x.Resource == null);
        emptyPlace.SetResource(resource);
        resource.transform.position = emptyPlace.Position.position;
        resource.transform.rotation = emptyPlace.Position.rotation;
        resource.transform.parent = emptyPlace.Position;
    }

    public class ItemPlace
    {
        public Transform Position { get; private set; }
        public ResourceItem Resource { get; private set; }

        public ItemPlace(Transform position)
        {
            Position = position;
        }

        public void SetResource(ResourceItem value)
        {
            Resource = value;
        }
    }
}

public enum StorageType
{
    Consumed,
    Produced
}