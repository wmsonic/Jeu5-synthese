using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    [SerializeField] GameObject _itemToDrop;
    BiomesEtatsManager _parentCube;

    private void Start() {
        _parentCube = transform.parent.GetComponent<BiomesEtatsManager>();
    }
    public void dropItem() {
        // Debug.Log("instanciate !");
        Vector3 posToSpawn = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject droppedItem = Instantiate(_itemToDrop, posToSpawn, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other) {
        if(gameObject.CompareTag("anyCanDestroy") || gameObject.CompareTag("cactus")){
            // Debug.Log("you can destroy me with any weapon");
            if(other.CompareTag("hache") || other.CompareTag("epee")){
                other.GetComponent<AudioSource>().Play();
                _parentCube.ChangerEtat(_parentCube._briser);
            }
        }else if(gameObject.CompareTag("swordCanDestroy")){
            // Debug.Log("you can destroy me with a sword");
            if(other.CompareTag("epee")){
                other.GetComponent<AudioSource>().Play();
                _parentCube.ChangerEtat(_parentCube._briser);
            }
        }
    }
    
}
