using UnityEngine;

namespace Custom.Factories
{
    public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T prefab;

        public T GetNewInstance(Transform spawnPosition)
        {
            return Instantiate(prefab, spawnPosition);
        }
        
        public void DestroyItem(T cell)
        {
            Destroy(cell.gameObject);
        }
    }
}