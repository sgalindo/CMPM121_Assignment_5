using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Salvador Galindo
 * sagalind
 * CMPM 121 - Assignment 5
 * Door.cs - Script for opening/animating the red door.
 */
public class Door : MonoBehaviour
{
    private Animator doorAnim;
    private ParticleSystem doorParts;
    // Start is called before the first frame update
    void Start()
    {
        doorAnim = GetComponent<Animator>();
        doorParts = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenDoor()
    {
        doorAnim.SetBool("DoorOpen", true);
    }

    public void ExplodeDoor()
    {
        doorParts.Play();
    }
}
