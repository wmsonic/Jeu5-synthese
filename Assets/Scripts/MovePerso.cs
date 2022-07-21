using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePerso : MonoBehaviour
{

    [SerializeField] private float vitesseMouvement = 20.0f;
    [SerializeField] private float vitesseRotation = 3.0f;
    [SerializeField] private float impulsionSaut = 30.0f;
    [SerializeField] private float gravite = 0.2f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject champForce;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _marcheEau;
    [SerializeField] private AudioClip _attaque;

    private float vitesseSaut;
    private Vector3 directionsMouvement = Vector3.zero;

    Animator animator;
    CharacterController controller;

    private Vector3 champForceDefaultScale;

    private float timeInJump = 0;
    private bool _canSwim = false;
    private bool _isUnderwater = false;

    private bool _canAttack = false;
    private GameObject _currentWeapon;
    private ParticleSystem _weaponTrail;
    private Collider _weaponCollider;
    
    void Awake(){
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        champForceDefaultScale = champForce.GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void Update(){
        //permet de faire rotate le personnage sur l'axe des Y à l'aide de vitesseRotation en le multipliant par l'axe d'input "Horizontal" qui retourne un chiffre entre -1 et 1
        transform.Rotate(0, Input.GetAxis("Mouse X")*vitesseRotation,0); 

        //permet définir notre vitesse gauche/droite à l'aide de vitesseMouvement en le multipliant par l'axe d'input  "Horizontal" qui retourne un chiffre entre -1 et 1
        float vitesseSideways = vitesseMouvement/1.5f * Input.GetAxis("Horizontal"); 
        //permet définir notre vitesse avant/arrière à l'aide de vitesseMouvement en le multipliant par l'axe d'input  "Vertical" qui retourne un chiffre entre -1 et 1
        float vitesseFoward = vitesseMouvement * Input.GetAxis("Vertical"); 

        //permet d'activer l'animation de course à l'aide de la vitesse en vérifiant si elle est plus grande que 0
        animator.SetBool("enCourse",Mathf.Abs(vitesseFoward) > 0); 

        //permet de créer la direction de mouvement de notre personnage à l'aide de vitesse en l'applicant sur l'axe des Z
        directionsMouvement = new Vector3(vitesseSideways,0, vitesseFoward); 

        //permet de convertir notre mouvement d'un mouvement local à un mouvement global à l'aide de la fonction TransformDirection en lui donnant notre vector3 de mouvement
        directionsMouvement = transform.TransformDirection(directionsMouvement);

        //permet de set la vitesse de notre saut à l'aide de vitesseSaut et impulsionSaut en vérifiant si le joueur appui sur le input de jump (espace) et que notre characterController est au sol
        if(controller.isGrounded){
            if(Input.GetButton("Jump")){
                animator.SetBool("enSaut", true);
                if(!_isUnderwater){
                    vitesseSaut = impulsionSaut; 
                }else if(_isUnderwater && !_canSwim){
                    vitesseSaut = impulsionSaut/6f; 
                }else if(_isUnderwater && _canSwim){
                    vitesseSaut = impulsionSaut/3f;
                }
            }else{
                vitesseSaut = 0;
                animator.SetBool("enSaut", false);
            }
        }else if(_isUnderwater && _canSwim){
                if(Input.GetButton("Jump")){
                    animator.SetBool("enSaut", true);
                    vitesseSaut = impulsionSaut/3f; 
                }else{
                    animator.SetBool("enSaut", false);
                }   
        }

        //permet de faire sauter notre personnage à l’aide de vitesseSaut en ajoutant ça valeur à notre direction de mouvement en Y
        directionsMouvement.y += vitesseSaut; 
        

        //permet de d'appliquer la graviter à notre personnage à l’aide de vitesseSaut et gravite en vérifiant que notre characterController n'est pas au sol et en enlevant de notre vitesse vertical la valeur de la graviter
        if(!controller.isGrounded && !_isUnderwater){
            vitesseSaut -= gravite;
        }else if(!controller.isGrounded && _isUnderwater){
            vitesseSaut -= (gravite/4);
        }

        //permet de bouger notre personnage à l’aide de la fonction Move du characterController et de directionsMouvement en lui donnant directionsMouvemnt multiplier par le Time.deltaTime pour que le mouvement se déroule en temps réel
        controller.Move(directionsMouvement * Time.deltaTime);

        //permet de modifier progressivement le FOV de la caméra pour qu'il corresponde à la vitesse de notre personnage à l'aide de la fonction Lerp en lui donnant les FOV minimum et maximum que l'on veut et la valeur de notre input
        mainCamera.fieldOfView = Mathf.Lerp(50,80, Input.GetAxis("Vertical"));
        
        if(controller.isGrounded){
            champForce.GetComponent<Transform>().localScale = champForceDefaultScale * Mathf.Lerp(0,1, Input.GetAxis("Vertical") );
            timeInJump = 0;
        } else {
            float duration= .5f;
            champForce.GetComponent<Transform>().localScale = champForceDefaultScale * Mathf.Lerp(1,0, timeInJump / duration);
            if(timeInJump<1){
                timeInJump += Time.deltaTime;
            }
            // Debug.Log(timeInJump);
        }

        if(Input.GetButton("Fire1") && controller.isGrounded && _canAttack) StartCoroutine(Attack());
    }
    public void setUnderwater(bool status){
        _isUnderwater = status;
        if(_isUnderwater){
            vitesseMouvement = vitesseMouvement/1.2f;
            vitesseRotation = vitesseRotation/1.2f;
            // _audioSource.clip = _marcheEau;
            // _audioSource.Play();
        }
        else{
            vitesseMouvement = vitesseMouvement*1.2f;
            vitesseRotation = vitesseRotation*1.2f;
            // _audioSource.Stop();
        }
    }
    public void AllowSwimming(){
        _canSwim = true;
    }

    public void AllowAttacking(){
        _canAttack = true;
    }
    public void SetWeapon(GameObject weapon){
        _currentWeapon = weapon;
        _weaponTrail = _currentWeapon.GetComponentInChildren<ParticleSystem>();
        _weaponCollider = _currentWeapon.GetComponentInChildren<Collider>();
    }

    IEnumerator Attack(){
        _weaponCollider.enabled = true;
        _canAttack = false;
        animator.SetTrigger("attaque");
        _audioSource.PlayOneShot(_attaque);
        _weaponTrail.Play();
        yield return new WaitForSeconds(1.5f);
        _canAttack = true;
        _weaponCollider.enabled = false;
        yield return null;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("water")){
            setUnderwater(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("water")){
            setUnderwater(false);
        }
        
    }

    public void PlayWalkSound(){
        if(_isUnderwater){
            _audioSource.PlayOneShot(_marcheEau);
        }
    }
}
