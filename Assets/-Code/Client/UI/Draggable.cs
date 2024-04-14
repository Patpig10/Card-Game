using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using NaughtyAttributes;

namespace Game.Client
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Transform _parentToReturnTo;
        Transform _placeholderParent;
        Transform _placeholder;

        public void OnBeginDrag ( PointerEventData eventData )
        {
            Debug.Log($"{nameof(Draggable)}.OnBeginDrag()",this);

            GameObject placeholderGO = new GameObject($"{nameof(Draggable)} {nameof(_placeholder)} #{this.GetInstanceID()}");
            var placeholderLayoutElement = placeholderGO.AddComponent<LayoutElement>();
            _placeholder = placeholderGO.transform;
            _placeholder.SetParent( transform.parent );

            var thisLayoutElement = GetComponent<LayoutElement>();
            placeholderLayoutElement.preferredWidth = thisLayoutElement.preferredWidth;
            placeholderLayoutElement.preferredHeight = thisLayoutElement.preferredHeight;
            placeholderLayoutElement.flexibleWidth = 0;
            placeholderLayoutElement.flexibleHeight = 0;

            _placeholder.SetSiblingIndex( transform.GetSiblingIndex() );

            _parentToReturnTo = transform.parent;
            _placeholderParent = _parentToReturnTo;
            transform.SetParent( transform.parent.parent );

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag ( PointerEventData eventData )
        {
            //Debug.Log($"{nameof(Draggable)}.OnDrag()",this);

            transform.position = eventData.position;

            if( _placeholder.parent!=_placeholderParent )
                _placeholder.SetParent( _placeholderParent );

            int newSiblingIndex = _placeholderParent.childCount;

            for( int i=0 ; i<_placeholderParent.childCount ; i++ )
            {
                if( transform.position.x<_placeholderParent.GetChild(i).position.x )
                {
                    newSiblingIndex = i;

                    if( _placeholder.GetSiblingIndex()<newSiblingIndex )
                        newSiblingIndex--;

                    break;
                }
            }

            _placeholder.SetSiblingIndex( newSiblingIndex );
        }

        public void OnEndDrag ( PointerEventData eventData )
        {
            Debug.Log($"{nameof(Draggable)}.OnEndDrag()",this);

            transform.SetParent( _parentToReturnTo );
            transform.SetSiblingIndex( _placeholder.GetSiblingIndex() );
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy( _placeholder.gameObject );
        }

        public void DropZone_PointerEnter ( Transform dropZone )
        {
            _placeholderParent = dropZone;
        }

        public void DropZone_PointerExit ( Transform dropZone )
        {
            if( _placeholderParent==dropZone )
            {
                _placeholderParent = _parentToReturnTo;
            }
        }

        public void DropZone_Drop ( Transform dropZone )
        {
            _parentToReturnTo = dropZone;
        }

    }
}
