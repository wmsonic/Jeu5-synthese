using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;  

public class QuitCinematiqueDebut : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        // Invoke("ChangeScene",8f);
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += ChangeScene;
    }
    
    void ChangeScene(UnityEngine.Video.VideoPlayer vp){
        SceneManager.LoadScene("Jeu");
    }
}
