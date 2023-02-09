using UnityEngine;
using UnityEngine.EventSystems;

namespace KINTEX_Parkinglot.Scripts
{
    public class MouseDoubleClick : MonoBehaviour, IPointerClickHandler
    {
        // 종료 스크립트
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                UIManager.Instance.ShowAndHideClosePopup(true);
            }
        }
    }
}
