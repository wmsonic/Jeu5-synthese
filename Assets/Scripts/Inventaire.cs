using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{

    [SerializeField] private Text branchesText;
    [SerializeField] private Text bambooText;
    private int branchesCompteur;
    private int bambooCompteur;
    
    // Start is called before the first frame update
    void Start(){
        branchesText.text = "Vous avez " + branchesCompteur + " branches";
        bambooText.text = "Vous avez " + bambooCompteur + " bamboo";
    }

    public void AddToInventory(string itemName){
        int nbToAdd = 1;
        switch(itemName){
            case "branches":
                branchesCompteur = branchesCompteur + nbToAdd;
                branchesText.text = "Vous avez " + branchesCompteur + " branches";
                break;
            case "bamboo":
                bambooCompteur = bambooCompteur + nbToAdd;
                bambooText.text = "Vous avez " + bambooCompteur + " bamboo";
                break;

        }
    }
}
