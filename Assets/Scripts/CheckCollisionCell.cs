using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionCell : MonoBehaviour {

    public Cell selectedCell;
    public GameObject cellTrigger;
    public int nbCollide = 0;

    public bool outPetri = true;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collide with " + collision.name);
        if(collision.CompareTag("Cell"))
        {
            if(collision.gameObject!= selectedCell.gameObject)
            {
                nbCollide++;
            }
        }

        if(collision.CompareTag("Petri"))
        {
            outPetri = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cell"))
        {
            nbCollide--;
        }

        if(collision.CompareTag("Petri"))
        {
            outPetri = true;
        }
    }
}
