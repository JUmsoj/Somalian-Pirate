using UnityEditor.Build.Player;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyOnFootScript : MonoBehaviour
{
    [SerializeField] private float cooldown = 2;
    Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] Vector2 move;
    [SerializeField] private GameObject player;
    [SerializeField] private float attack_cooldown = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
       player = GameObject.FindFirstObjectByType<PlayerScript>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    void Shoot()
    {
        bool dir = gameObject.transform.position.x <= player.transform.position.x;
        rb.AddForce((player.transform.position-gameObject.transform.position ));
        GameObject gun = transform.GetChild(0).gameObject;
        Debug.LogError("Shot");
        gun.transform.rotation = dir ? Quaternion.Euler(new Vector3(180, -180, 180)) : Quaternion.Euler(new Vector3(0, -180));
        GameObject bullet = Instantiate(Resources.Load<GameObject>("bullet"), new Vector2(gun.transform.position.x+3, gun.transform.position.y), dir ? Quaternion.Euler(new Vector3(180, -180, 180)) : Quaternion.Euler(new Vector3(0, -180)));
        Rigidbody2D rigidbody2d = bullet.GetComponent<Rigidbody2D>();
        rigidbody2d.gravityScale = 0.1f;
        if (dir) rigidbody2d.AddForceX(10000);
        
        else
        {
            rigidbody2d.AddForceX(-10000);
        }
        GetComponentInChildren<Animator>().SetTrigger("Shoot");
    }
    // Update is called once per frame
    void Update()
    {
        attack_cooldown -= Time.deltaTime;
        if(attack_cooldown <= 0)
        {
            Shoot();
            
            attack_cooldown = 5;
        }
        move = player.transform.position - gameObject.transform.position;
        rb.linearVelocityX = (move.x*speed)/5;
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Triangle")
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                collision.gameObject.GetComponent<PlayerScript>().health-=10;
                cooldown = 2;
                Debug.LogWarning(collision.gameObject.GetComponent<PlayerScript>().health);
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($" {gameObject.name} Collided with {collision.gameObject.name}");
        if (collision.gameObject.name == "bullet(Clone)") Destroy(gameObject);
        if (collision.gameObject.name == "Triangle") collision.gameObject.GetComponent<PlayerScript>().health--;
    }

}
