using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HanoiTowerTwo : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform selectedDisc;
    private RectTransform initialParent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Disc"))
        {
            selectedDisc = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
            initialParent = selectedDisc.parent.GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (selectedDisc != null)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0f;
            selectedDisc.position = newPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (selectedDisc != null)
        {
            GameObject dropZone = eventData.pointerCurrentRaycast.gameObject;
            if (dropZone != null && dropZone.CompareTag("Peg") && IsMoveValid(dropZone))
            {
                selectedDisc.SetParent(dropZone.transform, false);
            }

            else
            {
                selectedDisc.SetParent(initialParent, true);
                selectedDisc.anchoredPosition = Vector2.zero;
            }

            selectedDisc = null;
        }
    }

    private bool IsMoveValid(GameObject targetPeg)
    {
        RectTransform[] discsInPeg = targetPeg.GetComponentsInChildren<RectTransform>();
        if (discsInPeg.Length == 0)
            return true;

        RectTransform topDisc = discsInPeg[discsInPeg.Length - 1];
        return selectedDisc.sizeDelta.x < topDisc.sizeDelta.x;
    }
}