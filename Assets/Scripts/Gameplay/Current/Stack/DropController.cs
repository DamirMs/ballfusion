using System;
using Gameplay.Elements;
using PT.Logic.ProjectContext;
using PT.Tools.EventListener;
using Zenject;
using UnityEngine;

namespace Gameplay
{
    public class DropController : MonoBehaviourEventListener
    {
        [SerializeField] private Transform movementLimitTransformFrom;
        [SerializeField] private Transform movementLimitTransformTo;
        [SerializeField] private Transform initialPositionTransform;
        
        [SerializeField] private BoxCollider2D inputZone; //myb move to new input related
        [SerializeField] private Camera mainCamera; 

        [Inject] private InputManager _inputManager;

        public event Action OnElementDropped;
        public event Action<Vector2> OnElementDragged;

        private Vector2 _movementLimit;
        private float _yOffset;
        private Vector2 _initialPosition;

        private Transform _dropTransform; 
        private bool _isDropping;
        
       private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameMenuOpened, OnGameMenuOpened },
                { GlobalEventEnum.GameMenuClosed, OnGameMenuClosed },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });

            _initialPosition = initialPositionTransform.position;
        }

        private void Start()
        {
            if (!mainCamera) mainCamera = Camera.main;

            _movementLimit = new Vector2(
                movementLimitTransformFrom.position.x,
                movementLimitTransformTo.position.x
            );

            _yOffset = movementLimitTransformFrom.position.y;

            Debug.Log($"[DropController] Movement limits: {_movementLimit.x} → {_movementLimit.y}, Y = {_yOffset}");
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SignOutInput();
        }

        // Called externally when new element is spawned
        public void SetElement(Transform elementTransform)
        {
            _dropTransform = elementTransform;
            _dropTransform.position = _initialPosition;
            _isDropping = true;

            Debug.Log($"[DropController] Set element at {_initialPosition}");
        }

        private void OnGameStarted() => SignUpInput();
        private void OnGameMenuClosed() => SignUpInput();
        private void OnGameMenuOpened() => SignOutInput();
        private void OnGameEnded() => SignOutInput();

        private void SignUpInput()
        {
            _inputManager.OnDrag += Move;
            _inputManager.OnRelease += Drop;
            Debug.Log("[DropController] Listening to input");
        }

        private void SignOutInput()
        {
            _inputManager.OnDrag -= Move;
            _inputManager.OnRelease -= Drop;
            Debug.Log("[DropController] Stopped listening to input");
        }

        private void Move(Vector2 screenPosition)
        {
            if (!_isDropping) return;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(
                new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(mainCamera.transform.position.z))
            );

            if (!InInputZone(worldPos))
                return;

            float clampedX = Utils.AdjustValueBetweenMinMax(_movementLimit.x, _movementLimit.y, worldPos.x);
            Vector2 movePosition = new Vector2(clampedX, _yOffset);

            if (_dropTransform != null)
                _dropTransform.position = movePosition;

            OnElementDragged?.Invoke(movePosition);

            Debug.Log($"[DropController] Dragging → {movePosition}");
        }

        private void Drop(Vector2 screenPosition)
        {
            if (!_isDropping) return;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(
                new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(mainCamera.transform.position.z))
            );

            if (!InInputZone(worldPos))
                return;

            Debug.Log($"[DropController] Dropped element at {worldPos}");

            _dropTransform = null;
            _isDropping = false;

            OnElementDropped?.Invoke();
        }

        private bool InInputZone(Vector2 worldPos)
        {
            bool inZone = inputZone.OverlapPoint(worldPos);
            if (!inZone)
                Debug.Log($"[DropController] Input outside zone: {worldPos}");
            return inZone;
        }
    }
}