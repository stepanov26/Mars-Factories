using System.Collections;
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

    private List<ResourceItem> _resourceStack = new List<ResourceItem>();

    public void AddResource(ResourceItem resource)
    {
        resource.transform.SetParent(transform);
        _resourceStack.Add(resource);

        PlaceResourceSmoothly(resource, CalculateResourcePosition(_resourceStack.Count - 1));
    }

    public bool HasResource(ResourceType resource)
    {
        return _resourceStack.Any(x => x.ResourceType == resource);
    }

    public ResourceItem GetResource(ResourceType resourceType)
    {
        if (_resourceStack.Any(x => x.ResourceType == resourceType))
        {
            var resource = _resourceStack.FindLast(x => x.ResourceType == resourceType);
            _resourceStack.Remove(resource);
            RecalculateStackPositions();
            return resource;
        }

        return null;
    }

    private void PlaceResourceSmoothly(ResourceItem resource, Vector3 targetPosition)
    {
        StartCoroutine(SmoothMovement(resource, targetPosition));
    }

    private IEnumerator SmoothMovement(ResourceItem resource, Vector3 targetPosition)
    {
        float duration = 0.15f;
        Vector3 startPosition = resource.transform.localPosition;
        Quaternion startRotation = resource.transform.localRotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            resource.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            resource.transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, time / duration);
            yield return null;
        }

        resource.transform.localPosition = targetPosition;
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
