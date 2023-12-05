using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTile : MonoBehaviour
{

    public static bool currently_building;
    public static int menu_open;

    public bool space_to_build;

    public GameObject prefab_building;

    public Transform build_trans;
    public GameObject build_object;
    public SpriteRenderer confirmation;

    public KeyCode[] key_start;

    public Camera player_cam;

    public LayerMask blockers;
    public LayerMask destructable;

    public Vector3 previous_build;
    public float time_since_build;
    public float build_held;

    public GameObject[] potential_items;
    public float scroll_select;
    public int chosen_item;

    public TextMesh showup;
    public TextMesh connect_text;

    public Transform folder;

    public Sprite[] sprite_types;

    public Node[] connecting_nodes;

    public LayerMask nodes;

    // Start is called before the first frame update
    void Start()
    {
        previous_build = new Vector3(0f, -200f, 20f);

        prefab_building = potential_items[chosen_item];
        showup.text = prefab_building.name;

        menu_open = 0;
    }

    // Update is called once per frame
    void Update()
    {

        scroll_select += Input.mouseScrollDelta.y * -1f;

        if (scroll_select > 1f) {scroll_select = 0f; chosen_item += 1; if (chosen_item > potential_items.Length - 1) {chosen_item = 0;} prefab_building = potential_items[chosen_item]; showup.text = prefab_building.name;}
        if (0f > scroll_select) {scroll_select = 0f; chosen_item -= 1; if (chosen_item < 0) {chosen_item = potential_items.Length - 1;} prefab_building = potential_items[chosen_item]; showup.text = prefab_building.name;}


        for (int i = 0; i < key_start.Length; i++)
        {
            if (Input.GetKeyDown(key_start[i]))
            {
                if (i + 1 == menu_open)
                {
                    menu_open = 0;
                    currently_building = false;
                    connecting_nodes[0] = null;
                    connecting_nodes[1] = null;
                } else
                {
                    currently_building = true;
                    menu_open = i + 1;
                    connecting_nodes[0] = null;
                    connecting_nodes[1] = null;
                }
            }
        }

        confirmation.enabled = currently_building;

        if (menu_open != 0) {confirmation.sprite = sprite_types[menu_open - 1];}

        if (currently_building && (menu_open == 1 || menu_open == 2))
        {
            build_trans.position = player_cam.ScreenToWorldPoint(Input.mousePosition);
            build_trans.position = new Vector3 (Mathf.Round(build_trans.position.x), Mathf.Round(build_trans.position.y), 0f);
            if (menu_open == 1) {showup.color = new Vector4 (1f, 1f, 1f, 1f);} else {showup.color = new Vector4 (1f, 1f, 1f, 0f);}
            if (menu_open == 2) {connect_text.color = new Vector4 (1f, 1f, 1f, 1f);} else {connect_text.color = new Vector4 (1f, 1f, 1f, 0f);}
        } else
        {
            showup.color = new Vector4 (1f, 1f, 1f, 0f);
            connect_text.color = new Vector4 (1f, 1f, 1f, 0f);
        }

        if (menu_open == 1)
        {
            space_to_build = !Physics2D.OverlapCircle(build_trans.position, 0.4f, blockers);
            if (space_to_build) {confirmation.color = Color.green;} else {confirmation.color = Color.red;}
        } else if (menu_open == 2)
        {
            space_to_build = Physics2D.OverlapCircle(build_trans.position, 0.4f, nodes);
            if (space_to_build) {confirmation.color = Color.green;} else {confirmation.color = Color.red;}
        } else 
        {
            space_to_build = false;
            if (space_to_build) {confirmation.color = Color.green;} else {confirmation.color = Color.red;}
        }

        time_since_build += Time.deltaTime;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {build_held += Time.deltaTime;} else {build_held = 0f; previous_build = new Vector3(0f, -200f, 20f);}

        if (menu_open == 2)
        {
            if (connecting_nodes[0] == null)
            { connect_text.text = "Select Start";}
            else if (connecting_nodes[1] == null)
            { connect_text.text = "Select End";}
        }

        if (Input.GetMouseButton(0) && menu_open == 2)
        {
            if (space_to_build)
            {
                Collider2D node_chosen = Physics2D.OverlapCircle(build_trans.position, 0.4f, nodes);

                if (connecting_nodes[0] == null)
                {
                    if (node_chosen != null)
                    {
                        if (node_chosen.gameObject.GetComponent<Node>().node_type != 1)
                        {
                            connecting_nodes[0] = node_chosen.gameObject.GetComponent<Node>();
                        }
                    }

                } else if (connecting_nodes[1] == null)
                {
                    if (node_chosen != null)
                    {
                        if (node_chosen.gameObject.GetComponent<Node>() != connecting_nodes[0])
                        {
                            if (node_chosen.gameObject.GetComponent<Node>().node_type != 3)
                            {
                                connecting_nodes[1] = node_chosen.gameObject.GetComponent<Node>();
                            }
                        }
                    }

                    if (connecting_nodes[0] != null && connecting_nodes[1] != null)
                    {
                        connecting_nodes[0].ConnectNode(connecting_nodes[1]);
                        //Debug.Log("Connection Made");
                        connecting_nodes[0] = connecting_nodes[1];
                        connecting_nodes[1] = null;
                    }

                } 
            }
        }
        if (Input.GetMouseButton(1) && menu_open == 2)
        {
            if (space_to_build)
            {
                Collider2D node_chosen = Physics2D.OverlapCircle(build_trans.position, 0.4f, nodes);

                if (node_chosen.gameObject.GetComponent<Node>())
                {
                    node_chosen.gameObject.GetComponent<Node>().EraseConnections();
                    connecting_nodes[0] = null;
                    connecting_nodes[1] = null;
                }
            } else
            {
                connecting_nodes[0] = null;
                connecting_nodes[1] = null;
            }
        }

        if ((menu_open == 1) && ((currently_building && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) || (currently_building && (Input.GetMouseButton(0) || Input.GetMouseButton(1)) && build_held > 0.25f) && Vector3.Distance(build_trans.position, previous_build) > 0.2f))
        {
            if (Input.GetMouseButton(0) && menu_open == 1)
            {
                if (space_to_build)
                {
                    time_since_build = 0f;
                    GameObject ob = Instantiate(prefab_building, build_trans);
                    previous_build = build_trans.position;
                    ob.transform.parent = folder;
                    // Debug.Log("Prefabbed");
                }
            } else
            {
                if (Input.GetMouseButton(1) && menu_open == 1)
                {
                    Collider2D my_blocker = Physics2D.OverlapCircle(build_trans.position, 0.4f, destructable);
                    if (my_blocker != null)
                    {
                        if (my_blocker.gameObject.tag == "Player")
                        {
                            if (PlayerController.players > 1)
                            {
                                if (my_blocker.gameObject.GetComponent<PlayerController>().my_cam.active_player == my_blocker.gameObject)
                                {
                                    build_held = -0.5f;
                                }
                                previous_build = my_blocker.gameObject.transform.position;
                                Destroy(my_blocker.gameObject);
                            }
                        } else if (my_blocker.gameObject.tag == "Indestructable Node")
                        {

                        } else
                        {
                            previous_build = my_blocker.gameObject.transform.position;
                            Destroy(my_blocker.gameObject);
                        }
                    }
                }
            }
        }

    }
}
