using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bouer;
    [SerializeField] private GameObject _masque;
    private float _niveauEau;
    private Vector2 _bouerLocation;
    private Vector3 _masqueLocation;

    public void SpawnBouer(){
        GetNiveauEau();
        Instantiate(_bouer, new Vector3(_bouerLocation.x, _niveauEau + 6f,_bouerLocation.y), Quaternion.identity );
    }
    public void SpawnMasque(){
        GetNiveauEau();
        Instantiate(_masque, new Vector3(_masqueLocation.x, _masqueLocation.y + 1f, _masqueLocation.z), Quaternion.identity );
    }
    public void SetSpawnPosition(string powerUpName, Vector3 position){
        if(powerUpName == "bouer"){
            _bouerLocation = new Vector2(position.x, position.z);
        }else if(powerUpName == "masque"){
            _masqueLocation = position;
        }
    }
    private void GetNiveauEau(){
        _niveauEau = GameObject.FindWithTag("niveau_eau").transform.position.y;
    }
}
