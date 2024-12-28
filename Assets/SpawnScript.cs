using UnityEngine;
using System;
[CreateAssetMenu(fileName = "SpawnScript", menuName = "Scriptable Objects/SpawnScript")]
public class SpawnScript : ScriptableObject
{
    public int wave { get; set; }
    public float frequency { get; set; } = 1;
    public void Spawn(GameObject spawn, float yval)
    {
        frequency+=0.5f;
        wave++;
        for (int i = 0; i < wave; i++)
        {
            Instantiate(spawn).GetComponent<Transform>()
            .position = new Vector2(-5, yval);
           
        }

    }
}
