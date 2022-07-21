using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class Inventaire : MonoBehaviour
{

    [SerializeField] private Text branchesText;
    [SerializeField] private Button axeCraftBtn;
    [SerializeField] private int requiredBranches;
    private int branchesCompteur;
    [SerializeField] private Text bambooText;
    [SerializeField] private Button tubaCraftBtn;
    [SerializeField] private int requiredBamboo;
    private int bambooCompteur;
    [SerializeField] private Text feuillesText;
    [SerializeField] private Button palmesCraftBtn;
    [SerializeField] private int requiredFeuilles;
    private int feuillesCompteur;
    [SerializeField] private Text piecesText;
    private int requiredPieces;
    // private int totalPieces;
    private int piecesCompteur;
    [SerializeField] private Text vasesText;
    private int requiredVases;
    // private int totalVases;
    private int vasesCompteur;

    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject player;
    
    // Start is called before the first frame update
    void Start(){
        branchesText.text = branchesCompteur + "/"+requiredBranches+ "\r\n branches";
        bambooText.text = bambooCompteur + "/"+requiredBamboo+ "\r\n bamboo";
        feuillesText.text = feuillesCompteur + "/"+requiredFeuilles+ "\r\n feuilles";
        piecesText.text = "Vous avez " + piecesCompteur + "/? pieces";
        vasesText.text = "Vous avez " + vasesCompteur + "/? vases";
    }

    public void SetTotalVases(int totalAmount){
        // totalVases = totalAmount;
        Debug.Log("Il y a un total de " +totalAmount+" vases");
        requiredVases = Mathf.RoundToInt(totalAmount / 4f);
        Debug.Log("Il faudra " +requiredVases+" vases pour gagner");
    }
    public void SetTotalPieces(int totalAmount){
        // totalPieces = totalAmount;
        Debug.Log("Il y a un total de " +totalAmount+" pieces");
        requiredPieces = Mathf.RoundToInt(totalAmount / 6f);
        Debug.Log("Il faudra " +requiredPieces+" pieces pour gagner");
    }

    public void AddToInventory(string itemName){
        int nbToAdd = 1;
        switch(itemName){
            case "branche":
                branchesCompteur = branchesCompteur + nbToAdd;
                branchesText.text = branchesCompteur + "/"+requiredBranches+ "\r\n branches";
                if(branchesCompteur >= requiredBranches){
                    axeCraftBtn.interactable = true;
                }
                break;
            case "bamboo":
                bambooCompteur = bambooCompteur + nbToAdd;
                bambooText.text = bambooCompteur + "/"+requiredBamboo+ "\r\n bamboo";
                if(bambooCompteur >= requiredBamboo){
                    tubaCraftBtn.interactable = true;
                }
                break;
            case "feuille":
                feuillesCompteur = feuillesCompteur + nbToAdd;
                feuillesText.text = feuillesCompteur + "/"+requiredFeuilles+ "\r\n feuilles";
                if(feuillesCompteur >= requiredFeuilles){
                    palmesCraftBtn.interactable = true;
                }
                break;
            case "pieces":
                piecesCompteur = piecesCompteur + nbToAdd;
                piecesText.text = "Vous avez " + piecesCompteur + "/" + requiredPieces + " pieces";
                if(piecesCompteur >= requiredPieces && vasesCompteur >= requiredVases){
                    WinGame();
                }
                break;
            case "vases":
                vasesCompteur = vasesCompteur + nbToAdd;
                vasesText.text = "Vous avez " + vasesCompteur + "/" + requiredVases + " vases";
                if(piecesCompteur >= requiredPieces && vasesCompteur >= requiredVases){
                    WinGame();
                }
                break;

        }
    }
    public void removeFromInventory(string itemName){
        
        switch(itemName){
            case "branche":
                branchesCompteur = branchesCompteur - requiredBranches;
                branchesText.text = "Vous avez " + branchesCompteur + "/"+requiredBranches+" branches";
                if(branchesCompteur < requiredBranches){
                    axeCraftBtn.interactable = false;
                }
                break;
            case "bamboo":
                bambooCompteur = bambooCompteur - requiredBamboo;
                bambooText.text = "Vous avez " + bambooCompteur + "/"+requiredBamboo+" bamboo";
                if(bambooCompteur < requiredBamboo){
                    tubaCraftBtn.interactable = false;
                }
                break;
            case "feuille":
                feuillesCompteur = feuillesCompteur - requiredFeuilles;
                feuillesText.text = "Vous avez " + feuillesCompteur + "/"+requiredFeuilles+" feuilles";
                if(feuillesCompteur < requiredFeuilles){
                    palmesCraftBtn.interactable = false;
                }
                break;
        }
    }

    public void WinGame(){
        winText.SetActive(true);
        Invoke("PlayEnding",3f);
    }

    private void PlayEnding(){
        SceneManager.LoadScene("CinematiqueFin");
    }

}
