using System.Collections.Generic;
using Gameplay.Elements;
using Gameplay.Enums;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class ElementsTextsManager : MonoBehaviourEventListener
    {
        [SerializeField] private TextMeshProUGUI textPrefab;
        [SerializeField] private Transform textsParent;

        [Inject] private ElementsManager _elementsManager;

        private MonoBehPool<ElementTextView> _textViewPool = new();
        private Dictionary<IElement, ElementTextView> _activeTextsDict = new(); 
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted},
            });

            _textViewPool.Init(textPrefab.gameObject, textsParent, 5);
        }

        private void OnGameStarted()
        {
            // _activeTextsDict.Clear();
        }

        private void Update()
        {
            RemoveUnusedElements();
            
            MoveTextsToActiveElements();
        }

        private void RemoveUnusedElements()
        {
            var toRemove = new List<IElement>();
            foreach (var pair in _activeTextsDict)
            {
                if (!_elementsManager.ActiveElements.Contains(pair.Key))
                {
                    _textViewPool.Set(pair.Value);
                    toRemove.Add(pair.Key);
                }
            }
            foreach (var key in toRemove) _activeTextsDict.Remove(key);
        }

        private void MoveTextsToActiveElements()
        {
            foreach (var element in _elementsManager.ActiveElements)
            {
                if (element.ElementType == ElementType.Empty) continue;

                if (!_activeTextsDict.ContainsKey(element))
                {
                    var view = _textViewPool.Get();
                    _activeTextsDict[element] = view;
                }

                _activeTextsDict[element].Set(element.ElementType);
                _activeTextsDict[element].transform.position = element.Transform.position;
            }
        }
    }
}