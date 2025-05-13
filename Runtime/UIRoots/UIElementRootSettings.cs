using NaughtyAttributes;
using uGUIFramework.UIElements;
using UnityEngine;

namespace uGUIFramework.UIRoots
{
    [CreateAssetMenu(fileName = "(UIRoot) NewUIRoot", menuName = "UI/Root Settings")]
    public class UIElementRootSettings : ScriptableObject
    {
        public enum RootType
        {
            Default,
            Custom,
            SubRoot,
        }

        [field: SerializeField] public RootType Type { get; private set; }

        [field: SerializeField]
        [field: ShowIf(nameof(IsSubRoot))]
        public UISubRoot Prefab { get; private set; }

        [field: SerializeField]
        [field: AllowNesting]
        [field: ShowIf(nameof(IsNotDefault))]
        public bool OverrideSorting { get; private set; }

        [field: SerializeField]
        [field: AllowNesting]
        [field: ShowIf(nameof(OverrideSorting))]
        public int SortingOrder { get; private set; }

        [field: SerializeField] public bool DynamicLayout { get; private set; }

        [field: SerializeField]
        [field: BoxGroup("UI Element Behaviour")]
        public UIElementBehaviourType Behaviour { get; private set; }

        private bool IsNotDefault => Type is not RootType.Default;
        private bool IsSubRoot => Type is RootType.SubRoot;
    }
}