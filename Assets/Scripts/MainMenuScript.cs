using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private UIDocument doc;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        button = (Button)doc.rootVisualElement.Q("Play");
        button.RegisterCallbackOnce<ClickEvent>((Event) => SceneManager.LoadScene(0));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
