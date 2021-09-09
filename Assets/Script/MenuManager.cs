using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    private void Awake() {

        if (instance != null)
        {
            Debug.LogWarning("Mais de uma instancia feita!");
            return;
        }
        instance = this;
    }

    public Animator animator;
    public GameObject finalScreen;

    public void CarregarProxLevel(){
        AudioManager.instance.PlayAudioClip(0);

        if(SceneManager.GetActiveScene().buildIndex == 11){
            finalScreen.SetActive(true);
        }
        else{
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
        
    }
    
    public void CarregarScenePorIndex(int index){
        StartCoroutine(LoadScene(index));
    }

    public void ReloadScene(){
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void FecharJogo(){
        Application.Quit();
    }

    IEnumerator LoadScene(int index){
        animator.Play("TransicaoIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }   

}
