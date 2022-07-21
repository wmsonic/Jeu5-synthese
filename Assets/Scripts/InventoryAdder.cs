using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAdder : MonoBehaviour
{
    [SerializeField] private Inventaire _inventaire;
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject[] _palmes;
    [SerializeField] private GameObject _tuba;
    [SerializeField] private GameObject _masque;
    [SerializeField] private GameObject _sac;
    [SerializeField] private GameObject _epee;
    [SerializeField] private GameObject _step3;
    [SerializeField] private GameObject _objectifStep3;
    [SerializeField] private GameObject _step4;
    [SerializeField] private GameObject _objectifStep4;
    [SerializeField] private GameObject _step5Optionnal;
    [SerializeField] private Image _healthBar;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private AudioClip _musiqueEau;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _particleDegat;
  

    private float _healthBarProgression = 0;
    private int _totalHealth = 5;
    private int _currentHealth;
    private bool _hasWon = false;

    private void Start() {
        _currentHealth = _totalHealth;
    }
    public void setHasWon(){
        _hasWon = true;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("item") || other.CompareTag("collectible")){
            string type = other.GetComponent<Item>().itemType;
            _inventaire.AddToInventory(type);
            Destroy(other.gameObject);
        }else if(other.CompareTag("bouee")){
            AddWeapon(_epee);
            _step3.SetActive(false);
            _objectifStep3.SetActive(false);
            _step4.SetActive(true);
            _objectifStep4.SetActive(true);
            Destroy(other.gameObject);
        }else if(other.CompareTag("masque")){
            addEquipment("masque");
            _step5Optionnal.SetActive(false);
            Destroy(other.gameObject);
        }else if(other.CompareTag("ennemi")){ 
            if(!_hasWon){
                   RemoveHP();
            }
        }
    }

    public void RemoveHP(bool fromWater = false){
        _currentHealth = _currentHealth - 1;
        _particleDegat.Play();
        if(_currentHealth > 0){
            _healthBarProgression = _healthBarProgression + .2f;
            _healthBar.transform.localScale = new Vector3(_healthBarProgression, _healthBar.transform.localScale.y, _healthBar.transform.localScale.z);
            StartCoroutine(FlashHealthBar());
            if(fromWater){
                GetComponentInChildren<SwimSystem>().StartCoroutine("OxygenCounter",5f);
            }
        }else{
            if(fromWater){
                GetComponentInChildren<SwimSystem>().StopAllCoroutines();
            }
            _healthBarProgression = 1;
            _healthBar.transform.localScale = new Vector3(_healthBarProgression, _healthBar.transform.localScale.y, _healthBar.transform.localScale.z);
            TriggerLose();
            GetComponentInChildren<SwimSystem>().setHasLost();
        }
    }

    public void AddHP(GameObject other){//recevoir gameobject qui à call
        if(_currentHealth < _totalHealth){
            _currentHealth = _currentHealth + 1;
            _healthBarProgression = _healthBarProgression - .2f;
            _healthBar.transform.localScale = new Vector3(_healthBarProgression, _healthBar.transform.localScale.y, _healthBar.transform.localScale.z);
            Destroy(other);
            StartCoroutine(HealHealthBar());
            //detrui gameobject qui à call
        }
    }

    public void TriggerLose(){
        Time.timeScale = 0f;
        _loseText.SetActive(true);
        this.GetComponent<MovePerso>().enabled = false;
    }

    public void AddWeapon(GameObject weapon){
        foreach (Transform child in _rightHand.transform){
            if(child.CompareTag("weapon")){
                Destroy(child.gameObject);
            }
        }
        if(weapon.name == "hache"){
            GetComponent<Animator>().SetFloat("vitesseAttaque",1f);
            GetComponent<MovePerso>().AllowAttacking();
        }
        if(weapon.name == "epee"){
            GetComponent<Animator>().SetFloat("vitesseAttaque",1.2f);
        }
        GameObject newWeapon = Instantiate(weapon,_rightHand.transform);
        newWeapon.GetComponentInChildren<Collider>().enabled = false;
        GetComponent<MovePerso>().SetWeapon(newWeapon);
    }

    public void addEquipment(string itemName){
        switch(itemName){
            case "palmes":
                foreach (var palme in _palmes)
                {
                    palme.SetActive(true);
                }
            break;
            case "tuba":
                _tuba.SetActive(true);
                _audioSource.clip = _musiqueEau;
                _audioSource.Play();
                break;
            case "masque":
                _masque.SetActive(true);
                GetComponentInChildren<SwimSystem>().setHasMasque();
                //activate underwater sight
            break;

        }
    }

    private IEnumerator FlashHealthBar(){
        Color healthBarColor = _healthBar.transform.parent.GetComponent<Image>().color;
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,255);
        yield return new WaitForSeconds(.1f);
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,healthBarColor.a);
        yield return new WaitForSeconds(.1f);
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,255);
        yield return new WaitForSeconds(.1f);
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,healthBarColor.a);
        yield return new WaitForSeconds(.1f);
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,255);
        yield return new WaitForSeconds(.1f);
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,healthBarColor.a);
        yield return null;
    }
    private IEnumerator HealHealthBar(){
        Color healthBarColor = _healthBar.transform.parent.GetComponent<Image>().color;
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,255);
        yield return new WaitForSeconds(.5f);
        _healthBar.transform.parent.GetComponent<Image>().color = new Color(healthBarColor.r,healthBarColor.g,healthBarColor.b,healthBarColor.a);
        yield return null;
    }

}
