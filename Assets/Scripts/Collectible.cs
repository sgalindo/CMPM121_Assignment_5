using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [HideInInspector] public bool paused = false;
    private ParticleSystem parts;
    // Start is called before the first frame update
    void Start()
    {
        parts = GetComponent<ParticleSystem>();
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
    }
}
