using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRiser : MonoBehaviour
{
    private float _waterMinHeight = 0;
    private float _waterMaxHeight = 20;

    float Sigmoid(float value){
        float k = 20f; // Intensiter de la courbe. Plus cette valeur est élever plus notre valeur d'entrer atteindra une valeur de 1 rapidement
        float c = 0f; // Valeur de base de la courbe (si on regarde sur un graphique c'est la valeur de Y quand X est 0)lorsque la valeur d'entrer est 0.
        return 1/(1+Mathf.Exp(-k*(value-c))); //Formule de la fonction sigmoïde. Retourne la valeur équivalente entre 0 et 1 à notre valeur d'entrer appliqué sur la courbe de la fonction sigmoïde
    }
    public void riseWater(float waterHeight){
        float targetHeight = Mathf.Lerp(_waterMinHeight,_waterMaxHeight,waterHeight);
        StartCoroutine(animateWaterRise(transform.position.y,targetHeight,2f));
    }

    IEnumerator animateWaterRise(float originalHeight, float targetHeight, float time){
        float animProgress = 0.0f; // déclaration d'un float pour tenir note de la progression de l'animation
        float rate = (1.0f / time) * 2f; // déclaration d'un rate auquel l'animation doit progrésser
        while(animProgress < 1.0f){ // tant que la porgression de l'animation n'est pas rendu à 1 (donc 100%)
            animProgress += Time.deltaTime * rate; // incrémentation de la progression selon le temps depuis la dernière frame multiplier par le rate
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(originalHeight, targetHeight, animProgress), transform.position.z); // Set la position de l'object à la position approprier entre l'original et la cible selon la progression de l'animation
            yield return null;
        }
    }
}
