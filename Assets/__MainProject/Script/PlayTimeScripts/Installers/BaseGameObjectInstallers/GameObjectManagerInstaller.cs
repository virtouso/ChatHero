using UnityEngine;
using Zenject;

public class GameObjectManagerInstaller : MonoInstaller<GameObjectManagerInstaller>
{
    public override void InstallBindings()
    {


        Container.Bind<IGameManager>().FromComponentInParents().AsSingle();
        //   Container.Bind<IMenuManager>().FromComponentInParents().AsSingle();

    }
}