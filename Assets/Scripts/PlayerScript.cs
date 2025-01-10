using Mono.Cecil.Cil;
using System;
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
    private InputAction plant;
    
    public int health
    {
        get;
        set;
    } = 100;
    public bool thing = false;
    private GameObject c4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public static void KillScreen()
    {
        GameObject player = null;
      IEnumerator enumerator()
        {
            yield return new WaitForSeconds(40);
            Destroy(GameObject.Find("KillScreen"));
            SceneManager.LoadScene("MainMenu");
        }
        UIDocument screen = new GameObject("KillScreen").AddComponent<UIDocument>();
        
        
        NewDocument(screen, Resources.Load<VisualTreeAsset>("KillScreen"));
        foreach (var i in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID))
        {
            if (i.name != "Main Camera" && i.name != "KillScreen") {
                if (i.name != "Triangle")
                {
                    Destroy(i);
                }
                else
                {
                    player = i;
                }

            }
        }
        screen.gameObject.AddComponent<Sigma>().StartCoroutine(enumerator());
        Destroy(player);
    }
    static void NewDocument(UIDocument doc, VisualTreeAsset asset, ThemeStyleSheet theme = null)
    {
        doc.panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
        doc.panelSettings.themeStyleSheet = theme != null ? theme : AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>("Assets/UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme.tss");
        doc.panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
        doc.visualTreeAsset = asset;
    }
    private void Awake() 
    {

        c4 = Resources.Load<GameObject>("C4");
        c4script.C4s = 3;
        GrenadeScript.grenades = 3;
        doc = GameObject.FindFirstObjectByType<UIDocument>();
        _actions = new();
        rb = GetComponent<Rigidbody2D>();
        plant = _actions.Player.Plant;
        plant.Enable();
        plant.performed += Plant;
        
       
    }
    void Plant(InputAction.CallbackContext ctx)
    {
        if (c4script.C4s > 1)
        {
            Instantiate(c4, gameObject.transform.position, Quaternion.identity);
            c4script.C4s--;
            Debug.LogWarning("Bomb has been planted");
        }
    }
    private void OnEnable()
    {
        _actions.Player.Move.Enable();
        _actions.Player.Jump.Enable();
        _actions.Player.Jump.performed += (ctx) =>
        {
            rb.AddForceY(2000);
        };
        _actions.Player.Throw.Enable();
        _actions.Player.Throw.performed += (ctx) =>
        {
            if (GrenadeScript.grenades > 0)
            {
                Instantiate(Resources.Load<GameObject>("Grenade"), gameObject.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(GunScript.dir * 10, 4));
                GrenadeScript.grenades--;
            }
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

        if (doc.rootVisualElement.Q<Label>("Health") != null) GameplayUIScript.UpdateHud<Label>(doc, "Health", health);
        if (health <= 0)
        {
            KillScreen();
        }
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
            rb.linearVelocityY = -3f;
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
        UIDocument doc;
        if (!completed && cooldown <= 0)
        {
            doc = gameObject.GetComponent<UIDocument>();

            completed = true;
            

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
