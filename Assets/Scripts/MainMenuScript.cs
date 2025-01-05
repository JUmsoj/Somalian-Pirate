using Mono.Cecil.Cil;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private UIDocument doc;
    private Button button;
    public static int difficulty;
    public static int high_score { get; set; } = 0;
    private FloatField field;
    private Label label;
    [SerializeField] bool testing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        
        if (testing)
        {
            gameObject.AddComponent<Sigma>();
            Destroy(this);
            return;
        }
       
        doc = GetComponent<UIDocument>();
        button = (Button)doc.rootVisualElement.Q("Play");
        field = doc.rootVisualElement.Q<FloatField>("Thing");
        if (high_score < OilBarrelScript.money)
        {
            high_score = OilBarrelScript.money;
            doc.rootVisualElement.Q<Label>("HighScore").text = $"HighScore: {high_score}";
        }
        OilBarrelScript.money = 0;
        if (!button.IsUnityNull())button.RegisterCallbackOnce<ClickEvent>((Event) => SceneManager.LoadScene("Ship"));
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
