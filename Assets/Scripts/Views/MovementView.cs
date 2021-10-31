using UnityEngine;
using UnityEngine.Events;
using System;


namespace Views
{
    public class MovementView : MonoBehaviour
    {
        [SerializeField] private float minTouchMagnitude = 1f;
        
        private UnityEvent<MovementDirection> _onCubesMoving;

        private Vector2 _touchStartPosition;

        public void AddListener(UnityAction<MovementDirection> listener)
        {
            _onCubesMoving.AddListener(listener);
        }

        public void Awake()
        {
            _onCubesMoving = new UnityEvent<MovementDirection>();
        }

        public void Update()
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        _touchStartPosition = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        CheckTouchMoving(touch.position - _touchStartPosition);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    _onCubesMoving?.Invoke(MovementDirection.Bottom);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    _onCubesMoving?.Invoke(MovementDirection.Left);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    _onCubesMoving?.Invoke(MovementDirection.Top);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _onCubesMoving?.Invoke(MovementDirection.Right);
                }
            }
        }

        private void CheckTouchMoving(Vector2 magnitude)
        {
            float deltaX = magnitude.x;
            float deltaY = magnitude.y;
            
            if (Math.Max(Math.Abs(deltaX), Math.Abs(deltaY)) < minTouchMagnitude)
                return;

            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                _onCubesMoving.Invoke(deltaX > 0 ? MovementDirection.Right : MovementDirection.Left);
            }

            if (Math.Abs(deltaX) < Math.Abs(deltaY))
            {
                _onCubesMoving.Invoke(deltaY > 0 ? MovementDirection.Top : MovementDirection.Bottom);
            }
        }
    }
}
