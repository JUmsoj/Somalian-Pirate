using UnityEngine;
using UnityEngine.UIElements;

public class GrenadeScript : MonoBehaviour
{
    private SpawnScript spawn;
    private float cooldown = 10;
    private static float grenade;
    public static float grenades 
    { 
        get
        {

            return grenade;
        }
        set
        {
            grenade=value;
            GameplayUIScript.UpdateHud<Label>(GameObject.FindFirstObjectByType<UIDocument>(), "Grenades", grenade);
        } 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        spawn = ScriptableObject.CreateInstance<SpawnScript>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown-=Time.deltaTime;
        }
        else
        {
            spawn.Spawn(5, gameObject.transform.position, Resources.Load<GameObject>("Shrapnel"));
            Destroy(gameObject);
        }
    }
}
