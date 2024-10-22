using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerBehaviour _playerPrefab;

    public override void InstallBindings()
    {
        BindNotificationSystem();
        BindInput();
        InstallPlayer();
    }

    private void InstallPlayer()
    {
        var player = Container.InstantiatePrefabForComponent<PlayerBehaviour>(_playerPrefab);
        Container.Bind<PlayerBehaviour>().FromInstance(player).AsCached().NonLazy();
    }

    private void BindInput()
    {
        Container.Bind<IInput>()
         .To<JoystickInput>()
         .FromComponentInHierarchy()
         .AsSingle()
         .NonLazy();
    }

    private void BindNotificationSystem()
    {
        Container.Bind<NotificationSystem>()
         .FromComponentInHierarchy()
         .AsSingle()
         .NonLazy();
    }
}
