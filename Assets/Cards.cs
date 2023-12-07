using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{

    public int ap_cost;
    public int card_id;


    private void OnMouseDown()
    {
        if (card_id == 1)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(40f * Time.deltaTime, 65f * Time.deltaTime, 15f * Time.deltaTime));
    }
}
