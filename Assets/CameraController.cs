using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform self;
    public Vector3 offset;

    public GameObject active_player;

    // Update is called once per frame
    void Update()
    {

        if (active_player != null)
        {
            self.position = active_player.transform.position + offset;
            if (self.position.y < 0f)
            {
                self.position = new Vector3(self.position.x, 0f, self.position.z);
            }
        } else
        {
            active_player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
