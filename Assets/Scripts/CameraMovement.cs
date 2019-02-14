using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 pivot;
    private Transform player;

    public float yOffset = 2.0f;
    public float zOffset = 3.0f;
    public float orbitSpeed = 20.0f;
    public float yPivotOffset = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        offset = new Vector3(player.position.x, player.position.y + yOffset, player.position.z + zOffset);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime, Vector3.up) * offset;
        pivot = new Vector3(player.position.x, player.position.y + yPivotOffset, player.position.z);
        transform.position = player.position + offset;
        transform.LookAt(pivot);
    }
}
