using System;
using Cysharp.Threading.Tasks.Triggers;
using PT.Tools.Effects;
using PT.Tools.ObjectPool;
using UnityEngine;

namespace PT.GameplayAdditional.Effects
{
    [Serializable]
    public class EffectsSpawner
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        [SerializeField] private int amount = 10;
        
        private ParticleSystemPool _effectsPool = new();

        public void Init()
        {
            _effectsPool.Init(prefab, parent, amount);
        }

        public void Spawn(Vector2 pos)
        {
            var effect = _effectsPool.Get();
            effect.GetComponent<PoolEffectHandler>().Init(_effectsPool.Set);
            effect.transform.position = pos;
        }
    }
}