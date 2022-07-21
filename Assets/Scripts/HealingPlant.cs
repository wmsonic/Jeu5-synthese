using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlant : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            other.GetComponent<InventoryAdder>().AddHP(gameObject);
        }
    }
}
