using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Salvador Galindo
 * sagalind
 * CMPM 121 - Assignment 5
 * Collectible.cs - Script for individual collectibles (purple cubes).
 */
public class Collectible : MonoBehaviour
{
    [HideInInspector] public bool paused = false;
    private ParticleSystem parts;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        parts = GetComponent<ParticleSystem>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && paused == false)
        {
            Collect();
        }
    }

    private void Collect()
    {
        float delay = 0f;
        if (!parts.isPlaying)
            parts.Play();
        GetComponent<Renderer>().enabled = false;
        delay = parts.main.duration + parts.main.startLifetime.constant;
        Destroy(this.gameObject, delay);
        gm.IncreaseScore(1);

    }
}
