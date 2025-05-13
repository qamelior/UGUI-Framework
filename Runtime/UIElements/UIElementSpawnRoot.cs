using NaughtyAttributes;
using UnityEngine;

namespace uGUIFramework.UIElements
{
    public class UIElementSpawnRoot : MonoBehaviour
    {
        [field: SerializeField][field: Expandable] public UIElementSettings Data { get; private set; }
    }
}