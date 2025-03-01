using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TreeManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ObjectPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LODManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TerrainManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<OptimizationManager>().FromComponentInHierarchy().AsSingle();
    }
}