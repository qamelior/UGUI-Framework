using uGUIFramework.UIRoots;
using UnityEngine;
using Zenject;

namespace uGUIFramework
{
    [CreateAssetMenu(menuName = "UI/Zenject Installer", fileName = "(ZI) UI")]
    public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
    {
        [SerializeField] private UIRoot _rootPrefab;
        public override void InstallBindings()
        {
            Container.BindInstance(this).IfNotBound();
            Container.Bind<UIRoot>().FromComponentInNewPrefab(_rootPrefab).AsSingle().NonLazy();
        }
    }
}