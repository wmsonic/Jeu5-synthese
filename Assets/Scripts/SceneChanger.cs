using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;  

public class SceneChanger: MonoBehaviour {  
    public void Menu() {  
        SceneManager.LoadScene("Menu");  
    }  
    public void Jeu() {  
        SceneManager.LoadScene("Jeu");  
    }  

    public void CinematiqueFin(){
        SceneManager.LoadScene("CinematiqueFin");  
    }
    public void CinematiqueDebut(){
        SceneManager.LoadScene("CinematiqueDebut");
    }
}  