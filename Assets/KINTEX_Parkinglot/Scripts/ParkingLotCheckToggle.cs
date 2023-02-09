using UnityEngine;

namespace KINTEX_Parkinglot.Scripts
{
    public class ParkingLotCheckToggle : MonoBehaviour
    {
        // 주차 가능 color change
        public void ClickCheckParkingLotToggle(bool isOn)
        {
            UIManager.Instance.ClickCheckParkingLot(isOn);
        }
    }
}
