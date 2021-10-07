using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace example.y20210925
{
    // 关于粒子系统，还是有很多不明白的地方
    public class ParticleSystemScript : MonoBehaviour
    {
        private ParticleSystem particle;
        private ParticleSystemRenderer particleRender;
        private Material particleMaterial;

        private void Start()
        {
            particle = GetComponent<ParticleSystem>();
            particleRender = GetComponent<ParticleSystemRenderer>();
            particleMaterial = particleRender.material;
        }

        public void PlayParticleAtPosition(Vector3 pos)
        {
            var pmain = particle.main;

            transform.position = pos;
            particle.Play();
            particleMaterial.DOFade(1, 0);
            particleMaterial.DOFade(0, pmain.startLifetime.constant).SetEase(Ease.InExpo);
        }
    }
}