using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    public Node my_node;
    public LayerMask interactables;
    public Vector2 observant_direction;
    public bool saw_something;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        saw_something = Physics2D.Raycast(gameObject.transform.position, observant_direction, 2f, interactables);
        if (saw_something)
        {
            my_node.charge_time = 1f;
        } else
        {
            my_node.charge_time = 0f;
        }
    }
}
