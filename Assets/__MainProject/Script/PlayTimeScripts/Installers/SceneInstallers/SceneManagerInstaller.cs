using UnityEngine;
using Zenject;

public class SceneManagerInstaller : MonoInstaller<SceneManagerInstaller>
{
    public override void InstallBindings()
    {

        Container.Bind<IGameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IMenuManager>().FromComponentInHierarchy().AsSingle();

    }
}