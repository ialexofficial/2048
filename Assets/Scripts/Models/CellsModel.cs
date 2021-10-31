using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Components;
using UnityEngine.Events;

namespace Models
{
    public class CellsModel : MonoBehaviour
    {

        public List<int> EmptyCells => _emptyCells;
        public GameObject[,] Cubes => _cubes;
        public CubeBehaviour[,] CubeBehaviours => _cubeBehaviours;
        public int MapSize => mapSize;
        public float AnimationTime => animationTime;
        public float MoveDistance => moveDistance;
        
        public bool IsInputLocked { get; set; } = false;

        [NonSerialized] public UnityEvent<GameState> OnGameStateChange;
        
        [SerializeField] private float animationTime = 0.5f;
        [SerializeField] private float stepDistance = 0.125f;
        [SerializeField] private float moveDistance = 1f;
        [SerializeField] private int mapSize = 4;
        [SerializeField] private GameObject cubePrefab;
        
        
        private List<int> _emptyCells;
        private GameObject[,] _cubes;
        private CubeBehaviour[,] _cubeBehaviours;
        private GameState _gameState;

        public void Awake()
        {
            _gameState = GameState.Play;
            OnGameStateChange = new UnityEvent<GameState>();
        }
        
        public void Start()
        {
            InitializeGameObjects();
        }

        public void OnRestartGame()
        {
            foreach (GameObject cube in _cubes)
            {
                Destroy(cube);
            }

            InitializeGameObjects();
            IsInputLocked = false;
        }

        public void OnCubesMoved()
        {
            StartCoroutine(UpdateAfterMoving());
        }
        
        public void UpdateEmptyCells()
        {
            _emptyCells = (
                from i in Enumerable.Range(0, mapSize)
                from j in Enumerable.Range(0, mapSize)
                where _cubes[i, j] is null
                select i * mapSize + j
            ).ToList();
        }
        
        
        private IEnumerator UpdateAfterMoving()
        {
            IsInputLocked = true;
            
            yield return new WaitForSeconds(animationTime);
            
            UpdateEmptyCells();

            CubeGenerator.GenerateCube(_emptyCells, _cubes, _cubeBehaviours, cubePrefab, transform, stepDistance, mapSize);

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                { 
                    if (!(_cubeBehaviours[i, j] is null))
                    {
                        _cubeBehaviours[i, j].IsMerged = false;
                    }
                }
            }

            IsInputLocked = false;
            GameState state = GameStateChecker.CheckGameState(CubeBehaviours, MapSize);

            if (state != _gameState)
            {
                _gameState = state;
                OnGameStateChange?.Invoke(_gameState);
            }
            
        }
        private void InitializeGameObjects()
        {
            _emptyCells = Enumerable.Range(0, mapSize).ToList();
            _cubes = new GameObject[mapSize, mapSize];
            _cubeBehaviours = new CubeBehaviour[mapSize, mapSize];

            CubeGenerator.GenerateCube(_emptyCells, _cubes, _cubeBehaviours, cubePrefab, transform, stepDistance, mapSize);
            CubeGenerator.GenerateCube(_emptyCells, _cubes, _cubeBehaviours, cubePrefab, transform, stepDistance, mapSize);
        }
    }
}