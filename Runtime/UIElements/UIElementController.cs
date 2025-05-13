using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace uGUIFramework.UIElements
{
    public class UIElementController : IDisposable
    {
        private readonly ReactiveProperty<bool> _windowStatus = new();
        private UIElement _window;

        protected UIElementController(UIElementSettings settings, UIManager uiManager)
        {
            Prefab = settings.Prefab;
            Settings = settings;
            UIManager = uiManager;
            UIManager.RegisterUIController(this);
        }

        public IReadOnlyReactiveProperty<bool> WindowStatus => _windowStatus;

        public UIElementSettings Settings { get; }
        public UIManager UIManager { get; }
        public virtual IEnumerable<Type> MutuallyExclusiveWindowType => new Type[] { };
        public virtual IEnumerable<Type> HigherPriorityWindows => new Type[] { };

        public UIElement Prefab { get; }
        public Transform SpawnRoot { get; private set; }

        public void Dispose()
        {
            if (_window != null)
                _window.CloseSilent();
            OnDispose.Invoke();
        }

        private void ProcessParentDisposal()
        {
            HideWindow();
            OnDispose.Invoke();
        }

        public event Action OnDispose = delegate { };

        public void SetParent(UIElementController parent, Transform spawnRoot)
        {
            if (parent == null) return;
            parent.OnDispose += ProcessParentDisposal;
            SetSpawnRoot(spawnRoot);
        }


        protected void SetSpawnRoot(Transform root) => SpawnRoot = root;

        public virtual UIElement CreateUI()
        {
            if (_window != null) return _window;
            _window = UIManager.SpawnWindow(this);
            _window.OnClose.Subscribe(_ => ProcessWindowClosure());
            _windowStatus.Value = true;
            return _window;
        }

        private void ProcessWindowClosure()
        {
            if (Settings.PersistentController)
            {
                _window = null;
                _windowStatus.Value = false;
                return;
            }
            Dispose();
        }

        public void CloseWindowSilent()
        {
            if (_window == null) return;
            _window.CloseSilent();
            _window = null;
            _windowStatus.Value = false;
        }

        public void HideWindow()
        {
            if (_window == null) return;
            _window.Hide();
        }

        public void ToggleWindow(bool isActive)
        {
            if (isActive)
                HideWindow();
            else
                CreateUI();
        }

        protected T SpawnNestedController<T>(UIElementSettings controllerSettings) where T : UIElementController
        {
            var controller = UIManager.GetOrCreateController<T>(controllerSettings);
            controller.SetParent(this, _window.NestedUIRoots[controllerSettings]);
            return controller;
        }
    }
}