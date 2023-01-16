using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParkingLot : MonoBehaviour
{
    [SerializeField] private GameObject Lot;
    [Space(10)]
    [SerializeField] private List<GameObject> parkingLotAreaA;
    [SerializeField] private List<GameObject> parkingLotAreaB;
    [SerializeField] private List<GameObject> parkingLotAreaC;
    [SerializeField] private List<GameObject> parkingLotAreaD;
    [SerializeField] private List<GameObject> parkingLotAreaE;
    void Start()
    {
        var xPosition = 5142.49f;
        var zPosition = 3582.25f;
        var yRotation = 79.33f;
        var parkingLot = Instantiate(Lot);
        parkingLot.transform.position = new Vector3(xPosition - 8500, 1f, 6000 - zPosition);
        parkingLot.transform.Rotate(0f, yRotation, 0f);
    }
}
