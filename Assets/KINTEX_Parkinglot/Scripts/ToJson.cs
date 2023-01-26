using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEditor.Experimental.GraphView;

namespace KINTEX_Parkinglot.Scripts
{
    public class ToJson : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pillarData;
        private void GetData(List<GameObject> pillarList, List<InfoPillarData> list)
        {
            foreach (var pillar in pillarList)
            {
                Transform[] pList = pillar.GetComponentsInChildren<Transform>();
                foreach (var infoPillar in pList)
                {
                    var pData = new InfoPillarData();
                    if (infoPillar.childCount > 0) continue;
                    //Debug.Log(infoPillar);
                    //Debug.Log(infoPillar.position.x);
                    pData.xPosition = infoPillar.position.x;
                    //Debug.Log(pData.xPosition);
                    pData.zPosition = infoPillar.position.z;
                    pData.yRotation = infoPillar.rotation.y;
                    list.Add(pData);
                }
            }
        }
        void Awake()
        {
            var _infoPillarDataList = new List<InfoPillarData>();
            GetData(pillarData, _infoPillarDataList);
            //Debug.Log(_infoPillarDataList[0].xPosition);
            //Debug.Log(_infoPillarDataList[1].xPosition);
            //Debug.Log(_infoPillarDataList[2].xPosition);
            FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.OpenOrCreate);
            string jsonData = JsonConvert.SerializeObject(_infoPillarDataList, Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }

}
