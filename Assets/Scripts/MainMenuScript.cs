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
        
        if(!button.IsUnityNull())button.RegisterCallbackOnce<ClickEvent>((Event) => SceneManager.LoadScene("Ship"));
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
