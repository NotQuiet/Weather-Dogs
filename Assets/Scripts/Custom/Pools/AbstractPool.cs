using System.Collections.Generic;
using Custom.Factories;
using Unity.Mathematics;
using UnityEngine;

namespace Custom.Pools
{
    public class AbstractPool<T> where T : MonoBehaviour
    {
        private Queue<T> _pool = new();

        private int _poolSize;
        private int _counter;

        public T[] PoolArray => _pool.ToArray();

        public void SetPoolSize(int size)
        {
            _poolSize = size;
        }

        public T GetItem(GenericFactory<T> factory, Transform spawnPosition)
        {
            if (_counter >= _poolSize)
            {
                return ReuseItem();
            }
            else
            {
                _counter++;
                var cell = SpawnItem(factory, spawnPosition);
                return cell;
            }
        }

        private T SpawnItem(GenericFactory<T> factory, Transform spawnPoint)
        {
            var cell = factory.GetNewInstance(spawnPoint);
            _pool.Enqueue(cell);

            return cell;
        }

        protected virtual T ReuseItem()
        {
            T cell = _pool.Dequeue();
            
            _pool.Enqueue(cell);

            cell.gameObject.SetActive(true);
            
            
            cell.transform.localRotation = quaternion.identity;

            return cell;
        }
    }
}