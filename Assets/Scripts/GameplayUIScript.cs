using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameplayUIScript : MonoBehaviour
{
    private UIDocument doc;
    [SerializeField] bool settings=false;
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
            Debug.LogError ("performed");
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
        exitmainmenu = doc.rootVisualElement.Q<Button>("ExitMainMenu");
        exitgame = doc.rootVisualElement.Q<Button>("ExitGame");
        Callbacks(false);


    }
    private void Callbacks(bool hud)
    {
        if(hud)
        {
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
            medkit.RegisterCallback<ClickEvent>(evt => PowerUp(false));
            ammo.RegisterCallback<ClickEvent>((evt) => PowerUp(true));

        }
        else if(!hud)
        {
            ammo.UnregisterCallback<ClickEvent>((evt) => PowerUp(true));

            medkit.UnregisterCallback<ClickEvent>(e => PowerUp(false));
            exitmainmenu.RegisterCallbackOnce<ClickEvent>((e) =>
            {
                GameObject.FindFirstObjectByType<PlayerScript>().thing = true;
                SceneManager.LoadScene("MainMenu");
            });
            exitgame.RegisterCallbackOnce<ClickEvent>(e =>
            {
                Application.Quit();
            });
        }
    }
    void OnHUD()
    {

        
        doc.visualTreeAsset = HUD;
        ammo = doc.rootVisualElement.Q<VisualElement>("PowerUps").Q<Button>("PowerUp1");
        medkit = doc.rootVisualElement.Q<VisualElement>("PowerUps").Q<Button>("PowerUp2");
        Callbacks(true);



    }
    void PowerUp(bool ammo)
    {
        if(ammo)
        {
            if (OilBarrelScript.money - 10 <= 0)
            {

                OilBarrelScript.money -= 10;
                GameObject.FindFirstObjectByType<GunScript>().ammo = 10;
                Debug.LogError("Hey");
            }
        }
        else
        {
            medkit.RegisterCallback<ClickEvent>(evt =>
            {
                if (OilBarrelScript.money - 20 >= 0)
                {
                    OilBarrelScript.money -= 20;
                    GameObject.FindFirstObjectByType<PlayerScript>().health = 100;

                }
            });
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
