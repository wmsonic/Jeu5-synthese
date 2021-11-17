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
        if(gameObject.CompareTag("anyCanDestroy")){
            Debug.Log("you can destroy me with any weapon");
            if(other.CompareTag("hache") || other.CompareTag("epee")){
                // get parent component pour changer de biome ?`
                // biome.ChangerEtat(biome._briser);
                _parentCube.ChangerEtat(_parentCube._briser);
            }
        }else if(gameObject.CompareTag("swordCanDestroy")){
            Debug.Log("you can destroy me with a sword");
            if(other.CompareTag("epee")){
                // biome.ChangerEtat(biome._briser);
                _parentCube.ChangerEtat(_parentCube._briser);
            }
        }
    }
    
}
