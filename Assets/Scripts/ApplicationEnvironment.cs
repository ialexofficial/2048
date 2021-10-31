using System;
using UnityEngine;
using Views;
using Controllers;
using Models;

public enum MovementDirection
{
    Top,
    Right,
    Bottom,
    Left
}
public enum GameState
{
    Play,
    Win,
    Lose
}

[RequireComponent(typeof(MovementView))]
[RequireComponent(typeof(MenuView))]
public class ApplicationEnvironment : MonoBehaviour
{

    [SerializeField] private CellsModel cellsModel;

    private MovementController _movementController;
    private MovementView _movementView;
    private MenuController _menuController;
    private MenuView _menuView;

    public void Start()
    {
        _movementView = GetComponent<MovementView>();
        _menuView = GetComponent<MenuView>();
        
        _movementController = new MovementController(_movementView, _menuView, cellsModel);
        _menuController = new MenuController(_menuView, cellsModel);
    }
    
}