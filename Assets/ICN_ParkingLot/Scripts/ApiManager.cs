using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace ICN_ParkingLot.Scripts
{
    public class ApiManager : MonoBehaviour
    {
        public static ApiManager Instance = null;
        
        private const string URI = "https://apis.data.go.kr/B551177/ParkLocationData/getParkLocationData?serviceKey=";
        private const string SERVICE_KEY =
            "N%2F7QUUMOpDfLEjZZ6DoescdQEfhary4twMzw7qPfxYghOGvAHQZc1D8LxsuidQUI2uEWoqSF4W165fVRC0Mp%2BQ%3D%3D";
        private const string PARAMETER_CODE = "&pageNo=1&numOfRows=10000&type=json&terminalid=T1";

        // private const string PARKING_LOT_URI =
        //     "https://apis.data.go.kr/B551177/ParkLocationData/getParkLocationData?serviceKey=N%2F7QUUMOpDfLEjZZ6DoescdQEfhary4twMzw7qPfxYghOGvAHQZc1D8LxsuidQUI2uEWoqSF4W165fVRC0Mp%2BQ%3D%3D&pageNo=1&numOfRows=10000&type=json&terminalid=T1";

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
            const string uri = URI + SERVICE_KEY + PARAMETER_CODE;

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
                    var jObject = JObject.Parse(jsonResult);
                    var jToken = jObject["response"]["body"]["items"];
                    //jToken -> 각각의 parkinglotdata info
                    UIManager.Instance.ClearParkingLotList();
                        
                    foreach (JToken item in jToken)
                    {
                        // Check parkzoneno 지하 1층
                        var parkingZoneNo = item["parkzoneno"]!.ToString();
                        if (parkingZoneNo != "02")
                        {
                            continue;
                        }
                            
                        // Check parkinglanecode
                        var parkingLaneCode = item["parklanecode"]!.ToString();

                        if (parkingLaneCode.Length != 6)
                        {
                            continue;
                        }

                        var checkLineCode = parkingLaneCode.Substring(0, 2);
                        if (checkLineCode == "A1" || checkLineCode == "H1" || checkLineCode == "C1" || checkLineCode == "D1")
                        {
                            var addParkingLotData = new ParkingLotData
                            {
                                ParkingLotNo = item["parklotno"]!.ToString(),
                                ParkingZoneNo = parkingZoneNo,
                                ParkingZoneName = checkLineCode.Substring(0, 1),
                                ParkingLaneCode = item["parklanecode"]!.ToString(),
                                ParkingCarNo = item["parkcarno"]!.ToString(),
                                CarStatus = item["carstatus"]!.ToString(),
                                CarInDate = item["carindate"]!.ToString(),
                                TerminalNo = item["terno"]!.ToString()
                            };
                            UIManager.Instance.AddParkingLotList(addParkingLotData);
                        }
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