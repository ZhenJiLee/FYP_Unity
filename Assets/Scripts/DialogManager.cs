using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; 
    public TextMeshProUGUI dialogText; 
    public float typingSpeed = 0.05f; 

    private Queue<string> dialogQueue; 
    private Coroutine typingCoroutine; 

    void Awake()
    {
        dialogQueue = new Queue<string>();
        dialogPanel.SetActive(false);
    }

    public void StartDialog(string[] dialogLines)
    {
        Time.timeScale = 0f; 
        dialogPanel.SetActive(true);

        if (SongManager.Instance != null)
        {
            SongManager.Instance.PauseSong(); 
        }

        dialogQueue.Clear(); 

        foreach (string line in dialogLines)
        {
            dialogQueue.Enqueue(line); 
        }

        DisplayNextDialog(); 
    }

    public void DisplayNextDialog()
    {
        if (dialogQueue.Count == 0)
        {
            EndDialog();
            return;
        }

        string nextLine = dialogQueue.Dequeue(); 

        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        
        typingCoroutine = StartCoroutine(TypeDialog(nextLine));
    }

    private IEnumerator TypeDialog(string line)
    {
        dialogText.text = ""; 
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter; 
            yield return new WaitForSecondsRealtime(typingSpeed); 
        }
    }

    public void EndDialog()
    {
        dialogPanel.SetActive(false);
        Time.timeScale = 1f;

        if (SongManager.Instance != null)
        {
            SongManager.Instance.ResumeSong(); // 确保从暂停位置继续播放音乐
        }

        // 恢复生成节点的逻辑
        foreach (var lane in FindObjectsOfType<Lane>())
        {
            lane.enabled = true;
        }
    }



    public void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); 
            typingCoroutine = null;

            
            if (dialogQueue.Count > 0)
            {
                dialogText.text = dialogQueue.Peek();
            }
        }
    }
}
