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

        private const string URL = "";
        private const int DELAY_TIME = 60;

        void Awake()
        {
            // �̱����� ���� �ϳ��� Instance�� ����
            // Ÿ ��ũ��Ʈ���� �ش� ��ũ��Ʈ�� Instance ���� ����
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
            // Coroutine ����
            StartCoroutine(ParkingLotLoop());
        }
        
        public IEnumerator GetParkingLotData()
        {
            // API ������ ���� ����Ƽ ���� ���� Ŭ������ UnityWebRequest()�� Ȱ��
            const string uri = URL;
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            yield return webRequest.SendWebRequest();
        
            var pages = uri.Split('/');
            var page = pages.Length - 1;
        
            switch (webRequest.result)
            {
                // API ���� ���� �� break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;

                // webresquest success
                case UnityWebRequest.Result.Success:
                    var jsonResult = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    var jObject = JObject.Parse(jsonResult);
                    var jToken = jObject["lists"];

                    // jToken -> ������ parkinglotdata info
                    UIManager.Instance.ClearParkingLotList();
                    UIManager.Instance.ClearParkingSumData();

                    // ��ҿ� �����ϱ� ���� JObject �迭 ��ҿ� ����
                    List<JToken> jList = new List<JToken>();
                    jList.Add(jObject["sum"]["total"]);
                    jList.Add(jObject["sum"]["A"]);
                    jList.Add(jObject["sum"]["B"]);
                    jList.Add(jObject["sum"]["C"]);
                    jList.Add(jObject["sum"]["D"]);
                    jList.Add(jObject["sum"]["E"]);

                    foreach (JToken item in jList)
                    {
                        var addSumData = new ParkingSumData
                        {
                            All = item["all"].ToString(),
                            None = item["none"].ToString(),
                            Free = item["free"].ToString(),
                            In = item["in"].ToString()
                        };

                        UIManager.Instance.AddSumDataList(addSumData);
                    }

                    foreach (JToken item in jToken)
                    {
                            var addParkingLotData = new ParkingLotData
                            {
                                SenserName = item["senser_name"].ToString(),
                                SlotName = item["slot_name"].ToString(),
                                CctvIp = item["cctv_ip"].ToString(),
                                Top = item["top"].ToString(),
                                Left = item["left"].ToString(),
                                Width = item["width"].ToString(),
                                Height = item["height"].ToString(),
                                RecommendLockId = item["recommend_lock_id"].ToString(),
                                RecommendLockDate = item["recommend_lock_date"].ToString(),
                                SlotType = item["slot_type"].ToString(),
                                SlotStatus = item["slot_status"].ToString(),
                                CreatedOn = item["created_on"].ToString(),
                                ModifiedOn = item["modified_on"].ToString(),
                                Uuid = item["uuid"].ToString(),
                                SpotUuid = item["spot_uuid"].ToString(),
                                FloorUuid = item["floor_uuid"].ToString(),
                                LeftPillarUuid = item["left_pillar_uuid"].ToString(),
                                RightPillarUuid = item["right_pillar_uuid"].ToString(),
                                SlotTypeBit = item["slot_type_bit"].ToString(),
                                SlotStatusIndex = item["slot_status_index"].ToString(),
                                SpaceName = item["space_name"].ToString()
                             };

                            UIManager.Instance.AddParkingLotList(addParkingLotData);
                    }
                        
                    UIManager.Instance.SetParkingLot();
                        
                    break;
            }
        }

        private IEnumerator ParkingLotLoop()
        {
            // Coroutine�� ���� DELAY_TIME �� 1���� API ȣ��
            StartCoroutine(GetParkingLotData());
            
            yield return new WaitForSeconds(DELAY_TIME);
            
            StartCoroutine(ParkingLotLoop());
        }
    }
}