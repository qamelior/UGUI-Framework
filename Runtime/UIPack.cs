using System.Collections.Generic;
using System.Collections.ObjectModel;
using NaughtyAttributes;
using uGUIFramework.UIElements;
using UnityEngine;
using Zenject;

namespace uGUIFramework
{
    [CreateAssetMenu(menuName = "UI/Pack", fileName = "(UIP) NewUIPack")]
    public class UIPack : ScriptableObjectInstaller<UIPack>
    {
        [Expandable] [SerializeField] private List<UIElementSettings> _data = new();
        public ReadOnlyCollection<UIElementSettings> GetAllElements() => _data.AsReadOnly();

        public override void InstallBindings()
        {
            foreach (var uiData in _data)
                uiData.Bind(Container);
        }
    }
}