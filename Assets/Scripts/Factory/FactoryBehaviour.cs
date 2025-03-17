using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class FactoryBehaviour : MonoBehaviour
{
    [SerializeField]
    private ResourceItem _resourceItem;
    
    [SerializeField]
    private float _productionDuration;
    
    [SerializeField]
    private FactoryStorage _producedItemsStorage;

    [SerializeField]
    private FactoryStorage[] _consumedItemsStorages;
    
    private readonly CompositeDisposable _disposables = new();
    private IFactoryRequirement[] _requirements;

    private bool _isActive;
    private NotificationService _notificationService;

    [Inject]
    public void Construct(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    private void Awake()
    {
        _requirements = new IFactoryRequirement[]
        {
            new HasResourcesRequirement(_resourceItem.ResourceType, _consumedItemsStorages, _notificationService),
            new HasEmptySpaceRequirement(_producedItemsStorage, _notificationService),
        };

        foreach (var requirement in _requirements)
        {
            var subscription = requirement.IsProductionAllowed.Subscribe(_ => TryStartProduction());
            _disposables.Add(subscription);
            _disposables.Add(requirement);
        }

        TryStartProduction();
    }

    private void TryStartProduction()
    {
        if (_isActive)
        {
            return;
        }
        
        if (_requirements.All(requirement => requirement.IsProductionAllowed.Value))
        {
            ConsumeResources();
            StartCoroutine(ProduceResource());
        }
    }

    private void ConsumeResources()
    {
        foreach (var storage in _consumedItemsStorages)
        {
            Destroy(storage.GetResource().gameObject);
        }
    } 
    
    private IEnumerator ProduceResource()
    {
        _isActive = true;
        yield return new WaitForSeconds(_productionDuration);
        _isActive = false;
        
        var resource = Instantiate(_resourceItem, transform.position, Quaternion.identity);
        _producedItemsStorage.AddResource(resource);
        TryStartProduction();
    }

    private void OnDestroy()
    {
        _disposables.Clear();
    }
}
