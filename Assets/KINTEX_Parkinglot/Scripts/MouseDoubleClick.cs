using UnityEngine;
using UnityEngine.EventSystems;

namespace KINTEX_Parkinglot.Scripts
{
    public class MouseDoubleClick : MonoBehaviour, IPointerClickHandler
    {
        // ���� ��ũ��Ʈ
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                UIManager.Instance.ShowAndHideClosePopup(true);
            }
        }
    }
}
