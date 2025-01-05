using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
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
    public bool thing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnDestroy()
    {
        
         if(!thing) KillScreen();
    }
    public static void KillScreen()
    {
      IEnumerator enumerator()
        {
            yield return new WaitForSeconds(40);
            Destroy(GameObject.Find("KillScreen"));
            SceneManager.LoadScene("MainMenu");
        }
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

        if(doc.rootVisualElement.Q<Label>("Health") != null) doc.rootVisualElement.Q<Label>("Health").text = $"Health: {health}";
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
    private float cooldown = 1.0f; // Set your cooldown duration
    private bool completed = false;
    

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (!completed && cooldown <= 0)
        {
            var doc = gameObject.GetComponent<UIDocument>();

            completed = true;
            List<StylePropertyName> styles = new List<StylePropertyName>();
            styles.Add(new StylePropertyName("opacity"));

            foreach(VisualElement item in doc.rootVisualElement.Q<VisualElement>("visual").Children())
            {
                item.AddToClassList ("word-change");
                 IEnumerator Wait()
                {
                    yield return new WaitForSeconds(3f);
                    item.RemoveFromClassList("word-change");
                }
                StartCoroutine(Wait());
               
            }
        }
    }
   
    

}
