using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesStack : MonoBehaviour
{
    [SerializeField] private int _capacity;
    [SerializeField] private int _itemsPerRow;
    [SerializeField] private float _heightOffset;
    [SerializeField] private float _rowOffset;
    [SerializeField] private Transform _stackBase;

    public bool HasEmptySpace => _resourceStack.Count < _capacity;

    private readonly List<ResourceItem> _resourceStack = new();

    public void AddResource(ResourceItem resource)
    {
        resource.transform.SetParent(transform);
        _resourceStack.Add(resource);

        var targetPosition = CalculateResourcePosition(_resourceStack.Count - 1);
        resource.MoveTo(targetPosition, Quaternion.identity, convertToLocal: false);
    }
    
    public bool HasResource(ResourceType resource)
    {
        return _resourceStack.Any(x => x.ResourceType == resource);
    }

    public ResourceItem GetResource(ResourceType resourceType)
    {
        var resource = _resourceStack.FindLast(x => x.ResourceType == resourceType);
        if (resource != null)
        {
            _resourceStack.Remove(resource);
            RecalculateStackPositions();
        }

        return resource;
    }

    private Vector3 CalculateResourcePosition(int index)
    {
        int row = index / _itemsPerRow; 
        int positionInRow = index % _itemsPerRow;

        return new Vector3(0, positionInRow * _heightOffset, row * _rowOffset);
    }

    private void RecalculateStackPositions()
    {
        for (int i = 0; i < _resourceStack.Count; i++)
        {
            Vector3 newPosition = CalculateResourcePosition(i);
            _resourceStack[i].transform.localPosition = newPosition;
        }
    }
}
