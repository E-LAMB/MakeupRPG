using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public int node_type;
    // 1 = Only accepts inputs
    // 2 = Acts as a sort of conduit, Passing Signals
    // 3 = Only gives an output

    public Node[] connected_nodes;

    public SpriteRenderer my_sprite;
    public Color col_a;
    public Color col_d;

    public float charge_time;
    public bool always_generates;
    public bool be_active_when_disconnected;

    public GameObject[] pointer;

    public void TransferSignal(Node input_node, float power)
    {

        if (node_type == 1)
        {
            charge_time = 1f;
        }

        if (node_type == 2)
        {
            
            if (power - 0.1f > charge_time)
            {
                charge_time = power - 0.1f;
            }

            for (int i = 0; i < connected_nodes.Length; i++)
            {
                if (input_node != connected_nodes[i])
                {
                    if (connected_nodes[i] != null)
                    {
                        if (connected_nodes[i].charge_time > 0f)
                        {
                            connected_nodes[i].TransferSignal(this, charge_time);
                        }
                    }
                }
            }
        }
    }

    public void EraseConnections()
    {
        for (int i = 0; i < connected_nodes.Length; i++)
        {
            if (null != connected_nodes[i])
            {
                connected_nodes[i] = null;
            }
        }
    }

    public void ConnectNode(Node new_node)
    {
        if (node_type == 1)
        {
            // Debug.Log("Can't.");
        }

        if (node_type == 2)
        {
            connected_nodes[0] = new_node;
        }

        if (node_type == 3)
        {
            bool connection_found = false;
            for (int i = 0; i < connected_nodes.Length; i++)
            {
                if (connected_nodes[i] == null)
                {
                    connection_found = true;
                    connected_nodes[i] = new_node;
                }
            }
            if (!connection_found) {connected_nodes[Random.Range(0, connected_nodes.Length)] = new_node;}
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (be_active_when_disconnected && connected_nodes[0] == null) {charge_time = 1f;}
        if (node_type == 3 && always_generates) {charge_time = 1f;}

        if (charge_time > 0f)
        {
            charge_time -= Time.deltaTime;
            my_sprite.color = col_a;

            for (int i = 0; i < connected_nodes.Length; i++)
            {
                if (connected_nodes[i] != null)
                {
                    if (node_type == 3 && always_generates) {charge_time = 1f;}
                    // Debug.Log("Something here");
                    connected_nodes[i].TransferSignal(this, charge_time);
                }
            }

        } else
        {
            my_sprite.color = col_d;
        }

        if (pointer.Length > 0)
        {
            for (int i = 0; i < pointer.Length; i++)
            {
                if (connected_nodes[i] != null)
                {
                    pointer[i].SetActive(true);
                    pointer[i].transform.LookAt(connected_nodes[i].gameObject.transform.position);

                } else
                {
                    pointer[i].SetActive(false);
                }
            }
        }
    }
}
