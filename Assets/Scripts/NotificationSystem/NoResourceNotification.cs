using System.Collections.Generic;
using System.Text;

public class NoResourceNotification : INotification
{
    private ResourceType _resourceType;
    private List<ResourceType> _missingResources;

    public NoResourceNotification(ResourceType factoryResourceType, List<ResourceType> missingResources)
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