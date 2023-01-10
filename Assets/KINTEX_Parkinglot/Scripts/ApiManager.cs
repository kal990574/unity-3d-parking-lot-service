using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace KINTEX_Parkinglot.Scripts
{
    public class ApiManager : MonoBehaviour
    {
        public static ApiManager Instance = null;
        
        private const string URL = "https://kintex.watchmile.com/api/v1/parking/slot";

        // private const string PARKING_LOT_URI =
        //     "https://kintex.watchmile.com/api/v1/parking/slot"

        private const int DELAY_TIME = 60;

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
        
        void Start()
        {
        //코루틴 시작
            StartCoroutine(ParkingLotLoop());
        }
        
        public IEnumerator GetParkingLotData()
        {
            //API 연동
            const string uri = URL;
            Debug.Log(uri);
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            yield return webRequest.SendWebRequest();
        
            var pages = uri.Split('/');
            var page = pages.Length - 1;
        
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                //webresquest success
                case UnityWebRequest.Result.Success:
                    var jsonResult = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    Debug.Log(jsonResult);
                    var jObject = JObject.Parse(jsonResult);
                    Debug.Log(jObject);
                    var jToken = jObject["lists"];
                    //jToken -> 각각의 parkinglotdata info
                    Debug.Log(jToken);
                    UIManager.Instance.ClearParkingLotList();
                    UIManager.Instance.ClearParkingSumData();
                    Debug.Log(jObject["sum"]);
                    // 이부분 수정해야함 -> 230106
                    // 노가다
                    List<JToken> jlist = new List<JToken>();
                    jlist.Add(jObject["sum"]["total"]);
                    jlist.Add(jObject["sum"]["A"]);
                    jlist.Add(jObject["sum"]["B"]);
                    jlist.Add(jObject["sum"]["C"]);
                    jlist.Add(jObject["sum"]["D"]);
                    jlist.Add(jObject["sum"]["E"]);
                    foreach (JToken item in jlist)
                    {
                        var addSumData = new ParkingSumData
                        {
                            All = item["all"].ToString(),
                            None = item["none"].ToString(),
                            Free = item["free"].ToString(),
                            In = item["in"].ToString()
                        };
                        Debug.Log(addSumData.In);
                        UIManager.Instance.AddSumDataList(addSumData);
                    }
                    foreach (JToken item in jToken)
                    {
                        Debug.Log(item);
                            var addParkingLotData = new ParkingLotData
                            {
                                SenserName = item["senser_name"]!.ToString(),
                                SlotName = item["slot_name"]!.ToString(),
                                CctvIp = item["cctv_ip"]!.ToString(),
                                Top = item["top"]!.ToString(),
                                Left = item["left"]!.ToString(),
                                Width = item["width"]!.ToString(),
                                Height = item["height"]!.ToString(),
                                RecommendLockId = item["recommend_lock_id"]!.ToString(),
                                RecommendLockDate = item["recommend_lock_date"]!.ToString(),
                                SlotType = item["slot_type"]!.ToString(),
                                SlotStatus = item["slot_status"]!.ToString(),
                                CreatedOn = item["created_on"]!.ToString(),
                                ModifiedOn = item["modified_on"]!.ToString(),
                                Uuid = item["uuid"]!.ToString(),
                                SpotUuid = item["spot_uuid"]!.ToString(),
                                FloorUuid = item["floor_uuid"]!.ToString(),
                                LeftPillarUuid = item["left_pillar_uuid"]!.ToString(),
                                RightPillarUuid = item["right_pillar_uuid"]!.ToString(),
                                SlotTypeBit = item["slot_type_bit"]!.ToString(),
                                SlotStatusIndex = item["slot_status_index"]!.ToString(),
                                SpaceName = item["space_name"]!.ToString()
                             };
                            UIManager.Instance.AddParkingLotList(addParkingLotData);
                    }
                        
                    UIManager.Instance.SetParkingLot();
                        
                    break;
            }
        }

        private IEnumerator ParkingLotLoop()
        {
        //코루틴을 통해 DELAY_TIME 당 1번씩 API 호출
            StartCoroutine(GetParkingLotData());
            
            yield return new WaitForSeconds(DELAY_TIME);
            
            StartCoroutine(ParkingLotLoop());
        }
    }
}