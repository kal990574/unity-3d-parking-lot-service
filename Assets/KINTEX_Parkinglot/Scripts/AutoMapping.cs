using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace KINTEX_Parkinglot.Scripts
{
    public class AutoMapping : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private GameObject Lot;
        [SerializeField] private GameObject Pillar;
        [SerializeField] private GameObject PillarParent;
        [SerializeField] private GameObject HandicapParent;
        [SerializeField] private Material parkingInitHandicapMaterial;
        [SerializeField] private Material parkingBaseHandicapMaterial;

        [Space(10)]
        [SerializeField] private List<GameObject> parkingLotAreaA;
        [SerializeField] private List<GameObject> parkingLotAreaB;
        [SerializeField] private List<GameObject> parkingLotAreaC;
        [SerializeField] private List<GameObject> parkingLotAreaD;
        [SerializeField] private List<GameObject> parkingLotAreaE;

        private List<InfoParkingLots> _infoParkingLotsList;

        private List<InfoPillarData> _infoPillarDataList;

        private int listCnt = 0;

        private static float s_mapSizeX = 420f;

        private static float s_mapSizeZ = 300f;

        private void AddInfoParkingLots(InfoParkingLots infoParkinglots)
        {
            _infoParkingLotsList.Add(infoParkinglots);
        }

        private void AddInfoPillarData(InfoPillarData infoPillarData)
        {
            _infoPillarDataList.Add(infoPillarData);
        }

        private void SetInfoParkingLots()
        {
            foreach(var lot in _infoParkingLotsList)
            {
                string[] LotNo = lot.Name.Split('-');
                var area = int.Parse(LotNo[0]);
                var zoneNo = int.Parse(LotNo[1]);
                var spaceNo = int.Parse(LotNo[2]);

                var parkingLot = Instantiate(Lot);
                var parkingLotParent = GetParkingLotSpace(area, zoneNo, spaceNo);

                var xPosition = float.Parse(lot.Left) / 20;
                var zPosition = float.Parse(lot.Top) / 20;
                var yRotation = float.Parse(lot.Rotate);

                var typeName = lot.Type;

                if (typeName.Equals("handicap"))
                { 
                    parkingLot.tag = "HandicapZone";
                    parkingLot.GetComponent<MeshRenderer>().material = parkingInitHandicapMaterial;
                    parkingLot.transform.localScale = new Vector3(0.5f, 1f, 0.8f);

                    var baseLot = Instantiate(Lot);
                    baseLot.transform.position = new Vector3(xPosition - s_mapSizeX, 0.09f, s_mapSizeZ - zPosition);
                    baseLot.transform.Rotate(0f, yRotation, 0f);
                    baseLot.transform.parent = HandicapParent.transform;
                    baseLot.GetComponent<MeshRenderer>().material = parkingBaseHandicapMaterial;
                }

                parkingLot.transform.position = new Vector3(xPosition - s_mapSizeX, 0.1f, s_mapSizeZ - zPosition);
                parkingLot.transform.Rotate(0f, yRotation, 0f);
                parkingLot.transform.parent = parkingLotParent;
            }

            foreach(var pillar in _infoPillarDataList)
            {
                var pillarspace = Instantiate(Pillar);
                pillarspace.transform.position = new Vector3(pillar.xPosition, 0, pillar.zPosition);
                pillarspace.transform.rotation = Quaternion.Euler(0, pillar.yRotation, 0);
                pillarspace.transform.parent = PillarParent.transform;
            }
        }
        private Transform GetParkingLotSpace(int area, int zoneNo, int spaceNo)
        {
            // area, zoneNo, spaceNo를 통해 주차공간 추출
            List<GameObject> targetParkingLotList = null;

            if (2 <= area && area <= 8)
            {
                targetParkingLotList = parkingLotAreaC;
                area -= 2;
            }

            if (9 <= area && area <= 11)
            {
             targetParkingLotList = parkingLotAreaB;
             area -= 9;
            }

            if (12 <= area && area <= 20)
            {
                targetParkingLotList = parkingLotAreaA;
                area -= 12;
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
                else area -= 26;
            }

            var childTransform = targetParkingLotList[area].transform.GetChild(zoneNo - 1);
            return childTransform;
        }
        void Awake()
        {
            _infoParkingLotsList = new List<InfoParkingLots>();
            _infoPillarDataList = new List<InfoPillarData>();  

            var loadedLots = Resources.Load<TextAsset>("infoParkingLots");
            var loadedPillars = Resources.Load<TextAsset>("infoPillar");

            var jObject = JObject.Parse(loadedLots.ToString()).Children();
            var jArray = JArray.Parse(loadedPillars.ToString());

            List<JToken> jTokenLot = jObject.Children().ToList();
            List<JToken> jTokenPillar = jArray.Children().ToList();

            foreach (var item in jTokenLot)
            {
                var addInfoParkingLots = new InfoParkingLots
                {
                    Type = item["type"].ToString(),
                    Left = item["left"].ToString(),
                    Top = item["top"].ToString(),
                    Rotate = item["rotate"].ToString(),
                    SideNode = item["sideNode"].ToString(),
                    ShowString = item["showString"].ToString()
                };

                AddInfoParkingLots(addInfoParkingLots);
            }

            foreach (var item in jTokenPillar)
            {
                var addInfoPillarData = new InfoPillarData
                {
                    xPosition = float.Parse(item["xPosition"].ToString()),
                    zPosition = float.Parse(item["zPosition"].ToString()),
                    yRotation = float.Parse(item["yRotation"].ToString())
                };

                AddInfoPillarData(addInfoPillarData);
            }

            var jName = JObject.Parse(loadedLots.ToString());

            foreach (var item in (JToken)jName)
            {
                JProperty jProperty = item.ToObject<JProperty>();
                _infoParkingLotsList[listCnt].Name = jProperty.Name.ToString();
                listCnt += 1;
            }
            SetInfoParkingLots();
        }
    }
}
