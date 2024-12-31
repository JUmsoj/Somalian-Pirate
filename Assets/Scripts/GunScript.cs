using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class GunScript : MonoBehaviour
{
    private const int speed = 3;
    int dir = -1;
    GameObject bullet;
    private Animator anim;
    private int ammo = 10;
    private InputSystem_Actions controls;
    public bool active { get; set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        active = true;
        bullet = Resources.Load<GameObject>("bullet");
        controls = new();
        anim = GetComponent<Animator>();
        controls.Player.Axis.Enable();
        controls.Player.Axis.performed += (ctx) =>
        {
            dir = (int)controls.Player.Axis.ReadValue<float>()*speed;
            Debug.Log(dir);
            int temp = dir / 3;
            Debug.Log(temp);
            if(temp == 1)
            {
                gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.identity;
            }
        };
    }
    private void Update()
    {
        gameObject.SetActive(active);
    }
    private void OnEnable()
    {
        controls.Player.Shoot.Enable();
        controls.Player.Shoot.performed += Shoot;
    }
    private void OnDisable()
    {
        controls.Player.Shoot.Disable();
        controls.Player.Shoot.performed -= Shoot;
    }
    void Shoot(InputAction.CallbackContext ctx)
    {
        anim.SetTrigger("shoot");
        
        
    }
    
    public void Other()
    {
        Vector2 start_point = new Vector2(gameObject.transform.position.x+4, gameObject.transform.position.y);
        var Bullet = Instantiate(bullet, position: start_point, rotation: Quaternion.identity);
        
        Rigidbody2D rb = Bullet?.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (rb != null) Debug.Log("Hey");
        rb.linearVelocityX = dir*3;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    
}
