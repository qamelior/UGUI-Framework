using System;
using System.Collections.Generic;
using System.Linq;
using uGUIFramework.UIElements;
using uGUIFramework.UIRoots;
using UniRx;
using Object = UnityEngine.Object;

namespace uGUIFramework
{
    public class UIManager
    {
        private readonly ReactiveCollection<UIElementController> _controllers = new();
        private readonly ReactiveCommand<UIElement> _onWindowClosed = new();
        private readonly ReactiveCommand<UIElement> _onWindowOpened = new();
        private readonly List<UIElement> _openedWindows = new();
        private UIRoot _defaultUIRoot;
        private UIElement _windowInFocus;
        public IReadOnlyReactiveCollection<UIElementController> Controllers => _controllers;
        private bool HasOpenWindow => _windowInFocus != null;
        public IObservable<UIElement> OnWindowOpened => _onWindowOpened;
        public IObservable<UIElement> OnWindowClosed => _onWindowClosed;

        private void RegisterNewWindow(UIElement window, UIElementController controller)
        {
            _onWindowOpened.Execute(window);
            if (controller.Settings.Root.Behaviour is UIElementBehaviourType.Overlay) return;
            if (_windowInFocus == null || !controller.HigherPriorityWindows.Contains(_windowInFocus.GetType()))
                _windowInFocus = window;
            _openedWindows.Add(window);
            window.OnClose.Subscribe(ProcessWindowRemoval);
        }

        private void ProcessWindowRemoval(UIElement window)
        {
            _openedWindows.Remove(window);
            _onWindowClosed.Execute(window);
            if (_windowInFocus != window) return;
            _windowInFocus = _openedWindows.Count > 0 ? _openedWindows.Last() : null;
        }

        public UIElement SpawnWindow(UIElementController controller)
        {
            ForceCloseWindow(controller.MutuallyExclusiveWindowType);
            var parent = _defaultUIRoot.GetRoot(controller);
            var window = Object.Instantiate(controller.Prefab, parent);
            RegisterNewWindow(window, controller);
            window.Setup(controller);
            return window;
        }

        private void ForceCloseWindow(IEnumerable<Type> conflictWindows)
        {
            foreach (var conflictWindow in conflictWindows)
                for (var i = _openedWindows.Count - 1; i >= 0; i--)
                    if (_openedWindows[i].GetType() == conflictWindow)
                        _openedWindows[i].Hide();
        }

        public void SetUIRoot(UIRoot uiRoot) => _defaultUIRoot = uiRoot;

        public void RegisterUIController(UIElementController controller)
        {
            _controllers.Add(controller);
            controller.OnDispose += () => _controllers.Remove(controller);
        }

        /// <summary>
        ///     call this only when loading new scene
        /// </summary>
        public void ClearAllUI()
        {
            _openedWindows.Clear();
            _windowInFocus = null;

            for (var i = _controllers.Count - 1; i >= 0; i--)
            {
                if (_controllers[i].Settings.PreserveDuringSceneLoad) continue;
                _controllers[i].Dispose();
            }
        }

        public T GetOpenedWindow<T>() where T : UIElement =>
            _openedWindows.FirstOrDefault(el => el.GetType() == typeof(T)) as T;

        public UIElementController GetOrCreateController(UIElementSettings settings) =>
            _controllers.FirstOrDefault(el => el.Settings == settings) ?? settings.CreateController();

        public T GetOrCreateController<T>(UIElementSettings settings) where T : UIElementController =>
            (_controllers.FirstOrDefault(el => el.Settings == settings) ?? settings.CreateController()) as T;
    }
}