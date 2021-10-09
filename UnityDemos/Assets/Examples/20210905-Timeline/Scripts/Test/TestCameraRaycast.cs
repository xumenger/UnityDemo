using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraRaycast : MonoBehaviour
{
    GameObject wrapper;  // 外层物体 
    GameObject target;   // 内层物体 
    string info = "";    // 碰撞检测信息 

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 当鼠标左键按下时，向鼠标所在的屏幕位置发射一条射线 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 当射线与物体发生碰撞时，在场景视图中绘制射线 
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);

                // 获得第一次碰撞的外层物体对象 
                wrapper = hitInfo.collider.gameObject;

                // 以第一次的碰撞点为起点，沿原来的方向二次发射射线 
                Ray ray2 = new Ray(hitInfo.point, ray.direction);

                RaycastHit hitInfo2;
                if (Physics.Raycast(ray2, out hitInfo2))
                {
                    // 当射线与内层物体碰撞时，在场景中绘制射线 
                    Debug.DrawLine(ray2.origin, ray2.direction, Color.yellow);

                    // 获得内层物体对象 
                    target = hitInfo2.collider.gameObject;

                    // 将外层物体的网格隐藏 
                    wrapper.GetComponent<MeshRenderer>().enabled = false;

                    // 设置碰撞信息 
                    info = "检测到物体: " + target.name + "坐标: " + target.transform.position;
                }
                else
                {
                    // 如果二次发射的射线没有与内层物体碰撞 
                    // 显示外层物体的网格 
                    wrapper.GetComponent<MeshRenderer>().enabled = true;
                    // 设置碰撞信息 
                    info = "检测到物体: " + wrapper.name + "坐标: " + wrapper.transform.position;
                }
            }
        }
    }

    void OnGUI()
    {
        // 在屏幕上打印输出射线检测的信息 

        GUIStyle style = new GUIStyle();
        style.fontSize = 24;

        GUI.Label(new Rect(10, 75, 0, 0), info, style);
    }
}
