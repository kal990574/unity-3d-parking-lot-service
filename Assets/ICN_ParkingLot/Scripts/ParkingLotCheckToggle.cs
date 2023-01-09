using UnityEngine;

namespace ICN_ParkingLot.Scripts
{
    public class ParkingLotCheckToggle : MonoBehaviour
    {
    //주차 가능 color change
        public void ClickCheckParkingLotToggle(bool isOn)
        {
            UIManager.Instance.ClickCheckParkingLot(isOn);
        }
    }
}
