using Components;
using Views;
using Models;
using UnityEngine;

namespace Controllers
{
    public class MenuController
    {
        private MenuView _menuView;
        private CellsModel _cellsModel;

        public MenuController(MenuView view, CellsModel model)
        {
            _menuView = view;
            _cellsModel = model;
            
            _menuView.OnRestartGame.AddListener(_cellsModel.OnRestartGame);
            _menuView.OnInputStateChange.AddListener((bool state) => _cellsModel.IsInputLocked = state);
            _menuView.OnKeepGoingClick.AddListener(() =>
            {
                _cellsModel.IsInputLocked = false;
                GameStateChecker.IsKeepGoing = true;
            });
        }
    }
}