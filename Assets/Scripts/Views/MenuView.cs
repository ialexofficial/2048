using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Views
{
    public class MenuView : MonoBehaviour
    {
        [NonSerialized] public UnityEvent<bool> OnInputStateChange;
        [NonSerialized] public UnityEvent OnRestartGame;
        [NonSerialized] public UnityEvent OnKeepGoingClick;
        
        [SerializeField] private Image menu;
        [SerializeField] private Image loseMenu;
        [SerializeField] private Image winMenu;
        [SerializeField] private TextMeshProUGUI loseScore;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private float showMenuDelay;
        
        private RectTransform _menuRectTransform;
        private RectTransform _loseMenuTransform;
        private RectTransform _winMenuTransform;
        private int _score;

        public void Awake()
        {
            _score = 0;
            
            OnInputStateChange = new UnityEvent<bool>();
            OnRestartGame = new UnityEvent();
            OnKeepGoingClick = new UnityEvent();
        }
        public void Start()
        {
            _menuRectTransform = menu.GetComponent<RectTransform>();
            _loseMenuTransform = loseMenu.GetComponent<RectTransform>();
            _winMenuTransform = winMenu.GetComponent<RectTransform>();
            
            HideMenus();
        }

        public void ShowMenu()
        {
            _menuRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

            OnInputStateChange?.Invoke(true);
            menu.gameObject.SetActive(true);
        }
        
        public void RestartGame()
        {
            OnRestartGame?.Invoke();
            _score = 0;
            UpdateScoreText();
            HideMenus();
        }
        
        public void UpdateScore(int value)
        {
            _score += value;
            UpdateScoreText();
        }
        
        public void ResumeGame()
        {
            OnInputStateChange?.Invoke(false);
            menu.gameObject.SetActive(false);
        }
        
        public void HideMenu()
        {
            OnInputStateChange?.Invoke(false);
            menu.gameObject.SetActive(false);
        }

        public void KeepGoing()
        {
            OnKeepGoingClick?.Invoke();
            winMenu.gameObject.SetActive(false);
        }
        
        public void GameWin()
        {
            StartCoroutine(GameWinCoroutine());
        }

        public void GameLose()
        {
            StartCoroutine(GameLoseCoroutine());
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
        
        private void UpdateScoreText()
        {
            scoreText.text = "Score:\n" + _score;
        }
        private void HideMenus()
        {
            menu.gameObject.SetActive(false);
            winMenu.gameObject.SetActive(false);
            loseMenu.gameObject.SetActive(false);
        }

        private IEnumerator GameWinCoroutine()
        {
            yield return new WaitForSeconds(showMenuDelay);
            
            _winMenuTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            winMenu.gameObject.SetActive(true);
        }

        private IEnumerator GameLoseCoroutine()
        {
            yield return new WaitForSeconds(showMenuDelay);
            
            loseScore.text = _score.ToString();
            _loseMenuTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            loseMenu.gameObject.SetActive(true);
        }
        
    }
}