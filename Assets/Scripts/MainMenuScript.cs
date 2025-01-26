using Mono.Cecil.Cil;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.ComponentModel;
using System.Linq;

public class MainMenuScript : MonoBehaviour
{
    private List<VisualTreeAsset> menudocs = new List<VisualTreeAsset>();
    
    public static int health_bought { get; private set; }
    public static int ammo_bought { get; set; }
    private UIDocument doc;
    private Button button, shop;
    private VisualElement market;
    public static int difficulty;
    private Label total_money;
    private int money;
    public static int high_score { get; set; } = 0;
    private FloatField field;
    private Label label;
    [SerializeField] bool testing;
    private Button x, add_10_ammo, add_10_health; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void HighScoreUpdate()
    {
        high_score = OilBarrelScript.money;
        doc.rootVisualElement.Q<Label>("HighScore").text = $"HighScore: {high_score}";
        money += OilBarrelScript.money;
        (total_money.text == "Money:" ? total_money : doc.rootVisualElement.Q<Label>("money")).text = $"Money:{money}";
        OilBarrelScript.money = 0;
    }
    private void Awake()
    {
        
        foreach(var x in FindAllObjectsOfType<VisualTreeAsset>("Assets/UIStuff"))
        {
            Debug.LogError(x != null ? "Added Item Succesfully" : "Error accesing item");
               
            menudocs.Add(x);
            Debug.LogError(menudocs.Last().name);
        }
        (menudocs[0], menudocs[1]) = (menudocs[1], menudocs[0]);
        Debug.LogWarning(menudocs != null ? "Exists" : "NonExists");
        
        if (testing)
        {
            gameObject.AddComponent<Sigma>();
            Destroy(this);
            return;
        }
        VisualElement root = doc.rootVisualElement;
        doc = GetComponent<UIDocument>();
        button = root.Q<Button>("Play");
        field = root.Q<FloatField>("Thing");
        total_money = root.Q<Label>("money");




        HighScoreUpdate();
        
        if (!button.IsUnityNull()) button.RegisterCallbackOnce<ClickEvent>(Event => SceneManager.LoadScene("Ship"));
        shop = PlayerScript.NewButtonElement<ClickEvent>(root, "SHOP", OpenShop);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void CloseShop(ClickEvent evt)
    {
        print("Hello World");
        
        x.UnregisterCallback<ClickEvent>(CloseShop);
        PlayerScript.NewDocument(doc, menudocs[1]); // The Shop Asset
        
        shop = PlayerScript.NewButtonElement<ClickEvent>(doc.rootVisualElement, "SHOP", OpenShop);
        HighScoreUpdate();
        print("Shop Closed");

        add_10_ammo.UnregisterCallback<ClickEvent>(Event => Pay("ammo", 10, 10));
        add_10_health.UnregisterCallback<ClickEvent>(evt => Pay("health", 10, 10));
        
    }
    void Pay(string s, int cost, int product)
    {
        
        if (money>= cost)
        {
            money -= cost;
            switch(s.ToLower())
            {
                case "ammo":
                    ammo_bought += product;
                    break;
                case "health":
                    health_bought += product;
                    break;
                
            }
        }
        
    }
    IEnumerable<T> FindAllObjectsOfType<T>(UnityEngine.Object[] arr) where T : UnityEngine.Object
    {
        
        foreach(var item in arr)
        {
           if (item is T t)
           {
               yield return t;
           }
        }
    }
    public static T LoadAssetFromGUID<T>(string id) where T : UnityEngine.Object
    {
        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(id));
    }
    IEnumerable<T> FindAllObjectsOfType<T>(string path) where T : UnityEngine.Object
    {
        List<UnityEngine.Object> obj = new List<UnityEngine.Object>();
       
        foreach(var thing in AssetDatabase.FindAssets($"t:{typeof(T).Name}", new string[] {path} ))
        {
            obj.Add(LoadAssetFromGUID<VisualTreeAsset>(thing));
        }
        Debug.LogError(obj.Count >= 1 ? "Assets Loaded" : $"Assets Not Loaded {obj.Count}");
        if (obj != null)
        {
            foreach (var item in obj)
            {
                yield return item as T;
            }
        }
    }
    


    void OpenShop(ClickEvent evt)
    {
        
            PlayerScript.NewDocument(doc, menudocs[0] != null ? menudocs[0] : null); // The Shop Asset
        Debug.Log("ey");

        x = x != null ? x : PlayerScript.NewElement<Button>(doc.rootVisualElement.Q<VisualElement>("closing"));
        x.text = "CLOSE";
        add_10_ammo = doc.rootVisualElement.Q<Button>("ammo");
        add_10_health = add_10_health != null ? add_10_health : PlayerScript.NewElement<Button>(add_10_ammo.parent);
        add_10_health.RegisterCallbackOnce<ClickEvent>(evt => Pay("health", 10, 10));
        add_10_ammo.RegisterCallbackOnce<ClickEvent>(evt => Pay("ammo", 10, 10));
        x.RegisterCallbackOnce<ClickEvent>(CloseShop);
        market = add_10_ammo.parent;
        
    }
}
