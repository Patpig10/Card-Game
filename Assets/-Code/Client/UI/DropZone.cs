using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Game.Client
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter ( PointerEventData eventData )
        {
            //Debug.Log("OnPointerEnter");
            if( eventData.pointerDrag==null )
                return;

            if( eventData.pointerDrag.TryGetComponent<Draggable>(out var draggable) )
            {
                draggable.DropZone_PointerEnter( transform );
            }
        }

        public void OnPointerExit ( PointerEventData eventData )
        {
            //Debug.Log("OnPointerExit");
            if( eventData.pointerDrag==null )
                return;

            if( eventData.pointerDrag.TryGetComponent<Draggable>(out var draggable) )
            {
                draggable.DropZone_PointerExit( transform );
            }
        }

        public void OnDrop ( PointerEventData eventData )
        {
            Debug.Log($"{eventData.pointerDrag.name} was dropped on {gameObject.name}");
            if( eventData.pointerDrag.TryGetComponent<Draggable>(out var draggable) )
            {
                draggable.DropZone_Drop( transform );
            }
        }
    }
}
