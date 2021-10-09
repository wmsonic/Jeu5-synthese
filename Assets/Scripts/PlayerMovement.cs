using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speedModifier = 5f;
    [SerializeField] float _strafeSpeed = 2f;
    [SerializeField] float _fowardSpeed = 3f;
    [SerializeField] float _backwardsSpeed = 1f;
    [SerializeField] float _sprintModifier = 1.2f;
    [SerializeField] float _crouchModifier = .8f;
    [SerializeField] Transform _cameraTransform;
    private float _verticalSpeed;

    private Vector2 _moveDir;
    private float _sprintStatus;
    private float _crouchStatus; 



    private CharacterController _characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = this.GetComponent<CharacterController>();
        print(_characterController.center);
    }

    void OnMove(InputValue value){
        _moveDir = value.Get<Vector2>();
        if(_moveDir.y > 0){
            _verticalSpeed = _fowardSpeed;
        }else if(_moveDir.y < 0){
            _verticalSpeed = _backwardsSpeed;
            if(_crouchStatus == 2){
                _crouchStatus = 1;
            }
            _sprintStatus = 0;
        }else{
            _verticalSpeed = 0f;
            if(_crouchStatus == 2){
                _crouchStatus = 1;
            }
            _sprintStatus = 0;
        }
        // print("sprint status : " + _sprintStatus);
        // Debug.Log("On Move");
    }

    void OnSprint(InputValue value){
        if(_crouchStatus > 0){
            _crouchStatus = 0;
        }
        _sprintStatus = value.Get<float>();
        // print(_sprintStatus);
    }

    void OnCrouch(InputValue value){
        if(_sprintStatus > 0){
            _crouchStatus = 2; //sets crouchStatus to 2 which is a slide
            _sprintStatus = 0;
        }else{
            _crouchStatus = value.Get<float>(); //this sets _crouchStatus to 1 which means we are crouching
        }
    }

    void OnCrouchToggle(InputValue value){
        if(_sprintStatus > 0){
            _crouchStatus = 2; //sets crouchStatus to 2 which is a slide
            _sprintStatus = 0;
        }else if(_crouchStatus > 0){
            _crouchStatus = 0; //if we are already crouching i.e because of crouch toggle, toggle back to not crouching
        }else{
            _crouchStatus = value.Get<float>(); //this sets _crouchStatus to 1 which means we are crouching
        }
        // print(_crouchStatus);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = new Vector3(_moveDir.x * _strafeSpeed, 0, _moveDir.y * _verticalSpeed);
        moveVector = transform.TransformDirection(moveVector);
        _characterController.Move(Physics.gravity * Time.deltaTime *2f);
        if(_crouchStatus > 1){
            _characterController.Move(moveVector * Time.deltaTime * _speedModifier * _crouchModifier);
            transform.localScale = new Vector3(1.4f,.3f,1);
        }else if(_crouchStatus > 0 && _crouchStatus <= 1){
            _characterController.Move(moveVector * Time.deltaTime * _speedModifier * _crouchModifier);
            transform.localScale = new Vector3(1,.5f,1);
        }else if(_sprintStatus > 0){
            _characterController.Move(moveVector * Time.deltaTime * _speedModifier * _sprintModifier);
            transform.localScale = new Vector3(1,1,1);
        }else{
            _characterController.Move(moveVector * Time.deltaTime * _speedModifier);
            transform.localScale = new Vector3(1,1,1);
        }
        // print(moveVector);
    }
}
