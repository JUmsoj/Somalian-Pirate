using System.Data.SqlTypes;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class OilBarrelScript : MonoBehaviour
{
    private static bool alreadpickedip = false;
    private InputSystem_Actions controls;
    private static int cash = 0;
    public static int money
    {
        get
        {
            return cash;
        }
        set { 
            cash = value;
            if (SceneManager.GetActiveScene().name == "Ship")
                GameplayUIScript.UpdateHud<Label>(GameObject.FindAnyObjectByType<UIDocument>(), "Money", cash);
        }
    }
    [SerializeField] private bool picked_up = false;
    private GameObject player;
    [SerializeField] private float timer = 5;
    private GunScript gun;
    private Rigidbody2D body;
    private float distance;
    private BoxCollider2D _collider_;
    private UnityEngine.UIElements.Label label;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Pick_Up(InputAction.CallbackContext ctx)
    {
        if (distance < 10 && !alreadpickedip)
        {
            alreadpickedip=true;
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
        if (picked_up && alreadpickedip)
        {
            alreadpickedip = false;
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
    void Spawn(int times)
    {
        for(int i = 0; i < times; i++)
        {
            float random_x = UnityEngine.Random.Range(0, 10);
            var barrel = Instantiate<GameObject>(Resources.Load<GameObject>("OilBarrel"), new Vector2(random_x, 5), Quaternion.identity);
            barrel.GetComponent<Rigidbody2D>().AddForceY(-20);
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
    private void Start()
    {
        label = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement.Q<UnityEngine.UIElements.Label>("Money");
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
        if (picked_up && alreadpickedip)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                money++;
                timer = 5;
                label.text = $"MONEY: {money}";
                gun.active = true;
                if(!gun.isActiveAndEnabled)
                {
                    gun.gameObject.SetActive(true);
                }
                Debug.LogError(money);
                Spawn(money);
                alreadpickedip = false;
                Destroy(gameObject);
            }
        }
    }
}
