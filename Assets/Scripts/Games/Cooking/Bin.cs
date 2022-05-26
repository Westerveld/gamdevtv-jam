using UnityEngine;

namespace Cooking
{
    public class Bin : InteractArea
    {
        public Vector3 rotation = new Vector3(12f, 0f, 12f);
        private float timer = 0f;

        public override void Setup(CookingController p)
        {
            base.Setup(p);
        }

        public override bool PlaceItem(GameObject item)
        {
            Destroy(item);
            timer = 3f;
            return true;
        }

        public override GameObject GetItem(Transform parent)
        {
            return null;
        }

        void FixedUpdate()
        {
            if (timer > 0f)
            {
                timer -= Time.fixedDeltaTime;
                transform.rotation = Quaternion.Euler(new Vector3(Random.Range(-rotation.x, rotation.x),0f, Random.Range(-rotation.z, rotation.z)));
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }
        
    }
}