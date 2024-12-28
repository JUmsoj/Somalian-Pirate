using UnityEngine;
using System;
using UnityEngine.Events;
public class ShellScript : MonoBehaviour
{
    [SerializeField] int shrapnel_amt;
    [SerializeField] bool Exploded = false;
    private GameObject shrapnel;
    private UnityEvent blow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private void Awake()
    {
        blow = new();
        shrapnel = Resources.Load<GameObject>("Shrapnel");
        rb = GetComponent<Rigidbody2D>();
        blow.AddListener(BlowUp);
    }
    void Start()
    {
        rb.AddForce(new Vector2(5, 2));
    }
    void BlowUp()
    {
        for (int i = 0; i < shrapnel_amt; i++)
        {
            Vector3 pos = gameObject.transform.position;
            Instantiate(shrapnel, position: new Vector2(pos.x + UnityEngine.Random.Range(0, 2), pos.y + UnityEngine.Random.Range(0, 2)), rotation: Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(NewRandomVector2(-5, 5, 0, 5));
        }
            blow.RemoveListener(BlowUp);
            Destroy(gameObject);
        
    }
    
    public static Vector2 NewRandomVector2(float min, float max)
    {
        return new Vector2(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }
    public static Vector2 NewRandomVector2(float minx, float maxx, float miny, float maxy)
    {
        return new Vector2(UnityEngine.Random.Range(minx, maxx), UnityEngine.Random.Range(miny, maxy));
    }
    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Ship"))
        {
            blow.Invoke(); 
        }
    }
}