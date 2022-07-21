using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SwimSystem : MonoBehaviour
{
    [SerializeField] private Image _oxygenBar;
    [SerializeField] private MovePerso _mouvementSystem;
    [SerializeField] private GameObject _underwaterEffectNoMask;
    [SerializeField] private GameObject _underwaterEffectMask;
    [SerializeField] private Camera _mainCamera;
    private bool _hasTuba = false;
    private bool _hasMasque = false;
    private bool _hasWon = false;
    private bool _hasLost = false;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("water")){
            if(_hasMasque){
                _underwaterEffectMask.GetComponent<Volume>().enabled = true;
            }else{
                _underwaterEffectNoMask.GetComponent<Volume>().enabled = true;
            }
            // _mouvementSystem.setUnderwater(true);
            if(!_hasWon && !_hasLost){
                _oxygenBar.transform.parent.gameObject.SetActive(true);
                StartCoroutine(OxygenCounter(7f));
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("water")){
            if(_hasMasque){
                _underwaterEffectMask.GetComponent<Volume>().enabled = false;
            }else{
                _underwaterEffectNoMask.GetComponent<Volume>().enabled = false;
            }
            // _mouvementSystem.setUnderwater(false);
            StopAllCoroutines();
            _oxygenBar.transform.parent.gameObject.SetActive(false);
            _oxygenBar.transform.GetChild(0).localScale = new Vector3(0, _oxygenBar.transform.GetChild(0).localScale.y, _oxygenBar.transform.GetChild(0).localScale.z);
        }
    }

    public void setHasTuba(){
        _hasTuba = true;
    }
    public void setHasWon(){
        _hasWon = true;
    }
    public void setHasLost(){
        _hasLost = true;
    }
    public void setHasMasque(){
        _hasMasque = true;
        _underwaterEffectNoMask.GetComponent<Volume>().enabled = false;
        _underwaterEffectMask.GetComponent<Volume>().enabled = true;
        _mainCamera.cullingMask += LayerMask.GetMask("TreasureShader");
    }

    IEnumerator OxygenCounter(float time){
        float animProgress = 0.0f; // déclaration d'un float pour tenir note de la progression de l'animation
        float rate;
        if(_hasTuba){
            rate = (1.0f / time) * .4f; // déclaration d'un rate auquel l'oxygen doit descendre
        }else{
            rate = (1.0f / time) * 1.2f; // déclaration d'un rate auquel l'oxygen doit descendre
        }
        while(animProgress < 1.0f){ // tant que la porgression de l'animation n'est pas rendu à 1 (donc 100%)
            animProgress += Time.deltaTime * rate; // incrémentation de la progression selon le temps depuis la dernière frame multiplier par le rate
            _oxygenBar.transform.GetChild(0).localScale = new Vector3(animProgress, _oxygenBar.transform.GetChild(0).localScale.y, _oxygenBar.transform.GetChild(0).localScale.z);
            yield return null;
        }
        GetComponentInParent<InventoryAdder>().RemoveHP(true);
    }

}
