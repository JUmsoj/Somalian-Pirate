using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class PlayerScript : MonoBehaviour
{
    [SerializeField] bool grounded = false;
    Rigidbody2D rb;
    private InputSystem_Actions _actions;
    [SerializeField] private float speed;
    UIDocument doc;
    public int health
    {
        get;
        set;
    } = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnDestroy()
    {
        KillScreen();
    }
    void KillScreen()
    {
        
        UIDocument screen = new GameObject("KillScreen").AddComponent<UIDocument>();
        screen.panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
        screen.panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
        screen.panelSettings.themeStyleSheet = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>("Assets/UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme.tss");
        screen.visualTreeAsset = Resources.Load<VisualTreeAsset>("KillScreen");
        foreach (var i in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID))
        {
            if (i.name != "Main Camera" && i.name != "KillScreen") Destroy(i);
        }
        screen.gameObject.AddComponent<Sigma>().StartCoroutine(enumerator());
        
    }
    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(5);
        Destroy(GameObject.Find("KillScreen"));
        SceneManager.LoadScene("MainMenu");
    }
    private void Awake() 
    {
        doc = GameObject.FindFirstObjectByType<UIDocument>();
        _actions = new();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        _actions.Player.Move.Enable();
        _actions.Player.Jump.Enable();
        _actions.Player.Jump.performed += (ctx) =>
        {
            rb.AddForceY(2000);
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
        doc.rootVisualElement.Q<Label>("Health").text = $"Health: {health}";
        if (health <= 0) Destroy(gameObject);
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
            rb.linearVelocityY = -1f;
        }
       
    }
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.CompareTag("Ship"))
        {
            Debug.Log("thing");
            grounded = true;
        }
        else if (hit.gameObject.name == "bullet(Clone)")
        {
            health -= 20;
            Debug.LogError(health);
            Destroy(hit.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Debug.Log("Bro");
            grounded = false;
        }
        
    }
}
public class Sigma : MonoBehaviour
{

}
