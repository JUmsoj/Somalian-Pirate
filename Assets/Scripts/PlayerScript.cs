using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerScript : MonoBehaviour
{
    [SerializeField] bool grounded = false;
    Rigidbody2D rb;
    private InputSystem_Actions _actions;
    [SerializeField] private float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnDestroy()
    {
        Application.Quit();
    }
    private void Awake()
    {
        _actions = new();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        _actions.Player.Move.Enable();
        _actions.Player.Jump.Enable();
        _actions.Player.Jump.performed += (ctx) =>
        {
            rb.AddForceY((100*2)^2);
        };
    }
    private void OnDisable()
    {
        _actions.Player.Move.Disable();
        _actions.Player.Jump.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 move = _actions.Player.Move.ReadValue<Vector2>()*speed;
        if(move != Vector2.zero) Debug.Log(move);
        
        // GetOrAddComponent
        // GetOrAddComponent
        rb.linearVelocityX = move.x;
        
        Gravity();

    }
    void Gravity()
    {
        if (!grounded)
        {
            rb.linearVelocityY = -2f;
        }
       
    }
    private void OnCollisionEnter2D(Collision2D hit)
    {
        Debug.Log("thing");
        grounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Bro");
            grounded = false;
        
    }
}
