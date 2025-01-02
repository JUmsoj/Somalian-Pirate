using Mono.Cecil.Cil;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private UIDocument doc;
    private Button button;
    public static int difficulty;
    
    private FloatField field;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        button = (Button)doc.rootVisualElement.Q("Play");
        field = doc.rootVisualElement.Q<FloatField>("Thing");
        
        button.RegisterCallbackOnce<ClickEvent>((Event) => SceneManager.LoadScene("Ship"));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        difficulty = (int)field.value;
    }
}
