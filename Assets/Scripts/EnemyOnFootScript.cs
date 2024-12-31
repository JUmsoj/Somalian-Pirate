using UnityEngine;

public class EnemyOnFootScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] Vector2 move;
    [SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
       player = GameObject.FindFirstObjectByType<PlayerScript>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        move = player.transform.position - gameObject.transform.position;
        rb.linearVelocityX = (move.x*speed)/5;
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($" {gameObject.name} Collided with {collision.gameObject.name}");
        if (collision.gameObject.name == "bullet(Clone)") Destroy(gameObject);
        else if (collision.gameObject.name == "Triangle") Destroy(collision.gameObject);
    }

}
