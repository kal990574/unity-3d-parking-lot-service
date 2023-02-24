using UnityEngine;
using UnityEngine.EventSystems;

namespace KINTEX_Parkinglot.Scripts
{
    public class MouseDoubleClick : MonoBehaviour, IPointerClickHandler
    {
        // �ش� ��ü ���콺 ����Ŭ�� �� ���� �˾� on
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                UIManager.Instance.ShowAndHideClosePopup(true);
            }
        }
    }
}
