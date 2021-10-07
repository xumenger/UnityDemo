using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace example.y20210925
{
    public class DebugRestarter : MonoBehaviour
    {
        // 这个方法什么时候会被调用？
        void OnRestart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}