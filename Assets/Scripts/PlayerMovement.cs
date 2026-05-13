using System;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool puedeMoverse, puedeSaltar;
    [SerializeField] private  bool saltando;
    public LayerMask layerPiso;
    public Animator anim;
    public float velocidadMovimiento;
    Rigidbody2D rb2d;
    float horizontal;
    float gravedad;
    public float tiempoEntrePasos;
    float tiempoUltimoPaso;
   [SerializeField] bool grounded;
    public Collider2D col2D;
    private InputAction jumpAction;
    private InputAction moveAction;
    bool checkCayendo;
    
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gravedad = Physics2D.gravity.y;
    }

    private void Start()
    {
        jumpAction  = InputSystem.actions.FindAction("Jump");
        moveAction  = InputSystem.actions.FindAction("Move");
        //CheckPointSystem.instance.ActualizarUltimaPos(transform.position);
    }

    private void Update()
    {
        Saltar();
        Moverse();
    }
    
    void Saltar()
    {
        if (!puedeSaltar) return;
        if(jumpAction.IsInProgress())
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x,  5f);
            StartCoroutine(CheckAterrizaje());
            saltando = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (col2D.IsTouchingLayers(layerPiso))
        {
           HaySuelo(true);
        }
    }

    void Moverse()
    {
        if (!puedeMoverse) return;

        if(!saltando && !grounded && !checkCayendo)
        {
            checkCayendo = true;
            StartCoroutine(CheckAterrizaje());
        }
    }

    private void FixedUpdate()
    {
        rb2d.linearVelocity = new Vector2(moveAction.ReadValue<Vector2>().x , rb2d.linearVelocity.y) ;
    }
    
    
    private void HaySuelo(bool state)
    {
        grounded = state;
    }
    private IEnumerator CheckAterrizaje()
    {
        yield return new WaitForSeconds(0.1f);

        while(!grounded)
        {
            yield return null;
        }
        saltando = false;
        checkCayendo = false;
    }

}