using UnityEngine;

[CreateAssetMenu(fileName = "FactorySettings", menuName = "Game/FactorySettings")]
public class FactorySettings : ScriptableObject
{
    public ResourceItem ResourceItem;
    public float ProductionInterval;
}
