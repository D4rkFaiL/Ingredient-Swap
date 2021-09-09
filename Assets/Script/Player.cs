using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Burga/Bun Characteristics")]
    public float velocidadePlayer;
    public float forcaDoPulo;
    public float divisaoPulo;
    public float groundDistanceDetect;
    public BoxCollider2D boxCollider;

    [Header("Bun Info")]
    public bool downDash;
    //public float multiplicadorDeVelocidadeDeQueda;
    public float multiplicadorDeBounce;
    public bool dashAvaliable;
    public bool firstTimeOutOfGround;
    public float resetGroundTimer;
    public float rotationSpeed;
    float totalSpeed;
    
    [Header("Cheesu Info")]
    public float throwForce;
    public float velocidadeDeAdicaoDeAngulo;
    public float angleInFloat;
    public bool stuckIntoSomething;
    public GameObject indicatorObj;
    public GameObject eixoDoIndicador;
    public bool jogarQueijo;

    [Header("Object Type")]
    public bool Hambuga;
    public bool Bun;
    public bool Cheesu;

    [Header("Laya Mask")]
    public LayerMask groundMask;

    [Header("Misc")]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public float horizontalInput;
    public float verticalInput;
    Animator animator;
    bool jump;
    bool asd;
    
    [Header("Audio Stuff")]
    public AudioSource audioSource;
    public AudioClip[] audioClip;
    
    public float timer,timercd;

    Controle controle;

    private void Awake() {
        controle = new Controle();
        controle.Player.Jump.performed += _ => Jump();
        controle.Player.Jump.canceled += _ => JumpReduce();
    }

    void Start()
    {

        defaultScaleX = transform.localScale.x;
        defaultScaleY = transform.localScale.y;

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        //audioSource = GetComponent<AudioSource>();      
        //animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {

        SquishySquashy();

        if(Hambuga || Bun){

            //Basic stuff
            if(horizontalInput != 0){

                rb.velocity = new Vector2(horizontalInput * velocidadePlayer * Time.deltaTime,rb.velocity.y);

                if(horizontalInput > 0){
                    totalSpeed -= rotationSpeed;
                }
                else{
                    totalSpeed += rotationSpeed;             
                }

                sprite.transform.rotation = Quaternion.Euler(0,0,totalSpeed);
            }
            else{
                rb.velocity = new Vector2(0,rb.velocity.y);
            }

            if(jump){
                jump = false;     
                AudioManager.instance.PlayAudioClip(4);
                sprite.transform.localScale = new Vector3(1,0.5f,1);
                rb.velocity = new Vector2(rb.velocity.x,forcaDoPulo * Time.deltaTime);
            }

            IsGrounded();
            
            //Bun stuff
            if(Bun && IsGrounded()){
               if(resetGroundTimer >= 0)
                    resetGroundTimer -= Time.deltaTime;
                else
                    firstTimeOutOfGround = true;
            }
            else{
                resetGroundTimer = 0.1f;
            }

            if(Bun && !IsGrounded() && firstTimeOutOfGround){
                lastPosition = transform.position.y;
                firstTimeOutOfGround = false;
                dashAvaliable = true;
            }

            if(verticalInput < 0 && Bun && dashAvaliable && !IsGrounded() && rb.velocity.y <= 0){
                DownDash();
            }

            if(downDash && IsGrounded()){
                BounceBack();
            }

        }
        else{
            
            if(jogarQueijo){
                ThrowTheCheesu();
            }

            if(horizontalInput != 0){
                angleInFloat += velocidadeDeAdicaoDeAngulo * horizontalInput * Time.deltaTime;
                eixoDoIndicador.transform.rotation = Quaternion.Euler(0,0,angleInFloat);
            }

        }
    }

    [Header("Squishy Squashy")]
    public float defaultScaleX,defaultScaleY;
    public float velocidadeParaVoltarAoNormal;

    void SquishySquashy(){
        if(sprite.transform.localScale.x != defaultScaleX)
            sprite.transform.localScale = Vector2.Lerp(sprite.transform.localScale,new Vector3(defaultScaleX,sprite.transform.localScale.y,1),velocidadeParaVoltarAoNormal * Time.deltaTime);
        if(sprite.transform.localScale.y != defaultScaleY)
            sprite.transform.localScale = Vector2.Lerp(sprite.transform.localScale,new Vector3(sprite.transform.localScale.x,defaultScaleY,1),velocidadeParaVoltarAoNormal * Time.deltaTime);
    }

    public void Jump(){
        if(IsGrounded() && Hambuga){
            jump = true;
        }

        if(stuckIntoSomething && Cheesu){
            jogarQueijo = true;
        }
    }

    float lastPosition;
    
    public void DownDash(){

        AudioManager.instance.PlayAudioClip(3);

        dashAvaliable = false;

        sprite.transform.localScale = new Vector3(1,0.01f,1);

        rb.velocity = new Vector2(rb.velocity.x,0);
        rb.velocity = new Vector2(rb.velocity.x,-forcaDoPulo * Time.deltaTime);
        
        downDash = true;
    }

    public void BounceBack(){

        AudioManager.instance.PlayAudioClip(1);

        downDash = false;
        sprite.transform.localScale = new Vector3(1,0.1f,1);
        var totalDistance = transform.position.y - lastPosition;
        rb.velocity = new Vector2(rb.velocity.x,totalDistance * -multiplicadorDeBounce * Time.deltaTime);
    }
    
    public void ThrowTheCheesu(){

        AudioManager.instance.PlayAudioClip(6);

        jogarQueijo = false;
        stuckIntoSomething = false;

        sprite.transform.localScale = new Vector3(1,0.5f,1);

        rb.gravityScale = 1;
        rb.velocity = new Vector2(0,0);
        rb.velocity = eixoDoIndicador.transform.up * throwForce * Time.deltaTime;
    }

    public void JumpReduce(){
        if(rb.velocity.y > 0 && Hambuga){
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/divisaoPulo);
        }
    }

    bool groundFirstTime;

    public bool IsGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0f,Vector2.down, groundDistanceDetect,groundMask);

        if(raycastHit.collider != null){
            if(!groundFirstTime){
                groundFirstTime = true;
                sprite.transform.localScale = new Vector3(1,0.5f,1);
            }
        }
        else{
            groundFirstTime = false;
        }

        return raycastHit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(Cheesu){
            if(other.gameObject.CompareTag("NonSticky")){
                rb.velocity = new Vector2(0,0);
                rb.gravityScale = 1;
                stuckIntoSomething = false;
            }else{
                AudioManager.instance.PlayAudioClip(5);
                sprite.transform.localScale = new Vector3(1,0.5f,1);
                rb.velocity = new Vector2(0,0);
                rb.gravityScale = 0;
                stuckIntoSomething = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Spike")){
            AudioManager.instance.PlayAudioClip(2);
            MenuManager.instance.ReloadScene();
        }

        if(other.CompareTag("Porta")){
            MenuManager.instance.CarregarProxLevel();
        }
    }

    private void OnEnable() {
        controle.Enable();
    }

    private void OnDisable() {
        controle.Disable();
    }
}
