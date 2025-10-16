using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class MovingCheckerController : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayer;
        [Space]
        [SerializeField] private Transform checkerTransform;
        [SerializeField] private float duration = 1;
        [Space]
        [SerializeField] private Transform startPosTransform;
        [SerializeField] private Transform endPosTransform;

        private UniTaskCompletionSource<bool> _checkTcs;
        
        private Tween _moveTween;

        private void Awake()
        {
            if (!checkerTransform) checkerTransform = transform;
        }
        
        private void OnDisable()
        {
            StopCheck();
            checkerTransform.position = startPosTransform.position;
        }

        public async UniTask<bool> StartCheck(CancellationToken token)
        {
            StopCheck(); 
            checkerTransform.position = startPosTransform.position;
    
            _checkTcs = new UniTaskCompletionSource<bool>();

            using (token.Register(() => TrySetCanceledSafe()))
            {
                _moveTween = checkerTransform.DOMove(endPosTransform.position, duration)
                    .OnComplete(() => TrySetResultSafe(false));

                try
                {
                    return await _checkTcs.Task;
                }
                catch (OperationCanceledException)
                {
                    return false;
                }
            }
        }
        private void TrySetResultSafe(bool result)
        {
            if (_checkTcs != null && !_checkTcs.Task.Status.IsCompleted())
            {
                _checkTcs.TrySetResult(result);
                StopCheck();
            }
        }

        private void PlayFound()
        {
            
        }
        
        private void TrySetCanceledSafe()
        {
            if (_checkTcs != null && !_checkTcs.Task.Status.IsCompleted())
            {
                _checkTcs.TrySetCanceled();
                
                StopCheck();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & collisionLayer.value) != 0)
            {
                TrySetResultSafe(true);
                
                PlayFound();
            }
        }

        private void StopCheck()
        {
            _moveTween?.Kill();
            _checkTcs?.TrySetCanceled();
            _checkTcs = null;
        }
    }
}