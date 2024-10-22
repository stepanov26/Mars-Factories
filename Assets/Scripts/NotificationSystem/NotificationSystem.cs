using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private Animator _notificationAnimator;
    [SerializeField] private float _notificationDuration = 3f;

    private Queue<INotification> _notificationQueue = new Queue<INotification>();
    private bool _isNotificationPlaying = false;

    public void ShowNotification(INotification notification)
    {
        if (_isNotificationPlaying)
        {
            _notificationQueue.Enqueue(notification);
        }
        else
        {
            StartCoroutine(ShowNotificationCoroutine(notification));
        }
    }

    private IEnumerator ShowNotificationCoroutine(INotification notification)
    {
        _isNotificationPlaying = true;
        _notificationText.text = notification.GetNotificationText();
        _notificationAnimator.SetTrigger("Show");

        yield return new WaitForSeconds(_notificationDuration);

        _notificationAnimator.SetTrigger("Hide");

        yield return new WaitForSeconds(0.5f);

        _isNotificationPlaying = false;

        if (_notificationQueue.TryDequeue(out var nextNotification))
        {
            StartCoroutine(ShowNotificationCoroutine(nextNotification));
        }
    }
}
