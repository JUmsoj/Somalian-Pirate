using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;
public class OilBarrelScript : MonoBehaviour
{
    private InputSystem_Actions controls;
    public static int money = 0;
    private bool picked_up = false;
    private GameObject player;
    [SerializeField] private float timer = 5;
    private GunScript gun;
    private Rigidbody2D body;
    private float distance;
    private BoxCollider2D _collider_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Pick_Up(InputAction.CallbackContext ctx)
    {
        if (distance < 10)
        {
            picked_up = true;
            gameObject.transform.parent = player.transform;
            gameObject.transform.position = gun.transform.position;
            gun.active = false;
            body.bodyType = RigidbodyType2D.Kinematic;
            _collider_.enabled = false;
        }
    }
    void Drop(InputAction.CallbackContext ctx)
    {
        if (picked_up)
        {
            picked_up = false;
            gameObject.transform.parent = null;
            gun.active = true;
            if (!gun.isActiveAndEnabled)
            {
                gun.gameObject.SetActive(true);
            }
            body.bodyType = RigidbodyType2D.Dynamic;
            _collider_.enabled=true;
        }
        
    }
    private void Awake()
    {
        body = gameObject.AddComponent<Rigidbody2D>();
        _collider_=gameObject.AddComponent<BoxCollider2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
        if (timer != 5) timer = 5;
        player = GameObject.FindFirstObjectByType<PlayerScript>().gameObject;
        gun = player.transform.GetChild(0).gameObject.GetComponent<GunScript>();
        controls = new();
    }
    private void OnEnable()
    {
        controls.Player.Drop.Enable();
        controls.Player.Pick_Up.Enable();
        controls.Player.Pick_Up.performed += Pick_Up;
        controls.Player.Drop.performed += Drop; 
    }
    private void OnDisable()
    {
        controls.Player.Drop.Disable();
        controls.Player.Pick_Up.Disable();
        controls.Player.Pick_Up.performed -= Pick_Up;
        controls.Player.Drop.performed -= Drop;
    }
    

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Abs(Vector2.Distance(gameObject.transform.position, player.transform.position));
        if (picked_up)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                money++;
                gun.active = true;
                if(!gun.isActiveAndEnabled)
                {
                    gun.gameObject.SetActive(true);
                }
                Debug.LogError(money);
                Instantiate(Resources.Load<GameObject>("OilBarrel"), position: new Vector2(-12, 6.14f), rotation: Quaternion.identity);
                
                Destroy(gameObject);
            }
        }
    }
}
