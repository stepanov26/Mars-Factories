using System.Linq;
using UniRx;

public class HasResourcesRequirement : IFactoryRequirement
{
    public IReadOnlyReactiveProperty<bool> IsProductionAllowed => _isProductionAllowed;
    
    private readonly ReactiveProperty<bool> _isProductionAllowed;
    
    private readonly FactoryStorage[] _consumedItemsStorages;
    private readonly NotificationService _notificationService;
    private readonly ResourceType _resourceType;
    
    private readonly CompositeDisposable _disposables = new();
    
    public HasResourcesRequirement(ResourceType resourceType, FactoryStorage[] consumedItemsStorages, NotificationService notificationService)
    {
        _consumedItemsStorages = consumedItemsStorages;
        _notificationService = notificationService;
        _resourceType = resourceType;
        _isProductionAllowed = new ReactiveProperty<bool>(false);

        foreach (var storage in _consumedItemsStorages)
        {
            var subscription = storage.ResourcesCount.Subscribe(_ => CheckRequirements());
            _disposables.Add(subscription); 
        }

        CheckRequirements();
    }

    private void CheckRequirements()
    {
        var emptyStorages = _consumedItemsStorages
            .Where(storage => !storage.HasResource)
            .Select(x => x.ResourceType)
            .ToArray();
        
        if (emptyStorages.Length == 0)
        {
            _isProductionAllowed.Value = true;
            return;
        }
        
        if (IsProductionAllowed.Value)
        {
            var notification = new NoResourceNotification(_resourceType, emptyStorages);
            _notificationService.ShowNotification(notification);
            _isProductionAllowed.Value = false;
        }
    }

    public void Dispose()
    {
        _disposables.Clear();
    }
}
