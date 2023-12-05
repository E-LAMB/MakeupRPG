using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{

    public Vector2 push_direction;
    public LayerMask interactables;
    public float intensity = 1f;

    public Transform self;

    public Node my_node;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (my_node != null)
        {
            if (my_node.charge_time > 0f)
            {
                RaycastHit2D ray_data = Physics2D.Raycast(self.position, push_direction, 5f, interactables);

                if (ray_data.collider != null)
                {
                    if (ray_data.collider.gameObject.GetComponent<Rigidbody2D>())
                    {
                        ray_data.collider.gameObject.GetComponent<Rigidbody2D>().velocity += (push_direction * intensity);
                    }
                }
            }

        } else
        {
            RaycastHit2D ray_data = Physics2D.Raycast(self.position, push_direction, 5f, interactables);

            if (ray_data.collider != null)
            {
                if (ray_data.collider.gameObject.GetComponent<Rigidbody2D>())
                {
                    ray_data.collider.gameObject.GetComponent<Rigidbody2D>().velocity += (push_direction * intensity);
                }
            }
        }

    }
}
