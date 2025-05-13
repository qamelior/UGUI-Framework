using UnityEngine;

namespace uGUIFramework.UIRoots
{
    public class UISubRoot : MonoBehaviour
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public Transform UIElementsSpawnRoot { get; private set; }
    }
}