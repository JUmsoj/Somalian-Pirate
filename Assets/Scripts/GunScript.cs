using System;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GunScript : MonoBehaviour
{
    private BoxCollider2D Collider;
    private Rigidbody2D rb;
    public static bool melee { get; set; } = false;
    private const int speed = 3;
    public static int dir { get; set; } = -1;
    GameObject bullet;
    private Animator anim;
    private int amm;
    public int ammo { get
        {
            return amm;
        }
        set
        {
            if (SceneManager.GetActiveScene().name == "Ship")
            {
                try
                {
                    amm = value;
                    GameplayUIScript.UpdateHud<Label>(GameObject.FindFirstObjectByType<UIDocument>(), "Ammunition", amm);
                }
                catch(Exception)
                {

                }
            }
        }
    }
    private InputSystem_Actions controls;
    public bool active { get; set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        ammo = 10;
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
    void Melee(InputAction.CallbackContext ctx)
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        Collider = gameObject.AddComponent<BoxCollider2D>();
        anim.SetTrigger("melee");
        melee = true;
    }
    void PullBack()
    {
        Destroy(rb);
        Destroy(Collider);
        melee = false;
    }
    private void Update()
    {
        gameObject.SetActive(active);
    }
    private void OnEnable()
    {
        controls.Player.melee.Enable();
        controls.Player.melee.performed += Melee;
        controls.Player.Shoot.Enable();
        controls.Player.Shoot.performed += Shoot;
    }
    private void OnDisable()
    {
        controls.Player.melee.Disable();
        controls.Player.melee.performed -= Shoot;
        controls.Player.Shoot.Disable();
        controls.Player.Shoot.performed -= Shoot;
    }
    void Shoot(InputAction.CallbackContext ctx)
    {
        if (ammo > 0 && !melee)
        {
            anim.SetTrigger("shoot");
            
        }
        
    }
    
    public void Other()
    {
        
       
        var Bullet = Instantiate(bullet, position: new Vector2(gameObject.transform.position.x + (dir), gameObject.transform.position.y), rotation: Quaternion.identity);
        
        Rigidbody2D rb = Bullet?.GetComponent<Rigidbody2D>();
        Bullet.AddComponent<BulletScript>();
        rb.gravityScale = 0.1f;
        if (rb != null) Debug.Log("Hey");
        rb.AddForce(new Vector2(dir * 500, 0));
        ammo--;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    
}
public class BulletScript : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ship"))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning($"Hit {collision.gameObject.name}");
        }
    }
}
