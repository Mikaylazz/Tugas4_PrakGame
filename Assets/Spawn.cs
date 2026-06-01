using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Spawn Object")]
    public GameObject Character;
    public Transform spawnposition;

    [Header("Spawn Setting")]
    [SerializeField] protected float spawndelay = 2f;
    [SerializeField] protected float spawninterval = 3f;

    [Header("Max Enemy")]
    public int maxEnemy = 3;

    private void Start()
    {
        InvokeRepeating(nameof(Spawner), spawndelay, spawninterval);
    }

    void Spawner()
    {
        Instantiate(Character, spawnposition.position, Quaternion.identity);
    } 
}
