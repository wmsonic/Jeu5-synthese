using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    // [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _pauseScreen;
    [HideInInspector] public bool _inPause = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")){
            if(!_inPause){
                EnterPauseMenu();
            }else{
                ExitPauseMenu();
            }
        }
    }

    public void EnterPauseMenu(){
        Time.timeScale = 0f;
        _inPause = true;
        _pauseScreen.SetActive(true);
    }
    public void ExitPauseMenu(){
        Time.timeScale = 1f;
        _inPause = false;
        _pauseScreen.SetActive(false);
    }

    public void ChangeVolume(float volume){
        AudioListener.volume = volume;
    }
}
