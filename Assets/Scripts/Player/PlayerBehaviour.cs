using UnityEngine;
using Zenject;

public class PlayerBehaviour : MonoBehaviour
{ 
    [SerializeField]
    private BaseMovement _movement;

    [SerializeField]
    private ResourcesStack _resourcesStack;

    private IInput _input;

    [Inject]
    public void Construct(IInput input)
    {
        _input = input;
    }

    private void Awake()
    {
        _movement ??= GetComponent<BaseMovement>();
    }

    private void Update()
    {
        _movement.SetDirection(_input.MoveDirection);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_input.MoveDirection != Vector3.zero)
        {
            return;
        }

        if (other.TryGetComponent<FactoryStorage>(out var storage))
        {
            HandleResourceTransfer(storage);
        }
    }

    private void HandleResourceTransfer(FactoryStorage storage)
    {
        switch (storage.StorageType)
        {
            case StorageType.Consumed:
                if (storage.HasEmptyPlace && _resourcesStack.HasResource(storage.ResourceType))
                {
                    storage.AddResource(_resourcesStack.GetResource(storage.ResourceType));
                }
                break;
            case StorageType.Produced:
                if (_resourcesStack.HasEmptySpace && storage.HasResource)
                {
                    _resourcesStack.AddResource(storage.GetResource());
                }
                break;
            default:
                Debug.LogError($"Unexpected type of storage: {storage.StorageType}");
                break;
        }
    }
}
