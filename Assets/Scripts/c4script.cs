
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class c4script : MonoBehaviour
{
    private Vector2 frozenat;
    bool frozen;
    private InputAction blowthisup;
    private static UIDocument document;
    private InputSystem_Actions action;
    private GameObject shrapnel;
    private float cooldown = 10;
    private SpawnScript spawnScript;
    private static int _c4S;
    public static int C4s { 
    get
        { 
            return _c4S;
        } 
    set
        {
            _c4S = value;
            GameplayUIScript.UpdateHud<Label>(document ?? GameObject.FindFirstObjectByType<UIDocument>(), "C4s", _c4S);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        document = GameObject.FindFirstObjectByType<UIDocument>();  
        shrapnel = Resources.Load<GameObject>("Shrapnel");
        action = new();
        action.Disable();
        blowthisup = action.asset.FindActionMap("Player").AddAction("BlowUp", InputActionType.Button, "<Keyboard>/f");
        action.Enable();
        blowthisup.performed += Explode;
        spawnScript = ScriptableObject.CreateInstance<SpawnScript>();
        Freeze(gameObject.transform.position);
    }
    
    void Start()
    {
        if(frozen)
        {
            GetComponent<Rigidbody2D>().position = frozenat;
        }
        if (cooldown > 0)
        {
            cooldown-=Time.deltaTime;

        }
        else
        {
            Explode();
        }
    }
    void Explode(InputAction.CallbackContext ctx )
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForceX(5);
        rb.Sleep();
        spawnScript.Spawn(5, transform.position, shrapnel);
        action.Disable();
        Destroy(gameObject);
        frozen = false;
    }
    void Explode()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForceX(5);
        rb.Sleep();
        spawnScript.Spawn(5, transform.position, shrapnel);
        action.Disable();
        Destroy(gameObject);
    }
    void Freeze(Vector2 pos)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        frozenat = pos;
        body.position = frozenat;
        frozen = true;
        body.bodyType = RigidbodyType2D.Kinematic;
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
