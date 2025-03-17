using System;
using UniRx;

public class HasEmptySpaceRequirement : IFactoryRequirement
{
    public IReadOnlyReactiveProperty<bool> IsProductionAllowed => _isProductionAllowed;
    
    private readonly ReactiveProperty<bool> _isProductionAllowed;

    private readonly FactoryStorage _producedItemsStorage;
    private readonly NotificationService _notificationService;
    
    private readonly IDisposable _subscription;

    public HasEmptySpaceRequirement(FactoryStorage producedItemsStorage, NotificationService notificationService)
    {
        _producedItemsStorage = producedItemsStorage;
        _notificationService = notificationService;
        
        _isProductionAllowed = new ReactiveProperty<bool>(false);
        _subscription = producedItemsStorage.ResourcesCount.Subscribe(_ => CheckRequirements());
    }

    private void CheckRequirements()
    {
        if (_producedItemsStorage.HasEmptyPlace)
        {
            _isProductionAllowed.Value = true;
            return;
        }
        
        if (IsProductionAllowed.Value)
        {
            var notification = new FullStorageNotification(_producedItemsStorage.ResourceType);
            _notificationService.ShowNotification(notification);
            _isProductionAllowed.Value = false;
        }
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}
