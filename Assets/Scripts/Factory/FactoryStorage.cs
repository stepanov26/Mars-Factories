using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class FactoryStorage : MonoBehaviour
{
    [SerializeField] private int _capacity;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private StorageType _storageType;
    [SerializeField] private List<Transform> _placeTransforms;

    public ResourceType ResourceType => _resourceType;
    public StorageType StorageType => _storageType;

    public bool HasEmptyPlace => _capacity > _resourcesCount.Value;
    public bool HasResource => _resourcesCount.Value > 0;

    public IReadOnlyReactiveProperty<int> ResourcesCount => _resourcesCount;

    private readonly ReactiveProperty<int> _resourcesCount = new();
    private IEnumerable<FactoryStorageItemPlace> _itemPlaces;


    private void Awake()
    {
        _itemPlaces = _placeTransforms.Select(x => new FactoryStorageItemPlace(x)).ToArray();
    }
    
    public void AddResource(ResourceItem resource)
    {
        var emptyPlace = _itemPlaces.First(x => x.Resource == null);
        emptyPlace.SetResource(resource);
        _resourcesCount.Value++;
    }
    
    public ResourceItem GetResource()
    {
        var place = _itemPlaces.Last(x => x.Resource != null);
        var resource = place.Resource;
        place.SetResource(null);
        _resourcesCount.Value--;
        return resource;
    }
}