using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //
using TMPro; //TMP_Text 활용
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ICN_ParkingLot.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;
        //카메라 움직임
        [SerializeField] private Animator mainCameraAnimator;

        [Space(10)] // 필드 사이 여백
        //전체, A,H,C,D 메뉴 버튼
        [SerializeField] private List<Toggle> menuButtonList;

        [Space(10)]
        //전체 주차 가능 공간
        [SerializeField] private TMP_Text availableParkingLotText;

        [Space(10)]
        //sector A 주차 가능 공간
        [SerializeField] private TMP_Text sectionATotalParkingLotText;
        [SerializeField] private TMP_Text sectionAAvailableParkingLotText;
        [Space(10)]
        //sector H 주차 가능 공간
        [SerializeField] private TMP_Text sectionHTotalParkingLotText;
        [SerializeField] private TMP_Text sectionHAvailableParkingLotText;
        [Space(10)]
        //sector C 주차 가능 공간
        [SerializeField] private TMP_Text sectionCTotalParkingLotText;
        [SerializeField] private TMP_Text sectionCAvailableParkingLotText;
        [Space(10)]
        //sector D 주차 가능 공간
        [SerializeField] private TMP_Text sectionDTotalParkingLotText;
        [SerializeField] private TMP_Text sectionDAvailableParkingLotText;

        [Space(10)]
        //시간 갱신
        [SerializeField] private TMP_Text refreshTimeText;

        [Space(10)]
        //종료 팝업
        [SerializeField] private GameObject closePopup;

        [Space(10)]
        //주차 가능 색 change toggle
        [SerializeField] private Toggle CheckParkingToggle;

        [Space(10)]
        //A,H,C,D 구역 표현 UI image
        [SerializeField] private List<Image> parkingLotAreaAImageList;
        [SerializeField] private List<Image> parkingLotAreaHImageList;
        [SerializeField] private List<Image> parkingLotAreaCImageList;
        [SerializeField] private List<Image> parkingLotAreaDImageList;

        [Space(10)]
        //A,H,C,D 구역의 각 칸 object
        [SerializeField] private List<GameObject> parkingLotAreaA;
        [SerializeField] private List<GameObject> parkingLotAreaH;
        [SerializeField] private List<GameObject> parkingLotAreaC;
        [SerializeField] private List<GameObject> parkingLotAreaD;

        [Space(10)]
        //상태 표시 material
        [SerializeField] private Material parkingInitMaterial;
        [SerializeField] private Material parkingPossibleMaterial;
        [SerializeField] private Material parkingImpossibleMaterial;
        [SerializeField] private Material parkingPossibleHandicapMaterial;
        [SerializeField] private Material parkingImpossibleHandicapMaterial;

        [Space(10)]
        //8 color cars
        [SerializeField] private List<GameObject> carModelList;

        private List<ParkingLotData> _parkingLotList;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
        //주차 공간 리스트
            _parkingLotList = new List<ParkingLotData>();
        }

        public void AddParkingLotList(ParkingLotData parkingLotData)
        {
        //주차 공간 API 활용 추가
            _parkingLotList.Add(parkingLotData);
        }

        public void ClearParkingLotList()
        {
        //주차 공간 초기화
            _parkingLotList.Clear();
        }

        public void SetParkingLot()
        {
        //전체 주차 공간 setting
            //각 주차 구역 초기화
            InitializeParkingLot(parkingLotAreaA);
            InitializeParkingLot(parkingLotAreaH);
            InitializeParkingLot(parkingLotAreaC);
            InitializeParkingLot(parkingLotAreaD);
            //주차장 현황 UI 세팅
            SetParkingLotInformationText();
            //시간 초기화
            SetRefreshTime();
            //주차되어 있는 공간에 carmodel 위치 시키기
            AdjustParkingLotData();

            if (CheckParkingToggle.isOn)
            {
                ClickCheckParkingLot(false);
                ClickCheckParkingLot(true);
            }
        }

        public void ClickAreaButton(int value)
        {
        //animator 속 parameter -> parkingareaCamera 값을 value로 설정 -> transition이 value에 따라 작동
            mainCameraAnimator.SetInteger("ParkingAreaCamera", value);

            StartCoroutine(WaitMenuAnimatorPlay());
        }

        public void ClickRefreshButton()
        {
        //refresh button click 시
            StartCoroutine(ApiManager.Instance.GetParkingLotData());
        }

        public void ClickCheckParkingLot(bool isOn)
        {
        //주차 가능 공간 color change
            SetParkingLotColor(parkingLotAreaA, isOn);
            SetParkingLotColor(parkingLotAreaH, isOn);
            SetParkingLotColor(parkingLotAreaC, isOn);
            SetParkingLotColor(parkingLotAreaD, isOn);
        }

        public void ShowAndHideClosePopup(bool value)
        {
        //종료 명령시 종료팝업 abled
            closePopup.SetActive(value);
        }

        public bool GetParkingLotAvailableToggleValue()
        {
        //주차 가능 색 토글값 리턴
            return CheckParkingToggle.isOn;
        }

        public void ExitProgram()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void InitializeParkingLot(List<GameObject> areaList)
        {
        //각 주차 구역 초기화
            foreach (var area in areaList)
            {
                foreach (Transform space in area.transform)
                {
                    if (space.name == area.name)
                    {
                        continue;
                    }
                    //HandicapZone color 구별
                    var parkingLotColor = parkingInitMaterial;

                    if (space.CompareTag("HandicapZone"))
                    {
                        parkingLotColor = parkingPossibleHandicapMaterial;
                    }

                    space.GetComponent<MeshRenderer>().material = parkingLotColor;

                    // remove Car model
                    if (space.childCount > 0)
                    {
                        Destroy(space.GetChild(0).gameObject);
                    }
                }
            }
        }

        private void SetParkingLotInformationText()
        {
        //주차장 현황 UI 세팅
            availableParkingLotText.text = _parkingLotList.Count(data => data.CarStatus.Equals("N")).ToString();

            var totalValue = int.Parse(sectionATotalParkingLotText.text);
            var availableValue = _parkingLotList.Count(u => u.ParkingZoneName.Equals("A") && u.CarStatus.Equals("N"));
            var parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionAAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaAImageList[0].color = parkingLotColor;
            parkingLotAreaAImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionHTotalParkingLotText.text);
            availableValue = _parkingLotList.Count(u => u.ParkingZoneName.Equals("H") && u.CarStatus.Equals("N"));
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionHAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaHImageList[0].color = parkingLotColor;
            parkingLotAreaHImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionCTotalParkingLotText.text);
            availableValue = _parkingLotList.Count(u => u.ParkingZoneName.Equals("C") && u.CarStatus.Equals("N"));
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionCAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaCImageList[0].color = parkingLotColor;
            parkingLotAreaCImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionDTotalParkingLotText.text);
            availableValue = _parkingLotList.Count(u => u.ParkingZoneName.Equals("D") && u.CarStatus.Equals("N"));
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionDAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaDImageList[0].color = parkingLotColor;
            parkingLotAreaDImageList[1].color = parkingLotColor;
        }

        private void AdjustParkingLotData()
        {
        //주차되어 있는 공간에 carmodel 위치 시키기
            foreach (var parkingLot in _parkingLotList)
            {
                if (parkingLot.CarStatus.Equals("N"))
                {
                    continue;
                }

                if (parkingLot.ParkingLaneCode.Length != 6)
                {
                    continue;
                }

                var area = parkingLot.ParkingLaneCode.Substring(0, 1);
                var zoneNo = int.Parse(parkingLot.ParkingLaneCode.Substring(2, 1));
                var spaceNo = int.Parse(parkingLot.ParkingLaneCode.Substring(3, 3));

                var targetParkingLot = GetParkingLotSpace(area, zoneNo, spaceNo);
                if (targetParkingLot.CompareTag("HandicapZone"))
                {
                    targetParkingLot.GetComponent<MeshRenderer>().material = parkingImpossibleHandicapMaterial;
                }
                
                var carNo = Random.Range(0, 7);
                var carModel = Instantiate(carModelList[carNo], targetParkingLot.position, targetParkingLot.rotation);
                carModel.transform.Rotate(0f, 90f, 0f);
                carModel.transform.parent = targetParkingLot;
            }
        }

        private Transform GetParkingLotSpace(string area, int zoneNo, int spaceNo)
        {
        //area, zoneNo, spaceNo를 통해 주차공간 추출
            List<GameObject> targetParkingLotList;

            switch (area)
            {
                case "A":
                    targetParkingLotList = parkingLotAreaA;
                    break;
                case "H":
                    targetParkingLotList = parkingLotAreaH;
                    break;
                case "C":
                    targetParkingLotList = parkingLotAreaC;
                    break;
                case "D":
                    targetParkingLotList = parkingLotAreaD;
                    break;
                default:
                    return null;
            }

            return targetParkingLotList[zoneNo].GetComponent<Transform>().GetChild(spaceNo - 1);
        }

        private void SetRefreshTime()
        {
        //현재시각 
            refreshTimeText.text = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }

        private void EnableAndDisableMenuButton(bool value)
        {
            foreach (var button in menuButtonList)
            {
                button.interactable = value;
            }
        }

        private IEnumerator WaitMenuAnimatorPlay()
        {
        //카메라 move 시 메뉴버튼 비활성화
            EnableAndDisableMenuButton(false);

            yield return new WaitForSeconds(3);

            EnableAndDisableMenuButton(true);
        }

        private Color SetParkingLotTextColor(int totalValue, int availableValue)
        {
        //잔여 주차 지역의 text color 변경
            var percentage = (availableValue / (float)totalValue) * 100;
            Color parkingLotColor;

            switch (percentage)
            {
                case >= 31.0f:
                    ColorUtility.TryParseHtmlString("#65EB8A", out parkingLotColor);
                    break;
                case > 3.0f:
                    ColorUtility.TryParseHtmlString("#FFD954", out parkingLotColor);
                    break;
                default:
                    ColorUtility.TryParseHtmlString("#EB5151", out parkingLotColor);
                    break;
            }

            return parkingLotColor;
        }

        private void SetParkingLotColor(List<GameObject> parkingLot, bool isOn)
        {
        //주차 가능 공간 색 바꾸기
            var colorMaterial = isOn ? parkingPossibleMaterial : parkingInitMaterial;

            foreach (var area in parkingLot)
            {
                foreach (Transform space in area.transform)
                {
                    if (space.name == area.name)
                    {
                        continue;
                    }

                    if (space.CompareTag("HandicapZone"))
                    {
                        continue;
                    }

                    if (space.childCount > 0)
                    {
                        continue;
                    }

                    space.GetComponent<MeshRenderer>().material = colorMaterial;
                }
            }
        }
    }
}