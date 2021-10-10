using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace example.y20211010
{
    public class FinalScreen : MonoBehaviour
    {
        public CanvasGroup canvas;
        public Transform cam;
        public Transform canvasParent;

        [Space]
        [Header("Parameters")]
        public float fadeSpeed = 0.2f;

        // Start is called before the first frame update
        void Start()
        {
            canvas.DOFade(1, fadeSpeed);
            canvasParent.DOShakePosition(fadeSpeed, 300, 30, 90, false, true);
            cam.DOShakePosition(fadeSpeed * 2, 0.5f, 40, 90, false, true);
        }
    }

}