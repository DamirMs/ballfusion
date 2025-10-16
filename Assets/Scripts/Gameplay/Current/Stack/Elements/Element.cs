using System;
using DG.Tweening;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Elements
{
    public class Element : MonoBehaviour, IElement
    {
        [SerializeField] private ElementView view;
        [SerializeField] private CircleCollider2D col;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask collisionLayer;
        
        public Transform Transform => transform;
        public ElementType ElementType => _elementType;
        public bool IsFalling { get { return rb.velocity.magnitude > 0.05f; } }
        
        private Action<IElement, IElement> _mergeAction;
        private ElementType _elementType;

        private void Awake()
        {
            view.Init();
        }
        
        public void Init(ElementData elementData, Action<IElement, IElement> mergeAction, int skinIndex)
        {
            SetData(elementData, skinIndex);
            
            _mergeAction = mergeAction;
            
            gameObject.SetActive(true);
            rb.isKinematic = true;
        }
        public void SetData(ElementData elementData, int skinIndex)
        {
            view.Set(elementData.Sprites[skinIndex], elementData.Color, (int)elementData.ElementType);
            
            _elementType = elementData.ElementType;
        }

        public void Drop() 
        {
            rb.isKinematic = false;
        }
        
        public void Drop(Vector2 direction, float power) 
        {
            rb.isKinematic = false;
            
            rb.AddForce(direction * power, ForceMode2D.Impulse);
        }
        
        public void PlayMergeEffect()
        {
            view.PlayMerge();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);

            if (!active) _elementType = ElementType.Empty;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & collisionLayer.value) != 0)
            {
                _mergeAction?.Invoke(this, collision.gameObject.GetComponent<Element>());
            }
        }
    }
}