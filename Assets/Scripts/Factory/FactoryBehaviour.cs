using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class FactoryBehaviour : MonoBehaviour
{
    [SerializeField]
    private FactorySettings _settings;
    
    [SerializeField]
    private FactoryStorage _producedItemsStorage;

    [SerializeField]
    private List<FactoryStorage> _consumedItemsStorages;

    private float _productionTimer;
    private bool _isActive;
    private NotificationSystem _notificationSystem;

    [Inject]
    public void Construct(NotificationSystem notificationSystem)
    {
        _notificationSystem = notificationSystem;
    }

    private void Update()
    {
        if (!_isActive)
        {
            CheckRequirements();
            return;
        }

        _productionTimer += Time.deltaTime;
        if (_productionTimer >= _settings.ProductionInterval)
        {
            var resource = Instantiate(_settings.ResourceItem, transform.position, Quaternion.identity);
            _producedItemsStorage.AddResource(resource);
            _productionTimer = 0;
            CheckRequirements();
        }
    }

    private void CheckRequirements()
    {      
        if (!_producedItemsStorage.HasEmptyPlace)
        {
            if (_isActive)
            {
                var notification = new FullStorageNotification(_settings.ResourceItem.ResourceType);
                _notificationSystem.ShowNotification(notification);
            }
            _isActive = false;
            return;
        }

        if (_consumedItemsStorages.Count > 0)
        {
            var emptyStorages = _consumedItemsStorages.Where(storage => !storage.HasResource).Select(x => x.Resource).ToList();
            if (emptyStorages.Count > 0)
            {
                if (_isActive)
                {
                    var notification = new NoResourceNotification(_settings.ResourceItem.ResourceType, emptyStorages);
                    _notificationSystem.ShowNotification(notification);
                }
                _isActive = false;
                return;
            }
        }

        ConsumResources();
        _isActive = true;
    }

    private void ConsumResources()
    {
        foreach (var storage in _consumedItemsStorages)
        {
            Destroy(storage.GetResource().gameObject);
        }
    } 
}
