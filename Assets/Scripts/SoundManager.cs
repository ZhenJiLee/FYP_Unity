using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    void Start()
    {
        // 检查是否有保存的音量值，如果没有则设置默认值为50
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 50); // 默认值设为50（即0.5）
            volumeSlider.value = 50; // 设置滑块初始值为50
        }
        else
        {
            Load(); // 如果有保存的音量值，加载并应用
        }

        ApplyVolume(); // 初始应用音量
    }

    public void ChangeVolume()
    {
        ApplyVolume(); // 更新全局音量
        Save(); // 保存当前音量值
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume"); // 读取0-100的值
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value); // 保存0-100的值
    }

    private void ApplyVolume()
    {
        AudioListener.volume = volumeSlider.value / 100f; // 将滑块值缩放到0-1范围并应用
    }

    public void low()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void med()
    {
        QualitySettings.SetQualityLevel(1);
    }
    public void high()
    {
        QualitySettings.SetQualityLevel(2);
    }
}
