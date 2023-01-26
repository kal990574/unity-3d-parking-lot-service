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
        private List<InfoPillarData> _infoPillarDataList;
        private void GetData(List<GameObject> pillarList)
        {
            foreach (var pillar in pillarList)
            {
                var pData = new InfoPillarData();
                Transform[] pList = pillar.GetComponentsInChildren<Transform>();
                foreach (var infoPillar in pList)
                {
                    if (infoPillar.childCount > 0) continue;
                    pData.xPosition = infoPillar.position.x;
                    pData.zPosition = infoPillar.position.z;
                    pData.yRotation = infoPillar.rotation.y;
                    _infoPillarDataList.Add(pData);
                }
            }
        }
        void Start()
        {
            _infoPillarDataList = new List<InfoPillarData>();
            GetData(pillarData);
            Debug.Log(_infoPillarDataList[0].xPosition);
            FileStream stream = new FileStream(Application.dataPath + "/infoPillarData.json", FileMode.OpenOrCreate);
            string jsonData = JsonConvert.SerializeObject(_infoPillarDataList, Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }
}
