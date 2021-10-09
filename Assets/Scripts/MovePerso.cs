using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePerso : MonoBehaviour
{

    [SerializeField] private float vitesseMouvement = 20.0f;
    [SerializeField] private float vitesseRotation = 3.0f;
    [SerializeField] private float impulsionSaut = 30.0f;
    [SerializeField] private float gravite = 0.2f;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject champForce;

    private float vitesseSaut;
    private Vector3 directionsMouvement = Vector3.zero;

    Animator animator;
    CharacterController controller;

    
    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //permet de faire rotate le personnage sur l'axe des Y à l'aide de vitesseRotation en le multipliant par l'axe d'input "Horizontal" qui retourne un chiffre entre -1 et 1
        transform.Rotate(0, Input.GetAxis("Horizontal")*vitesseRotation,0); 

        //permet définir notre vitesse avant/arrière à l'aide de vitesseMouvement en le multipliant par l'axe d'input  "Vertical" qui retourne un chiffre entre -1 et 1
        float vitesse = vitesseMouvement * Input.GetAxis("Vertical"); 

        //permet d'activer l'animation de course à l'aide de la vitesse en vérifiant si elle est plus grande que 0
        animator.SetBool("enCourse",vitesse > 0); 

        //permet de créer la direction de mouvement de notre personnage à l'aide de vitesse en l'applicant sur l'axe des Z
        directionsMouvement = new Vector3(0,0, vitesse); 

        //permet de convertir notre mouvement d'un mouvement local à un mouvement global à l'aide de la fonction TransformDirection en lui donnant notre vector3 de mouvement
        directionsMouvement = transform.TransformDirection(directionsMouvement);

        //permet de set la vitesse de notre saut à l'aide de vitesseSaut et impulsionSaut en vérifiant si le joueur appui sur le input de jump (espace) et que notre characterController est au sol
        if(Input.GetButton("Jump") && controller.isGrounded) vitesseSaut = impulsionSaut; 

        //permet de d'activer l'animation de saut à l'aide de la vitesseSaut et d'impulsionSaut en vérifiant que notre characterController n'est pas au sol et que la vitesse en Y est plus grande que l'impulsion du saut
        animator.SetBool("enSaut", !controller.isGrounded && vitesseSaut > -impulsionSaut);

        //permet de faire sauter notre personnage à l’aide de vitesseSaut en ajoutant ça valeur à notre direction de mouvement en Y
        directionsMouvement.y += vitesseSaut;

        //permet de d'appliquer la graviter à notre personnage à l’aide de vitesseSaut et gravite en vérifiant que notre characterController n'est pas au sol et en enlevant de notre vitesse vertical la valeur de la graviter
        if(!controller.isGrounded) vitesseSaut -= gravite;

        //permet de bouger notre personnage à l’aide de la fonction Move du characterController et de directionsMouvement en lui donnant directionsMouvemnt multiplier par le Time.deltaTime pour que le mouvement se déroule en temps réel
        controller.Move(directionsMouvement * Time.deltaTime);

        // mainCamera.position = mainCamera.position + (mainCamera.forward * Mathf.Clamp(vitesse * Time.deltaTime, 10, 30));
        
        // if(directionsMouvement.z !> 0 || directionsMouvement.y > 0) champForce.GetComponent<Transform>().localScale = champForce.GetComponent<Transform>().localScale * vitesse;

    }
}
