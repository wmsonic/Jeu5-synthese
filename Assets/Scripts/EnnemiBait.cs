using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiBait : MonoBehaviour
{
    void Start()
    {
        GameObject[] ennemies = GameObject.FindGameObjectsWithTag("ennemi");
        foreach(GameObject ennemi in ennemies){
            if(Random.Range(0f,100f) > 50){
                ennemi.GetComponent<EnnemiEtatsManager>().StopAllCoroutines();
                ennemi.GetComponent<EnnemiEtatsManager>().StartCoroutine("GotoBait",transform.position);
            }
        }
    }

}
