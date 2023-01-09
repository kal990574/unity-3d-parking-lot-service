using UnityEngine;

namespace ICN_ParkingLot.Scripts
{
    public class MenuToggleManager : MonoBehaviour
    {
        //���θ޴� ��ư Ŭ���� ��ũ��Ʈ
        [SerializeField] private int areaNo;

        public void ClickToggle(bool isOn)
        {
            if (isOn)
            {
                UIManager.Instance.ClickAreaButton(areaNo);
            }
        }
    }
}