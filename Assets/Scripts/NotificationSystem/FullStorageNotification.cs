public class FullStorageNotification : INotification
{
    private ResourceType _resourceType;

    public FullStorageNotification(ResourceType resourceType)
    {
        _resourceType = resourceType;
    }

    public string GetNotificationText()
    {
        return $"The factory of <b>{_resourceType}</b> has stopped production. Storage is full.";
    }
}