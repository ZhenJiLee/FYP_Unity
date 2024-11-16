using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject bossCutsceneCanvas; // 用于显示过场动画的 Canvas
    private bool triggered = false; // 防止重复触发

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            bossCutsceneCanvas.SetActive(true); // 显示过场动画
            Invoke("StartBossBattle", 3f); // 3秒后开始BOSS战
        }
    }

    private void StartBossBattle()
    {
        bossCutsceneCanvas.SetActive(false); // 隐藏过场动画
        // 在这里可以调用 BOSS 战的开始逻辑
        Debug.Log("Boss Battle Started!");
    }
}
