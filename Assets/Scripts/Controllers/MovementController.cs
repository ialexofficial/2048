using UnityEngine;
using Models;
using Views;
using System.Threading;
using Components;

namespace Controllers
{
    public class MovementController
    {
        private readonly MovementView _movementView;
        private readonly MenuView _menuView;
        private readonly CellsModel _cellsModel;
        
        public MovementController(MovementView movementView, MenuView menuView, CellsModel model)
        {
            _movementView = movementView;
            _menuView = menuView;
            _cellsModel = model;

            _cellsModel.OnGameStateChange.AddListener(GameStateListener);
            _movementView.AddListener(OnCubesMoving);
        }

        private void OnCubesMoving(MovementDirection direction)
        {
            bool isAnyCubeMoved = false;
            int mergedScore = 0;
            
            if(!_cellsModel.IsInputLocked)
                isAnyCubeMoved = CubesMoving.MoveCells(direction, _cellsModel.Cubes, _cellsModel.CubeBehaviours, _cellsModel.MapSize, _cellsModel.AnimationTime, _cellsModel.MoveDistance, ref mergedScore);

            if (isAnyCubeMoved)
            {
                _cellsModel.OnCubesMoved();
                _menuView.UpdateScore(mergedScore);
            }
        }

        private void GameStateListener(GameState state)
        {
            if (state == GameState.Lose)
            {
                _menuView.GameLose();
            }
            else if (state == GameState.Win)
            {
                _menuView.GameWin();
            }
        }
    }
}