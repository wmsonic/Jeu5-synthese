using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAdder : MonoBehaviour
{
    [SerializeField] private Inventaire _inventaire;
    [SerializeField] private GameObject _rightHand;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("item")){
            string type = other.GetComponent<Item>().itemType;
            _inventaire.AddToInventory(type);
            Destroy(other.gameObject);
        }
    }

    public void AddWeapon(GameObject weapon){
        foreach (Transform child in _rightHand.transform){
            if(child.CompareTag("weapon")){
                Destroy(child.gameObject);
            }
        }
        if(weapon.name == "hache"){
            GetComponent<Animator>().SetFloat("vitesseAttaque",.8f);
            GetComponent<MovePerso>().AllowAttacking();
        }
        if(weapon.name == "epee"){
            GetComponent<Animator>().SetFloat("vitesseAttaque",1.2f);
        }
        GameObject newWeapon = Instantiate(weapon,_rightHand.transform);
        newWeapon.GetComponentInChildren<Collider>().enabled = false;
        GetComponent<MovePerso>().SetWeapon(newWeapon);
    }
}
