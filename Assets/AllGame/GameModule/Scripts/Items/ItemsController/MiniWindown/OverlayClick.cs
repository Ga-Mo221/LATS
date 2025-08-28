using UnityEngine;
using UnityEngine.EventSystems;

public class OverlayClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
