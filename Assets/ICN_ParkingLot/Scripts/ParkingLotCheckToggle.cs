using UnityEngine;

namespace ICN_ParkingLot.Scripts
{
    public class ParkingLotCheckToggle : MonoBehaviour
    {
    //���� ���� color change
        public void ClickCheckParkingLotToggle(bool isOn)
        {
            UIManager.Instance.ClickCheckParkingLot(isOn);
        }
    }
}
