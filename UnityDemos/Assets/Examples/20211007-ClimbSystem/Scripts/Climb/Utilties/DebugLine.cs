using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    public class DebugLine : MonoBehaviour
    {
        public int maxRenderers;

        // https://www.cnblogs.com/driftingclouds/p/6442847.html
        // 关于渲染部分的知识点还是不懂的太多了
        List<LineRenderer> lines = new List<LineRenderer>();

        private void Start()
        {
            
        }

        void CreateLine(int i)
        {
            GameObject go = new GameObject();
            lines.Add(go.AddComponent<LineRenderer>());
            lines[i].widthMultiplier = 0.05f;
        }

        public void SetLine(Vector3 startpos, Vector3 endpos, int index)
        {
            if (index > lines.Count)
            {
                CreateLine(index);
            }

            lines[index].SetPosition(0, startpos);
            lines[index].SetPosition(1, endpos);
        }


        public static DebugLine singleton;
    }
}