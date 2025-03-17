using System.Collections.Generic;
using System.Text;

public class NoResourceNotification : INotification
{
    private readonly ResourceType _resourceType;
    private readonly IEnumerable<ResourceType> _missingResources;

    public NoResourceNotification(ResourceType factoryResourceType, IEnumerable<ResourceType> missingResources)
    {
        _resourceType = factoryResourceType;
        _missingResources = missingResources;
    }

    public string GetNotificationText()
    {
        var stringBuilder = new StringBuilder();
        var missingResourcesText = stringBuilder.AppendJoin(",", _missingResources);
        return $"The factory of <b>{_resourceType}</b> has stopped production. There is no resource: <b>{missingResourcesText}</b>.";
    }
}