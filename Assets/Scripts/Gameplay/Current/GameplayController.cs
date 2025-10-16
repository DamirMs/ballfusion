using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Elements.NextElement;
using Gameplay.Enums;
using Gameplay.General.Game;
using Gameplay.General.Score;
using UnityEngine;
using Zenject;

namespace Gameplay.Current
{
    public class GameplayController : LevelGameplayController
    {
        [SerializeField] private MovingCheckerController movingCheckerController;
        [SerializeField] private MergeManager mergeManager;
        
        [Inject] private ElementsManager _elementsManager;
        [Inject] private NextElementManager _nextElementManager;
        [Inject] private ScoreManager _scoreManager;

        protected override async void SignUp()
        {
            _elementsManager.OnDropTurnEnded += DroppedGameTurn;
            mergeManager.OnMerge += OnMerge;

            // Let game settle before first element
            await UniTask.DelayFrame(1);
            
            _elementsManager.SetNextDropElement(_nextElementManager.GetNext());
        }
        
        protected override void SignOut()
        {
            _elementsManager.OnDropTurnEnded -= DroppedGameTurn;
            mergeManager.OnMerge -= OnMerge;
        }

        private void DroppedGameTurn()
        {
            GameTurn(); // triggers async sequence in base
        }

        protected override async UniTask OnGameTurn(CancellationToken token)
        {
            // NOTE: Base handles _turnCts; don't create new CTS here.

            bool hasFoundElement = false;
            try
            {
                hasFoundElement = await movingCheckerController.StartCheck(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (hasFoundElement)
            {
                GameOver(); // handled async by base
            }
            else
            {
                _elementsManager.SetNextDropElement(_nextElementManager.GetNext());
            }
        }
        
        private void OnMerge(ElementType type)
        {
            _scoreManager.UpdateScore((int)type);
        }
    }
}
