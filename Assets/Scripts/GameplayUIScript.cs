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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Settings = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Resources/Settings.uxml");
        HUD =  AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIStuff/doc.uxml");
        doc = GetComponent<UIDocument>();
        actions = new();
        actions.Player.Exiting.Enable();
        actions.Player.Exiting.performed += (ctx) => {
            settings = !settings;
            Debug.LogError ("performed");
            if (settings)
            {
                doc.visualTreeAsset = Settings;
                exitmainmenu = doc.rootVisualElement.Q<Button>("ExitMainMenu");
                exitgame = doc.rootVisualElement.Q<Button>("ExitGame");
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
            else
            {
                exitgame.UnregisterCallback<ClickEvent>(e =>
                {
                    Application.Quit();
                });
                exitmainmenu.UnregisterCallback<ClickEvent>((e) =>
                {
                    SceneManager.LoadScene("MainMenu");
                });
                doc.visualTreeAsset = HUD;

            }
        };
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
