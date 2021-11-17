using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("item")){
            other.GetComponent<Item>().StartFollowing();
        }
    }
}
