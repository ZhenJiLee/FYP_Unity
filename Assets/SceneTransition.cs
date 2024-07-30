using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad = "RhythmLevel1";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadTargetScene();
        }
    }

    
    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}