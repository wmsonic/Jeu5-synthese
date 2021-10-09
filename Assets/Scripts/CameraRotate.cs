using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;
    private Transform _player;
    private Transform _transform;
    private Vector2 _rotateDir;
    private Transform _characterTransform;

    void OnRotateCam(InputValue value){
        _rotateDir = value.Get<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Transform>();
        _transform = _camera.GetComponent<Transform>();
        _characterTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 rotationVectorPlayer = new Vector3 (0, _rotateDir.x,0);
        Vector3 rotationVectorCamera = new Vector3 (_rotateDir.y, 0,0);

        _characterTransform.Rotate(rotationVectorPlayer,1f);

        _camera.LookAt(_player);
        _camera.RotateAround(_player.position, rotationVectorCamera, 1f);
    }
}
