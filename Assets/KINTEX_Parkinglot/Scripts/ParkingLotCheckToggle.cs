using UnityEngine;

namespace KINTEX_Parkinglot.Scripts
{
    public class ParkingLotCheckToggle : MonoBehaviour
    {
        // ���� ���� color change
        public void ClickCheckParkingLotToggle(bool isOn)
        {
            UIManager.Instance.ClickCheckParkingLot(isOn);
        }
    }
}
