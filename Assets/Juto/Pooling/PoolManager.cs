using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juto
{
    public class PoolManager : MonoBehaviour
    {

        Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        public Pool[] pools;

        private void Start()
        {
            foreach (Pool pool in pools)
            {
                CreatePool(pool.prefab, pool.size);
            }
        }

        /// <summary>
        /// Creates a new pool manager
        /// </summary>
        /// <param name="prefab">prefab to create</param>
        /// <param name="poolSize">How many of the prefabs to be created</param>
        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

                for (int i = 0; i < poolSize; i++)
                {
                    ObjectInstance newObj = new ObjectInstance(Object.Instantiate(prefab));
                    poolDictionary[poolKey].Enqueue(newObj);
                    newObj.SetParent(poolHolder.transform);
                }
            }
        }

        /// <summary>
        /// Reactivates a pool object
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Reactivates a pool object
        /// </summary>
        /// <param name="prefab">Prefab to activate</param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
                poolDictionary[poolKey].Enqueue(objectToReuse);

                return objectToReuse.Reuse(position, rotation);
            }
            else
            {
                Debug.LogError("Couldn't find the pool of " + prefab.name);
                return null;
            }
        }

        /// <summary>
        /// Safe way to destroy a pool object.
        /// </summary>
        /// <param name="obj">The object to destroy</param>
        public void Destroy(GameObject obj)
        {
            Destroy(obj, 0);
        }

        /// <summary>
        /// Safe way to destroy a pool object.
        /// </summary>
        /// <param name="obj">The object to destroy</param>
        /// <param name="time">time delay before getting destroyed</param>
        public void Destroy(GameObject obj, float time)
        {
            StartCoroutine(_Destroy(obj, time));
        }

        private IEnumerator _Destroy(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);

            if (obj.GetComponent<PoolObject>())
            {
                obj.SetActive(false);
            }
            else
            {
                Destroy(obj);
            }

            yield return null;
        }

        public class ObjectInstance
        {
            GameObject gameObject;
            Transform transform;
            bool hasPoolComponent;
            PoolObject poolObject;

            public ObjectInstance(GameObject objectInstance)
            {
                gameObject = objectInstance;
                transform = gameObject.transform;

                if (gameObject.GetComponent<PoolObject>())
                {
                    hasPoolComponent = true;
                    poolObject = gameObject.GetComponent<PoolObject>();
                }

                gameObject.SetActive(false);
            }

            public GameObject Reuse(Vector3 position, Quaternion rotation)
            {
                gameObject.SetActive(true);
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;

                if (hasPoolComponent)
                {
                    poolObject.OnObjectReuse();
                }

                return gameObject;
            }

            public void SetParent(Transform parent)
            {
                transform.SetParent(parent);
            }
        }

        [System.Serializable]
        public struct Pool
        {
            [Tooltip("Just for setting the name in the array. not used.")]
            public string name;
            public GameObject prefab;
            public int size;
        }
    }

}

