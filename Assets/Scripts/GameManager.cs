using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DialogManager dialogManager; 
    public string[] openingDialog;     

    private void Start()
    {
        
        if (dialogManager == null)
        {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        
        if (openingDialog != null && openingDialog.Length > 0)
        {
            dialogManager.StartDialog(openingDialog); 
        }
    }
}
