using System;
using System.Collections;
using UnityEngine;

namespace Models
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyingParticles : MonoBehaviour
    {
        public Color ParticlesColor
        {
            get => _particleSystem.main.startColor.color;
            set
            {
                var main = _particleSystem.main;
                main.startColor = value;
            }
        }
        
        [SerializeField] private float particlesLifetime = 0.5f;
        
        private ParticleSystem _particleSystem;

        public void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Start()
        {
            StartCoroutine(DeleteParticles());
        }

        private IEnumerator DeleteParticles()
        {
            yield return new WaitForSeconds(particlesLifetime);
            
            Destroy(gameObject);
        }
    }
}