using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suricate : MonoBehaviour
{
    private float moveSpeed = 3f;
    private Vector3 positionOriginale;
    private void Start()
    {
        positionOriginale=transform.position;
        transform.Rotate(0.0f,Random.Range(0.0f, 360.0f), 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            transform.Translate(Vector3.down * moveSpeed);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            // transform.Translate(Vector3.up * moveSpeed);
            transform.position=positionOriginale;
        }
    }
}
