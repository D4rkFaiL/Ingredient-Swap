using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{

    public Dialog[] dialog;
    public int dialogIndex;
    //bool perto;
    public bool krab = false;

    public void Update()
    {
        // if (Input.GetButtonDown("Use"))
        // {
        //     if (!FindObjectOfType<DialogManager>().comecou)
        //     {
        //         TriggerDialog();
        //         return;
        //     }
        //     else
        //     {
        //         ProxDialog();
        //     }          
        // }       
    }

    public void TriggerDialog(int index)
    {
        FindObjectOfType<DialogManager>().StartDialogue(dialog[index]);
    }

    public void ProxDialog()
    {
        FindObjectOfType<DialogManager>().DisplayNextSentence();
    }

    public void EndDialog()
    {
        FindObjectOfType<DialogManager>().EndDialogue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !krab)
        {
            //perto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player") && !krab)
        {
            //perto = false;
            EndDialog();
        }
    }

}
