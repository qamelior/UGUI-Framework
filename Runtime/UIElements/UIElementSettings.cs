using NaughtyAttributes;
using uGUIFramework.UIRoots;
using UnityEngine;
using Zenject;

namespace uGUIFramework.UIElements
{
    public abstract class UIElementSettings : ScriptableObject
    {
        private const string SpawnBoxGroupName = "Spawning";

        [field: SerializeField] [field: BoxGroup(SpawnBoxGroupName)]
        private UIStartUpBehaviour _controllerStartUpBehaviour;

        [SerializeField] [BoxGroup(SpawnBoxGroupName)] [ShowIf(nameof(PersistentController))]
        private UIStartUpBehaviour _uiStartUpBehaviour;

        [field: SerializeField]
        [field: BoxGroup(SpawnBoxGroupName)]
        public UIElementRootSettings Root { get; private set; }

        [field: SerializeField]
        [field: BoxGroup(SpawnBoxGroupName)]
        [field: ShowIf(nameof(IsGridSubRoot))]
        public int GridOrder { get; private set; }

        [field: SerializeField]
        [field: BoxGroup(SpawnBoxGroupName)]
        public UIElement Prefab { get; private set; }

        [field: SerializeField]
        [field: BoxGroup(SpawnBoxGroupName)]
        public bool PreserveDuringSceneLoad { get; private set; }

        [Inject] private UIManager _uiManager;
        public bool IsGridSubRoot => Root?.DynamicLayout ?? false;
        public bool PersistentController => _controllerStartUpBehaviour is UIStartUpBehaviour.Spawn;

        public void SpawnController()
        {
            if (!PersistentController) return;
            var controller = _uiManager.GetOrCreateController(this);
            if (_uiStartUpBehaviour is UIStartUpBehaviour.Spawn)
                controller.CreateUI();
        }

        public abstract UIElementController CreateController();

        public abstract void Bind(DiContainer container);
    }
}