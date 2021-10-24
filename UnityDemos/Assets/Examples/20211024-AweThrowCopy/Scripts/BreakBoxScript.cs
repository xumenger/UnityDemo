using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211024
{
    // 放在盒子上的脚本，用于实现当受到碰撞时，盒子破碎
    public class BreakBoxScript : MonoBehaviour
    {

        // 设置一个可破碎的盒子预制体
        public GameObject breakedBox;

        public void Break()
        {
            // 放置在场景中的盒子是正常的盒子，如果发生碰撞则实例化可破碎的盒子预制体，用于实现盒子破裂效果
            GameObject breaked = Instantiate(breakedBox, transform.position, transform.rotation);

            // 获取可破裂盒子的刚体对象
            Rigidbody[] rbs = breaked.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                // 为刚体施加力，实现盒子破裂
                rb.AddExplosionForce(150, transform.position, 30);
            }
            Destroy(gameObject);
        }
    }

}