using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("item") || other.CompareTag("collectible")){
            other.GetComponent<Item>().StartFollowing();
        }
    }
}
