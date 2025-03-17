using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationService : MonoBehaviour
{
    private const string SHOW_TRIGGER_NAME = "Show";
    private const string HIDE_TRIGGER_NAME = "Hide";
    
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private Animator _notificationAnimator;
    [SerializeField] private float _notificationDuration = 3f;
    
    private readonly Queue<INotification> _notificationQueue = new();
    private bool _isNotificationPlaying = false;

    public void ShowNotification(INotification notification)
    {
        if (_isNotificationPlaying)
        {
            _notificationQueue.Enqueue(notification);
            return;
        }
        
        StartCoroutine(ShowNotificationCoroutine(notification));
    }

    private IEnumerator ShowNotificationCoroutine(INotification notification)
    {
        _isNotificationPlaying = true;
        _notificationText.text = notification.GetNotificationText();
        _notificationAnimator.SetTrigger(SHOW_TRIGGER_NAME);

        yield return new WaitForSeconds(_notificationDuration);

        _notificationAnimator.SetTrigger(HIDE_TRIGGER_NAME);

        yield return new WaitForSeconds(0.5f);

        _isNotificationPlaying = false;

        if (_notificationQueue.TryDequeue(out var nextNotification))
        {
            StartCoroutine(ShowNotificationCoroutine(nextNotification));
        }
    }
}
