using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enemy
{
    public class LinearProjectile : Projectile
    {
        private static Dictionary<GameObject, List<LinearProjectile>> pools = new();

        static LinearProjectile()
        {
            SceneManager.sceneLoaded += (_, _) =>
            {
                foreach (var pool in pools.Values)
                {
                    pool.ForEach(x => x.Release());
                }
            };
        }

        public static void Spawn(GameObject prefab,
            Vector3 position, Vector3 direction, Quaternion rotation,
            float speed, float range,
            Func<Collider, bool> collisionCheck)
        {
            if (!pools.TryGetValue(prefab, out var pool))
            {
                pool = new();
                pools.Add(prefab, pool);
            }
            
            var projectile = pool.Find(x => !x.gameObject.activeSelf);
            if (projectile == null)
            {
                projectile = Instantiate(prefab).GetComponent<LinearProjectile>();
                DontDestroyOnLoad(projectile);
                pool.Add(projectile);
            }
            projectile.Init(position, direction, rotation, speed, range, collisionCheck);
        }

        public float Speed;
        public float Range;
        public Vector3 Direction;

        private Vector3 startPos;
        private Func<Collider, bool> isCollided;

        public void Init(Vector3 position, Vector3 direction, Quaternion rotation,
            float speed, float range,
            Func<Collider, bool> collisionCheck)
        {
            transform.SetPositionAndRotation(position, rotation);
            
            Speed = speed;
            Range = range;
            Direction = direction;
            isCollided = collisionCheck;

            startPos = transform.position;
            gameObject.SetActive(true);
        }

        protected override void Move()
        {
            transform.position += Direction * (Speed * Time.deltaTime);
        }

        private void Update()
        {
            Move();
            if (Vector3.SqrMagnitude(startPos - transform.position) > Range * Range)
                Release();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCollided?.Invoke(other) == true)
                Release();
        }

        private void Release()
        {
            gameObject.SetActive(false);
            isCollided = null;
        }
    }
}