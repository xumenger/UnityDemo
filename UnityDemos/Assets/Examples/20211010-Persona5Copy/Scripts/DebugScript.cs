using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace example.y20211010
{
    public class DebugScript : MonoBehaviour
    {
        void Update()
        {
            // 按下【R】后，重新加载场景，也就是重新播放动画
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }

        }
    }

}