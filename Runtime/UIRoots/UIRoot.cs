using System.Collections.Generic;
using uGUIFramework.UIElements;
using UnityEngine;
using Zenject;

namespace uGUIFramework.UIRoots
{
    public class UIRoot : MonoBehaviour
    {
        private readonly Dictionary<UIElementRootSettings, Transform> _subRoots = new();

        [Inject]
        public void Init(UIManager uiManager) => uiManager.SetUIRoot(this);

        public Transform GetRoot(UIElementController controller) => controller.Settings.Root.Type switch
        {
            UIElementRootSettings.RootType.Default => GetDefaultRoot(),
            UIElementRootSettings.RootType.Custom => controller.SpawnRoot,
            UIElementRootSettings.RootType.SubRoot => _subRoots.TryGetValue(controller.Settings.Root, out var existingSubRoot)
                ? existingSubRoot
                : SpawnSubRoot(controller.Settings.Root),
            _ => null,
        };

        public Transform GetDefaultRoot() => transform;

        private Transform SpawnSubRoot(UIElementRootSettings subRootSettings)
        {
            var newSubRoot = Instantiate(subRootSettings.Prefab, transform);
            newSubRoot.name = subRootSettings.name;
            newSubRoot.Canvas.overrideSorting = subRootSettings.OverrideSorting;
            if (subRootSettings.OverrideSorting)
                newSubRoot.Canvas.sortingOrder = subRootSettings.SortingOrder;
            _subRoots.Add(subRootSettings, newSubRoot.transform);
            return newSubRoot.UIElementsSpawnRoot;
        }
    }
}