using UnityEngine;

namespace uGUIFramework
{
    public class UISpawner : MonoBehaviour
    {
        [SerializeField] private UIPack _blockToSpawn;

        private void Start()
        {
            if (_blockToSpawn == null) return;
            foreach (var settings in _blockToSpawn.GetAllElements())
                settings.SpawnController();
        }
    }
}