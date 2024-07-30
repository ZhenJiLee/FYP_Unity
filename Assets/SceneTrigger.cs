using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public string nextSceneName; // 下一个场景的名称

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保只有玩家触发
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
