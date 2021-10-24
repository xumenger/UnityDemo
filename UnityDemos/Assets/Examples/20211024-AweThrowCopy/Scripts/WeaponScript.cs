using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211024
{
    public class WeaponScript : MonoBehaviour
    {

        public bool activated;

        public float rotationSpeed;

        void Update()
        {

            // 如果斧子正在空中飞，则用这个逻辑实现斧子的旋转
            if (activated)
            {
                transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
            }

        }

        // 武器上加了一个Box Collider，当和其他物体碰撞的时候，触发该函数
        private void OnCollisionEnter(Collision collision)
        {
            // 原来的代码中，这部分是11，因为原来项目中Ground 是11 层，但是我的复刻项目中Ground 是12 层注意修改
            if (collision.gameObject.layer == 12)
            {
                print(collision.gameObject.name);
                GetComponent<Rigidbody>().Sleep();
                GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                GetComponent<Rigidbody>().isKinematic = true;
                activated = false;
            }

        }

        // 用于实现检查是否碰到盒子，触发盒子破裂
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Breakable"))
            {
                if (other.GetComponent<BreakBoxScript>() != null)
                {
                    other.GetComponent<BreakBoxScript>().Break();
                }
            }
        }
    }

}