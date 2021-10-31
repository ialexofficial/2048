using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Models
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(TextMesh))]
    public class CubeBehaviour : MonoBehaviour
    {
        public Color[] cubeColors;
        [NonSerialized] public List<GameObject> CubesToDestroy;
        public bool IsMerged { get; set; }
        public int CubeScore { get; private set; }

        [SerializeField] private ParticleSystem destroyParticlesPrefab;

        private TextMesh _cubeText;
        private MeshRenderer _meshRenderer;
        private DestroyingParticles _destroyingParticles;
        private int _colorNum = 0;

        public void Awake()
        {
            CubeScore = 2; // Standard score
            CubesToDestroy = new List<GameObject>();
        }

        public void Start()
        {
            _cubeText = GetComponentInChildren<TextMesh>();
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_cubeText is null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            IsMerged = false;
            UpdateScoreText();
        }

        public void OnCollisionEnter(Collision other)
        {
            foreach (var t in CubesToDestroy)
            {
                t.GetComponent<MeshRenderer>().material.color = Color.black;
            }
            if (CubesToDestroy.Contains(other.gameObject))
            {
                CubesToDestroy.Remove(other.gameObject);
                other.gameObject.GetComponent<CubeBehaviour>().DestroyCube();
                UpdateScoreText();
            }
        }

        public void TranslateCube(Vector3 aimPosition, float animationTime)
        {
            transform.DOMove(aimPosition, animationTime);
        }

        public void IncreaseScore()
        {
            CubeScore *= 2;
        }

        public void UpdateScoreText()
        {
            if (_cubeText is null)
                return;

            _cubeText.text = CubeScore.ToString();
            ChangeColor();
        }

        public void DestroyCube()
        {
            DestroyingParticles destroyingParticles = Instantiate(
                destroyParticlesPrefab,
                transform.position,
                Quaternion.LookRotation(Vector3.up),
                transform.parent
            )
            .GetComponent<DestroyingParticles>();
            destroyingParticles.ParticlesColor = cubeColors[_colorNum];
            if (_colorNum > 3)
            {
                Debug.Log(destroyingParticles.ParticlesColor);
                Debug.Log(cubeColors[_colorNum]);
            }

            Destroy(gameObject);
        }

        private void ChangeColor()
        {
            _colorNum = (int) Math.Log(CubeScore / 2, 2);

            if (_colorNum > 16)
            {
                _colorNum = 16;
            }

            _meshRenderer.material.color = cubeColors[_colorNum];
            _cubeText.color = _colorNum < 2 ? new Color(0.122f, 0.110f, 0.102f) : Color.white;
        }
    }
}
