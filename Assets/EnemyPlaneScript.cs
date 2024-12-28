using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPlaneScript : MonoBehaviour
{
    Rigidbody2D rb;
    static SpawnScript spawnScript;
    [SerializeField] float cooldown = 2;
    [SerializeField] private float cooldownforparadrop;
    [SerializeField] GameObject enemy;
    [SerializeField] private int kamakize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        kamakize = UnityEngine.Random.Range(0, 1);
        if (kamakize == 1)
        {
            
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 3;
            gameObject.AddComponent<BoxCollider2D>();
            for(int i  = 0; i < 10; i++)
            {
                BANZAIIII(GameObject.FindFirstObjectByType<PlayerScript>().gameObject, 4, 4);
            }
            spawnScript = ScriptableObject.CreateInstance<SpawnScript>();
            return;
        }
        enemy = Resources.Load<GameObject>("Enemy");
        if (spawnScript.IsUnityNull()) { spawnScript = ScriptableObject.CreateInstance<SpawnScript>(); }
        if (GetComponent<Rigidbody2D>() != null)
        {
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 0;
            rb.linearVelocityX = 3;
        }
    }
    void Update()
    {
        if (kamakize == 0)
        {
            if (cooldownforparadrop > 0)
            {
                cooldownforparadrop -= Time.deltaTime;
            }
            else
            {
                ParaDrop();
                cooldownforparadrop = 5 / Mathf.Floor(spawnScript.frequency);
            }
            if (gameObject.transform.position.x > 20)
            {
                spawnScript.Spawn(gameObject, UnityEngine.Random.Range(5, 10));
                Destroy(gameObject);
            }
            if (cooldown > 0) cooldown -= Time.deltaTime;
            else DropTheBomb();
        }
        else
        {
            cooldown = 0;
            cooldownforparadrop = 0;
        }
        
    }
    void BANZAIIII(GameObject player, params int[] multipliers)
    {
        UnityEngine.Vector2 force = player.transform.position - gameObject.transform.position;
        if (rb != null)
        {
            for (int i = 0; i < multipliers.Length; i++)
            {
                force *= multipliers[i];
            }
            rb.AddForce(force);
        }
        else
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

            BANZAIIII(player);
            return;
        }
    }
    void DropTheBomb()
    {
        Instantiate(Resources.Load("Shell"), position: gameObject.transform.position, rotation: UnityEngine.Quaternion.identity).GetComponent<Rigidbody2D>()?.AddForceY(-6);
        cooldown = 3/Mathf.Floor(spawnScript.frequency);
    }
    void ParaDrop()
    {
        GameObject paratrooper = Instantiate(enemy, position: gameObject.transform.position, rotation: UnityEngine.Quaternion.identity);
        var paratrooper_rb = paratrooper.GetComponent<Rigidbody2D>();
        paratrooper_rb.gravityScale = paratrooper_rb.gravityScale <= 0 ? paratrooper_rb.gravityScale : 2f;
        paratrooper_rb.AddForceY(-4);
        spawnScript.frequency+=0.5f;
    }
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameobject = collision.gameObject;
        if (gameobject.name == "Triangle" && kamakize == 1)
        {

            Destroy(gameobject);
            spawnScript.Spawn(gameObject, UnityEngine.Random.Range(5, 10));

        }
        else if (gameobject.CompareTag("Ship")) { Destroy(gameObject); }
    }
}
