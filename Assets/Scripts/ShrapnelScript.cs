using UnityEngine;

public class ShrapnelScript : MonoBehaviour
{
    Vector2 blow_up;
    Rigidbody2D body;
    [SerializeField] bool onground = false;
    [SerializeField] float lifespan;
    [SerializeField] private float decomposition = 5;
    private void Update()
    {
        lifespan += Time.deltaTime;
        if(onground )
        {
            decomposition -= Time.deltaTime;
            if(decomposition <= 0 )
            {
                Destroy(gameObject);
            }
        }
    }
    private void Awake()
    {
        Debug.Log("Created Shrapnel");
        body = GetComponent<Rigidbody2D>();
        gameObject.AddComponent(typeof(BoxCollider2D));
        blow_up = ShellScript.NewRandomVector2(-5, 5, 0, 10) * (3 * 3);
        for (int  i = 0; i < 10; i++)
        {
            body.AddForce(blow_up);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Triangle" && !onground) Destroy(collision.gameObject);
        else if (collision.gameObject.CompareTag("Ship") && lifespan > 2) onground = true;
    }
}
