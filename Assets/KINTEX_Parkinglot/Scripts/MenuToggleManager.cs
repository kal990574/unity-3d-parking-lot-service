using UnityEngine;

namespace KINTEX_Parkinglot.Scripts
{
    public class MenuToggleManager : MonoBehaviour
    {
        // 메인메뉴 버튼 클릭시 스크립트
        // 버튼에 해당하는 areaNo를 UIManager에 전달
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