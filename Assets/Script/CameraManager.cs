using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    
    #region Singleton
    public static CameraManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Mais de uma instancia feita!");
            return;
        }
        instance = this;
    }
    #endregion

    bool seguirPlayer;
    public float cameraMinPos;
    public float cameraMaxPos;
    public GameObject player;
    public float smoothCamera;


    // Update is called once per frame
    void Update()
    {
        if(seguirPlayer){
            if(player.transform.position.x > cameraMinPos && player.transform.position.x < cameraMaxPos){
                var finalPos = Vector2.Lerp(transform.position,player.transform.position,smoothCamera * Time.deltaTime);
                transform.position = new Vector3(finalPos.x,transform.position.y,-10);
            }
        }
    }

    public void SeguirPlayer(bool seguir,Vector2 cameraLimits){
        seguirPlayer = seguir;
        cameraMinPos = cameraLimits.x;
        cameraMaxPos = cameraLimits.y;
        transform.position = transform.position = new Vector3(cameraLimits.x,transform.position.y,-10);       
    }
}
