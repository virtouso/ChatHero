using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller<SignalInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
 
        Container.DeclareSignal<ScoreUpdated>();
        Container.BindSignal<ScoreUpdated>().ToMethod<IGameManager>((x, y) => x.AddtoScore(y.Score)).FromResolve();
    }
}