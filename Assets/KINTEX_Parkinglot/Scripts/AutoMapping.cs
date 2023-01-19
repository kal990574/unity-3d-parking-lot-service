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
        [SerializeField] private GameObject Lot;

        [Space(10)]
        [SerializeField] private List<GameObject> parkingLotAreaA;
        [SerializeField] private List<GameObject> parkingLotAreaB;
        [SerializeField] private List<GameObject> parkingLotAreaC;
        [SerializeField] private List<GameObject> parkingLotAreaD;
        [SerializeField] private List<GameObject> parkingLotAreaE;

        [Space(10)]
        //8 color cars
        [SerializeField] private List<GameObject> carModelList;
        private List<InfoParkingLots> _infoParkingLotsList;
        private int listCnt = 0;
        private float getX = 420f;
        private float getZ = 300f;
        private void AddInfoParkingLots(InfoParkingLots infoParkinglots)
        {
            _infoParkingLotsList.Add(infoParkinglots);
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
                //Debug.Log(spaceNo);
                //Debug.Log(targetParkingLot);
                var xPosition = float.Parse(lot.Left) / 20;
                var zPosition = float.Parse(lot.Top) / 20; ;
                var yRotation = float.Parse(lot.Rotate);
                parkingLot.transform.position = new Vector3(xPosition - getX, 0.1f, getZ - zPosition);
                parkingLot.transform.Rotate(0f, yRotation, 0f);
                parkingLot.transform.parent = parkingLotParent;
            }
        }
        private Transform GetParkingLotSpace(int area, int zoneNo, int spaceNo)
        {
            //area, zoneNo, spaceNo를 통해 주차공간 추출
            List<GameObject> targetParkingLotList = null;

            if (2 <= area && area <= 8)
            {
                targetParkingLotList = parkingLotAreaC;
                area -= 2;
            }
            if (9 <= area && area <= 11)
            {
                //특이 케이스
                /*if (area == 10 && zoneNo == 2 && spaceNo > 22)
                {
                    targetParkingLotList = parkingLotAreaC;
                    area = 7;
                    zoneNo -= 1;
                    spaceNo -= 22;
                }*/
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
            var tmp1 = targetParkingLotList[area].transform.GetChild(zoneNo - 1);
            return tmp1;
        }
        void Awake()
        {
            _infoParkingLotsList = new List<InfoParkingLots>();
            var loadedJson = Resources.Load<TextAsset>("infoParkingLots");
            var j = JObject.Parse(loadedJson.ToString()).Children();
            Debug.Log(j.Children().ToList());
            List<JToken> tokens = j.Children().ToList();
            foreach (var item in tokens)
            {
                //JProperty jProperty = item.ToObject<JProperty>();
                //Debug.Log(item.Value<string>());
                //Debug.Log(item.Parent.First);
                var addInfoParkingLots = new InfoParkingLots
                {
                    //Name = jProperty.Name.ToString(),
                    Type = item["type"].ToString(),
                    Left = item["left"].ToString(),
                    Top = item["top"].ToString(),
                    Rotate = item["rotate"].ToString(),
                    SideNode = item["sideNode"].ToString(),
                    ShowString = item["showString"].ToString()
                };
                //Debug.Log(addSumData.In);
                AddInfoParkingLots(addInfoParkingLots);
            }
            var jobject = JObject.Parse(loadedJson.ToString());
            foreach (var item in (JToken)jobject)
            {
                JProperty jProperty = item.ToObject<JProperty>();
                _infoParkingLotsList[listCnt].Name = jProperty.Name.ToString();
                listCnt += 1;
            }
            SetInfoParkingLots();
        }
    }
}
