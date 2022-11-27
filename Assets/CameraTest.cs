using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Cube").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.player.position.x, this.player.position.y + 10, this.player.position.z + 10);
    }
}
