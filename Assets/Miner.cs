using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{

    public GameObject[] spawn_cycle;
    public float timer;
    public float timer_activation;

    public Transform folder;
    public Transform output_position;

    public int current;

    public Node has_node;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (has_node != null) {if (has_node.charge_time > 0f) {timer += Time.deltaTime;}} else {timer += Time.deltaTime;}

        if (timer_activation < timer) 
        {
            timer -= timer_activation;

            GameObject chosen = spawn_cycle[current];
            GameObject created = Instantiate(chosen, output_position);

            current += 1;
            if (current > spawn_cycle.Length - 1) {current = 0;}

            created.GetComponent<Rigidbody2D>().velocity += new Vector2(Random.Range(-2f, 2f),10f);

            created.transform.parent = folder;

        }

    }
}
