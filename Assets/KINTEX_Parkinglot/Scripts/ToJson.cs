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
        [SerializeField] private List<GameObject> pillarData;
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

            FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.OpenOrCreate);
            string jsonData = JsonConvert.SerializeObject(infoPillarDataList, Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }

}
