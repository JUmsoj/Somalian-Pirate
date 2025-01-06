using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameplayUIScript : MonoBehaviour
{
    private VisualElement root;
    private UIDocument doc;
    [SerializeField] bool settings = false;
    private InputSystem_Actions actions;
    private Button exitmainmenu;
    private Button exitgame;
    private VisualTreeAsset HUD;
    private VisualTreeAsset Settings;
    private Button medkit;
    private Button ammo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        Settings = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Resources/Settings.uxml");
        HUD = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIStuff/doc.uxml");
        OnHUD();
        


        actions = new();
        actions.Player.Exiting.Enable();
        actions.Player.Exiting.performed += (ctx) => {
            settings = !settings;
            Debug.LogError("performed");
            if (settings)
            {
                OnSettings();
               
            }
            else
            {

                OnHUD();
                
            }
        };
    }
    void OnSettings()
    {


        doc.visualTreeAsset = Settings;
        root = doc.rootVisualElement;
        exitmainmenu = root.Q<Button>("ExitMainMenu");
        exitgame = root.Q<Button>("ExitGame");
        Callbacks("pause");


    }
    private void Callbacks(string ui)
    {
        switch (ui.ToLower())
        {
            case "hud":
                if (exitgame != null)
                {
                    exitgame.UnregisterCallback<ClickEvent>(e =>
                    {
                        Application.Quit();
                    });
                    exitmainmenu.UnregisterCallback<ClickEvent>((e) =>
                    {
                        SceneManager.LoadScene("MainMenu");
                    });
                }
                medkit.RegisterCallback<ClickEvent>(evt => PowerUp("medkit"));
                ammo.RegisterCallback<ClickEvent>((evt) => PowerUp("ammo"));
                break;

            case "pause":
                ammo.UnregisterCallback<ClickEvent>((evt) => PowerUp("ammo"));

                medkit.UnregisterCallback<ClickEvent>(e => PowerUp("medkit"));
                exitmainmenu.RegisterCallbackOnce<ClickEvent>((e) =>
                {
                    GameObject.FindFirstObjectByType<PlayerScript>().thing = true;
                    SceneManager.LoadScene("MainMenu");
                });
                exitgame.RegisterCallbackOnce<ClickEvent>(e =>
                {
                    Application.Quit();
                });
                break;

        }
    }
    void OnHUD()
    {


        doc.visualTreeAsset = HUD;
        root = doc.rootVisualElement;
        ammo = root.Q<VisualElement>("PowerUps").Q<Button>("PowerUp1");
        medkit = root.Q<VisualElement>("PowerUps").Q<Button>("PowerUp2");
        Callbacks("hud");



    }
    public static void UpdateHud<T>(UIDocument doc, string name,  float val) where T : Label
    {
        Label element = doc.rootVisualElement.Q<T>(name);
        if (element != null && doc)
        {
            switch(name) {
                case "Ammunition":
                    element.text = $"Ammo: {val}";
                    break;
                case "Health":
                    element.text = $"Health :{val}";
                    break;
                case "Money":
                    element.text = $"Money: {val}";
                    break;
                case "Grenades":
                    element.text = $"Grenades: {val}";
                    break;
                
            }

        }
    }
    void PowerUp(string ammo)
    {
        switch (ammo.ToLower()) {
            case "ammo":
                if (OilBarrelScript.money - 10 <= 0)
                {

                    OilBarrelScript.money -= 10;
                    GameObject.FindFirstObjectByType<GunScript>().ammo = 10;
                    Debug.LogError("Hey");
                }
                break;


            case "medkit":
                if (OilBarrelScript.money - 20 >= 0)
                {
                    OilBarrelScript.money -= 20;
                    GameObject.FindFirstObjectByType<PlayerScript>().health = 100;
    
                }
            break;
        }


           
    
    }
           
        
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
