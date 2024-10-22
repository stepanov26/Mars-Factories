using UnityEngine;
using Zenject;

public class PlayerBehaviour : MonoBehaviour
{ 
    [SerializeField]
    private IMovement _movement;

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
        _movement = GetComponent<IMovement>();
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
                if (storage.HasEmptyPlace && _resourcesStack.HasResource(storage.Resource))
                {
                    storage.AddResource(_resourcesStack.GetResource(storage.Resource));
                }
                break;
            case StorageType.Produced:
                if (storage.HasResource && _resourcesStack.HasEmptySpace)
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
