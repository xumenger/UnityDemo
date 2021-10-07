using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    // 在场景中创建一个空物体
    // 为该空物体添加CameraHolder 脚本组件
    // 将Main Camera 拖到这个空物体下，作为其子物体
    public class CameraHolder : MonoBehaviour
    {
        // 在场景中调整该空物体、Main Camera、角色的相对位置
        // 将角色对象拖到target 属性上去
        public Transform target;
        public float speed = 9;

        public static CameraHolder singleton;

        private void Awake()
        {
            singleton = this;
        }

        // 实现相机追随角色的效果
        private void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 p = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
            transform.position = p;
        }
    }

}