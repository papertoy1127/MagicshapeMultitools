using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityModManagerNet;

namespace MagicshapeMultitools {
    public class SimpleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        public UnityEvent @event = new UnityEvent();

        public void OnPointerClick(PointerEventData eventData) {
            @event.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) { }
    }
}