using UnityEngine;
using UnityEngine.EventSystems;

namespace ICN_ParkingLot.Scripts
{
    public class MouseDoubleClick : MonoBehaviour, IPointerClickHandler
    {
        //���� ��ũ��Ʈ
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                UIManager.Instance.ShowAndHideClosePopup(true);
            }
        }
    }
}
