using System.Collections;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField]
    private ResourceType _resourceType;

    public ResourceType ResourceType => _resourceType;

    public void MoveTo(Vector3 position, Quaternion rotation, bool convertToLocal = true)
    {
        StopAllCoroutines();
        
        Vector3 targetPosition = convertToLocal ? transform.parent.InverseTransformPoint(position) : position;
        Quaternion targetRotation = convertToLocal ? Quaternion.Inverse(transform.parent.rotation) * rotation : rotation;

        StartCoroutine(MoveToSmoothly(targetPosition, targetRotation, convertToLocal));
    }

    private IEnumerator MoveToSmoothly(Vector3 targetPosition, Quaternion targetRotation, bool convertToLocal)
    {
        float duration = 0.15f;
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }
}