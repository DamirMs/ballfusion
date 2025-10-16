using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Configs;
using Gameplay.Elements;
using Gameplay.General.Configs;
using Gameplay.IOS.Shop;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ElementsManager : MonoBehaviourEventListener
    {
        [SerializeField] private MergeManager mergeManager;
        [Space]
        [SerializeField] private Element elementPrefab;
        [SerializeField] private Transform elementsParent;

        public List<IElement> ActiveElements => _activeElements;
        public event Action OnDropTurnEnded;
        
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private DropController _dropController;
        [Inject] private ShopManager _shopManager;
        
        private ElementsPool _elementsPool = new();

        private List<IElement> _activeElements = new();
        private IElement _currentDropElement;
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted},
                { GlobalEventEnum.GameEnded, OnGameEnded},
            });
            
            _elementsPool.Init(elementPrefab.gameObject, elementsParent, 6);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _dropController.OnElementDropped += ElementDropped;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            
            _dropController.OnElementDropped -= ElementDropped;
        }

        private void OnGameStarted()
        {
            // foreach (var element in _activeElements)
            // {
            //     if (element != null) Return(element);
            // }
            // // if (_currentDropElement != null) Return(_currentDropElement);
            //
            // _activeElements.Clear();
        }
        private void OnGameEnded()
        {
            while(_activeElements.Count > 0)
            {
                Return(_activeElements[0]);
            }
            // if (_currentDropElement != null) Return(_currentDropElement);
            
            _activeElements.Clear();
        }

        public void Return(IElement element)
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Return element");

            _activeElements.Remove(element);
            _elementsPool.Set(element);
        }
        
        public void SetNextDropElement(ElementData elementData)
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Set next Element");

            _currentDropElement = _elementsPool.Get();
            _currentDropElement.Init(elementData, mergeManager.MergeElements, _shopManager.GetEquippedId(ShopItemEnum.ShopMainItem));

            _activeElements.Add(_currentDropElement);

            _dropController.SetElement(_currentDropElement.Transform);
        }
        
        public bool ElementIsInGame(IElement element) => _activeElements.Contains(element);

        private async void ElementDropped()
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Drop Element");
            
            _currentDropElement.Drop();

            await WaitForElements();
            
            OnDropTurnEnded?.Invoke();
        }

        private async UniTask WaitForElements()
        {
            await UniTask.WaitForSeconds(_gameInfoConfig.DropWaitDelay);
            
            // _activeElements.RemoveAll(ball => ball == null);
            
            // await UniTask.WaitUntil(() => _activeElements.All(element => !element.IsFalling));
        }
    }

    class ElementsPool : PoolBase<IElement>
    {
        protected override IElement CreateObject()
        {
            return GameObject.Instantiate(_prefab, _parent).GetComponent<IElement>();
        }

        protected override void OnGet(IElement obj)
        {
            // obj.SetActive(true);
        }
        protected override void OnSet(IElement obj)
        {
            obj.SetActive(false);
        }
    }
}