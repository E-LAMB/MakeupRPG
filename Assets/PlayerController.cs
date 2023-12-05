using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float flight_speed;

    public static int players;

    public Rigidbody2D rb;
    public SpriteRenderer rend;

    public Transform self;

    public bool currently_active;
    public CameraController my_cam;
    
    // Start is called before the first frame update
    void Start()
    {
        my_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        players += 1;
    }

    void OnDestroy()
    {
        players -= 1;
    }

    void OnMouseDown()
    {
        //Debug.Log("Clc");
        if (!currently_active && !BuildTile.currently_building)
        {
            my_cam.active_player = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

        currently_active = my_cam.active_player == gameObject;

        if (currently_active)
        {
            rend.color = Color.white;

        } else
        {
            rend.color = Color.red;
        }

        if (currently_active)
        {
            if (Input.GetAxisRaw("Horizontal") != 0) {rend.flipX = (Input.GetAxisRaw("Horizontal") < 0f);}

            rb.velocity = new Vector2((Input.GetAxisRaw("Horizontal") * speed), rb.velocity.y);
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, flight_speed);
            }
        } else
        {
            rb.velocity = new Vector2(rb.velocity.x / 1.01f, rb.velocity.y);
            rend.flipX = (rb.velocity.x < 0f);
        }
    }
}
