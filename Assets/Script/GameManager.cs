using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Controle controle;
    private void Awake() {

        if (instance != null)
        {
            Debug.LogWarning("Mais de uma instancia feita!");
            return;
        }
        instance = this;

        controle = new Controle();
        
        controle.Player.SwitchCharacter.performed += ctx => AlterarIndex(ctx.ReadValue<float>());

        controle.Player.Esc.performed += _ => AbrirMenu();

        controle.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controle.Player.Move.canceled += _ => Move(new Vector2(0,0));

    }

    
    [Header("GM Info")]
    public Player[] players;
    public int index = 0;

    [Header("Player Info")]
    public Vector2 lastPos;
    public float lastHorizontalValue;
    public Vector2 lastVelocity;
    public float horizontalInput;
    public float verticalInput;

    [Header("Esc info")]
    public GameObject xqdl;


    void AbrirMenu(){
        xqdl.SetActive(!xqdl.activeSelf);
    }

    void AlterarIndex(float value){

        lastPos = players[index].gameObject.transform.position;
        lastHorizontalValue = players[index].horizontalInput;
        lastVelocity = players[index].rb.velocity;

        if(value > 0)
            index++;
        else
            index--;

        if(index > players.Length-1)
            index = 0;
        if(index < 0)
            index = players.Length-1;

        TrocarControlavel();
    }

    void TrocarControlavel(){


        for (int i = 0; i < players.Length; i++)
        {
            if(index == i){
                
                if(i == 1){
                    players[i].firstTimeOutOfGround = true;
                }

                players[i].horizontalInput = this.horizontalInput;
                players[i].verticalInput = this.verticalInput;
                players[i].gameObject.transform.position = lastPos;
                players[i].rb.gravityScale = 1;
                players[i].gameObject.SetActive(true);
                players[i].rb.velocity = lastVelocity;
                //players[i].horizontalInput = lastHorizontalValue;
          
            }else{
                players[i].gameObject.SetActive(false);
            }
        }
    }
    
    public void Move(Vector2 moveValues){
        this.horizontalInput = moveValues.x;
        this.verticalInput = moveValues.y;

        players[index].horizontalInput = moveValues.x;
        players[index].verticalInput = moveValues.y;
    }

    private void OnEnable() {
        controle.Enable();
    }

    private void OnDisable() {
        controle.Disable();
    }
}
