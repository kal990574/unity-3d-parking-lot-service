using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //
using TMPro; //TMP_Text 활용
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace KINTEX_Parkinglot.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;
        //카메라 움직임
        [SerializeField] private Animator mainCameraAnimator;

        [Space(10)] // 필드 사이 여백
        //전체, A,B,C,D,E 메뉴 버튼
        [SerializeField] private List<Toggle> menuButtonList;

        [Space(10)]
        //전체 주차 가능 공간
        [SerializeField] private TMP_Text availableParkingLotText;

        [Space(10)]
        //sector A 주차 가능 공간
        [SerializeField] private TMP_Text sectionATotalParkingLotText;
        [SerializeField] private TMP_Text sectionAAvailableParkingLotText;
        [Space(10)]
        //sector B 주차 가능 공간
        [SerializeField] private TMP_Text sectionBTotalParkingLotText;
        [SerializeField] private TMP_Text sectionBAvailableParkingLotText;
        [Space(10)]
        //sector C 주차 가능 공간
        [SerializeField] private TMP_Text sectionCTotalParkingLotText;
        [SerializeField] private TMP_Text sectionCAvailableParkingLotText;
        [Space(10)]
        //sector D 주차 가능 공간
        [SerializeField] private TMP_Text sectionDTotalParkingLotText;
        [SerializeField] private TMP_Text sectionDAvailableParkingLotText;
        [Space(10)]
        //sector E 주차 가능 공간
        [SerializeField] private TMP_Text sectionETotalParkingLotText;
        [SerializeField] private TMP_Text sectionEAvailableParkingLotText;

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
        //구역 표현 UI image
        [SerializeField] private List<Image> parkingLotAreaAImageList;
        [SerializeField] private List<Image> parkingLotAreaBImageList;
        [SerializeField] private List<Image> parkingLotAreaCImageList;
        [SerializeField] private List<Image> parkingLotAreaDImageList;
        [SerializeField] private List<Image> parkingLotAreaEImageList;

        [Space(10)]
        //구역의 각 칸 object
        [SerializeField] private List<GameObject> parkingLotAreaA;
        [SerializeField] private List<GameObject> parkingLotAreaB;
        [SerializeField] private List<GameObject> parkingLotAreaC;
        [SerializeField] private List<GameObject> parkingLotAreaD;
        [SerializeField] private List<GameObject> parkingLotAreaE;

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

        private List<ParkingSumData> _parkingSumData;

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
            _parkingSumData = new List<ParkingSumData>();
        }

        public void AddParkingLotList(ParkingLotData parkingLotData)
        {
        //주차 공간 API 활용 추가
            _parkingLotList.Add(parkingLotData);
        }
       
        public void AddSumDataList(ParkingSumData sumdata)
        {
            _parkingSumData.Add(sumdata);
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
            InitializeParkingLot(parkingLotAreaB);
            InitializeParkingLot(parkingLotAreaC);
            InitializeParkingLot(parkingLotAreaD);
            InitializeParkingLot(parkingLotAreaE);
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
            SetParkingLotColor(parkingLotAreaB, isOn);
            SetParkingLotColor(parkingLotAreaC, isOn);
            SetParkingLotColor(parkingLotAreaD, isOn);
            SetParkingLotColor(parkingLotAreaE, isOn);
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
        //수정요망 230109
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
            availableParkingLotText.text = _parkingSumData[0].Free;

            var totalValue = int.Parse(sectionATotalParkingLotText.text);
            var availableValue = int.Parse(_parkingSumData[1].Free);
            var parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionAAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaAImageList[0].color = parkingLotColor;
            parkingLotAreaAImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionBTotalParkingLotText.text);
            availableValue = int.Parse(_parkingSumData[2].Free);
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionBAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaBImageList[0].color = parkingLotColor;
            parkingLotAreaBImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionBTotalParkingLotText.text);
            availableValue = int.Parse(_parkingSumData[3].Free);
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionCAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaCImageList[0].color = parkingLotColor;
            parkingLotAreaCImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionBTotalParkingLotText.text);
            availableValue = int.Parse(_parkingSumData[4].Free);
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionDAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaDImageList[0].color = parkingLotColor;
            parkingLotAreaDImageList[1].color = parkingLotColor;

            totalValue = int.Parse(sectionBTotalParkingLotText.text);
            availableValue = int.Parse(_parkingSumData[5].Free);
            parkingLotColor = SetParkingLotTextColor(totalValue, availableValue);
            sectionEAvailableParkingLotText.text = availableValue.ToString();
            parkingLotAreaEImageList[0].color = parkingLotColor;
            parkingLotAreaEImageList[1].color = parkingLotColor;
        }

        private void AdjustParkingLotData()
        {
        //주차되어 있는 공간에 carmodel 위치 시키기
            foreach (var parkingLot in _parkingLotList)
            {
                if (parkingLot.SlotStatusIndex.Equals("2"))
                {
                    continue;
                }

                if (parkingLot.SlotName.Length < 5)
                {
                    continue;
                }
                //수정요망 ------------230106
                String[] LotNo = parkingLot.SlotName.Split('-');
                var area = int.Parse(LotNo[0]);
                var zoneNo = int.Parse(LotNo[1]);
                var spaceNo = int.Parse(LotNo[2]);

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

        private Transform GetParkingLotSpace(int area, int zoneNo, int spaceNo)
        {
        //area, zoneNo, spaceNo를 통해 주차공간 추출
            List<GameObject> targetParkingLotList = null;

            if (12 <= area && area <= 20)
            {
                targetParkingLotList = parkingLotAreaA;
                area -= 12;
            }
            if (9 <= area && area <= 11)
            {
                targetParkingLotList = parkingLotAreaB;
                area -= 9;
            }
            if (2 <= area && area <= 8)
            {
                targetParkingLotList = parkingLotAreaC;
                area -= 2;
            }
            if (21 <= area && area <= 24 || area == 26)
            {
                targetParkingLotList = parkingLotAreaD;
                if (area == 26) area -= 22;
                else area -= 21;
            }
            if (27 <= area || area == 25)
            {
                targetParkingLotList = parkingLotAreaE;
                if (area == 25) area -= 25;
                else area -= 27;
            }
            return targetParkingLotList[area].GetComponent<Transform>().GetChild(zoneNo - 1).GetComponent<Transform>().GetChild(spaceNo - 1);
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