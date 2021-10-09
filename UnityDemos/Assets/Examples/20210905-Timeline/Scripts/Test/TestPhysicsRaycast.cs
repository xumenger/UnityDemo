using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysicsRaycast : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // 这里展示发射一个射线，比如你想检测某个范围，可以通过发射多个射线实现
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // print("射线碰撞到了" + hit.collider.gameObject + "物体" + ray.origin + " " + hit.point);

            // 发生碰撞的时候，射线变成红色。DrawLine是给定起点和终点
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // 没有发生碰撞的时候，绿色。DrawRay是给定起点和方向
            Debug.DrawRay(ray.origin, transform.forward * 10, Color.green);
        }
    }
}
