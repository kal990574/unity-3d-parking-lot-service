using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace KINTEX_Parkinglot.Scripts
{
    public class CreateParkingLot : MonoBehaviour
    {
        [SerializeField] private GameObject Lot;
        [Space(10)]
        [SerializeField] private List<GameObject> parkingLotAreaA;
        [SerializeField] private List<GameObject> parkingLotAreaB;
        [SerializeField] private List<GameObject> parkingLotAreaC;
        [SerializeField] private List<GameObject> parkingLotAreaD;
        [SerializeField] private List<GameObject> parkingLotAreaE;
        private List<InfoParkingLots> _infoParkingLotsList;
        public void AddInfoParkingLots(InfoParkingLots infoParkinglots)
        {
            _infoParkingLotsList.Add(infoParkinglots);
        }
        public void SetInfoParkingLots()
        {
            foreach(var lot in _infoParkingLotsList)
            {
               /* string[] LotNo = lot.Name.Split('-');
                var area = int.Parse(LotNo[0]);
                var zoneNo = int.Parse(LotNo[1]);
                var spaceNo = int.Parse(LotNo[2]);*/
                var parkingLot = Instantiate(Lot);
                var xPosition = float.Parse(lot.Left);
                var zPosition = float.Parse(lot.Top);
                var yRotation = float.Parse(lot.Rotate);
                parkingLot.transform.position = new Vector3(xPosition - 8500, 1f, 6000 - zPosition);
                parkingLot.transform.Rotate(0f, yRotation, 0f);
            }
        }
        void Start()
        {
            _infoParkingLotsList = new List<InfoParkingLots>();
            var loadedJson = Resources.Load<TextAsset>("infoParkingLots");
            var j = JObject.Parse(loadedJson.ToString()).Children();
            Debug.Log(j.ToString());
            List<JToken> tokens = j.Children().ToList();
            foreach (var item in tokens)
            {
                //JProperty jProperty = item.ToObject<JProperty>();
                //Debug.Log(item.Value<string>());
                Debug.Log(item);
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
            SetInfoParkingLots();
        }
    }
}
