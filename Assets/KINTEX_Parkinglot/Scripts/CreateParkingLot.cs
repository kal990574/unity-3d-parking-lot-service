using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParkingLot : MonoBehaviour
{
    [SerializeField] private GameObject Lot;
    void Start()
    {
        var parkingLot = Instantiate(Lot);
        parkingLot.transform.position = new Vector3(1, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
