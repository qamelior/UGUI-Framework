using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace uGUIFramework.UIElements
{
    public abstract class UIElement : MonoBehaviour
    {
        private readonly Dictionary<UIElementSettings, Transform> _nestedUIRoots = new();
        private readonly ReactiveCommand<UIElement> _onClose = new();
        public IReadOnlyDictionary<UIElementSettings, Transform> NestedUIRoots => _nestedUIRoots;
        public IObservable<UIElement> OnClose => _onClose;

        public virtual void Setup(UIElementController interfaceController)
        {
            foreach (var nestedRoot in GetComponentsInChildren<UIElementSpawnRoot>())
                _nestedUIRoots.Add(nestedRoot.Data, nestedRoot.transform);
        }

        public virtual void Hide()
        {
            _onClose.Execute(this);
            CloseSilent();
        }

        public void CloseSilent() => Destroy(gameObject);
    }
}