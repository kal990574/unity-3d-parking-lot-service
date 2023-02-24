using UnityEngine;
using UnityEngine.EventSystems;

namespace KINTEX_Parkinglot.Scripts
{
    public class MouseDoubleClick : MonoBehaviour, IPointerClickHandler
    {
        // 해당 객체 마우스 더블클릭 시 종료 팝업 on
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                UIManager.Instance.ShowAndHideClosePopup(true);
            }
        }
    }
}
