using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinStick
{
    public class TwinStickBulletPool : MonoBehaviour
    {
        public List<GameObject> bullets = new List<GameObject>();

        public GameObject bulletPrefab;

        public int startingBullets;

        private GameObject tmpBullet;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < startingBullets; ++i)
            {
                CreateBullet();
            }
        }

        private GameObject CreateBullet()
        {
            tmpBullet = Instantiate(bulletPrefab);
            tmpBullet.transform.parent = transform;
            bullets.Add(tmpBullet);
            tmpBullet.SetActive(false);
            return tmpBullet;
        }

        public void FireBullet(Vector3 direction, Vector3 pos, Quaternion rotation, float speed, float damage, float range = 5f)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeSelf)
                {
                    FireBullet(bullets[i], direction, pos, rotation, speed, damage, range);
                    return;
                }
            }

            //No inactive bullets, make a new one
            FireBullet(CreateBullet(), direction, pos, rotation, speed, damage, range);
        }

        private void FireBullet(GameObject obj, Vector3 direction, Vector3 pos, Quaternion rotation, float speed,
            float damage, float range)
        {
            obj.transform.position = pos;
            obj.transform.rotation = rotation;
            TwinStickBullet bScript = obj.GetComponent<TwinStickBullet>();

            bScript.normalisedDirection = direction.normalized;
            bScript.damage = damage;
            bScript.speed = speed;
            bScript.bulletLifetime = range;
            obj.SetActive(true);


        }
    }
}