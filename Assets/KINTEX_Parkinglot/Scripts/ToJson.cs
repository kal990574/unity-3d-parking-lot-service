using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
namespace KINTEX_Parkinglot.Scripts
{
    public class ToJson : MonoBehaviour
    {
        // 기둥 객체 리스트
        [SerializeField] private List<GameObject> pillarData;

        // 리스트에 존재하는 기둥 객체의 position 및 rotation 값을 infoPillarData 클래스의 리스트에 저장
        private void GetData(List<GameObject> pillarList, List<InfoPillarData> list)
        {
            foreach (var pillarSet in pillarList)
            {
                Transform[] infoPillarList = pillarSet.GetComponentsInChildren<Transform>();
                foreach (var infoPillar in infoPillarList)
                {
                    var infoPillarData = new InfoPillarData();

                    if (infoPillar.childCount > 0)
                    {
                        continue;
                    }

                    infoPillarData.xPosition = infoPillar.position.x;
                    infoPillarData.zPosition = infoPillar.position.z;
                    infoPillarData.yRotation = infoPillar.rotation.y;
                    list.Add(infoPillarData);
                }
            }
        }
        void Awake()
        {
            var infoPillarDataList = new List<InfoPillarData>();
            GetData(pillarData, infoPillarDataList);

            // 리스트에 저장된 기둥 객체의 position 및 rotation 값을 Json 파일 형식으로 추출
            FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.OpenOrCreate);
            string jsonData = JsonConvert.SerializeObject(infoPillarDataList, Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }

}
