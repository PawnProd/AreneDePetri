using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRadius : MonoBehaviour {

    public Cell masterCell;

    public float alphaShown;
    public float alphaHiden = 0.05f;

	// Use this for initialization
	void Start () {
        hideRange();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Ajouter une cell de la liste dans laquelle il faut la stocker lorsque celle-ci entre dans le collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cell"))
        {
            Cell cell = collision.gameObject.GetComponent<Cell>();
            if (cell.owner != 0 && cell.owner != masterCell.owner)
            {
                masterCell.ennemyCells.Add(cell);
            }
            else if (cell.owner != 0 && cell.owner == masterCell.owner)
            {
                masterCell.friendlyCells.Add(cell);
            }
        }
    }

    // Enlever une cell de la liste dans laquelle elle est sortie lorsque celle-ci sort du collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cell"))
        {
            Cell cell = collision.gameObject.GetComponent<Cell>();
            if (cell.owner != 0 && cell.owner != masterCell.owner)
            {
                masterCell.ennemyCells.Remove(cell);
            }
            else if (cell.owner != 0 && cell.owner == masterCell.owner)
            {
                masterCell.friendlyCells.Remove(cell);
            }
        }
    }

    public void showRange()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = alphaShown;
        GetComponent<SpriteRenderer>().color = tmp;
    }

    public void hideRange()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = alphaHiden;
        GetComponent<SpriteRenderer>().color = tmp;
    }

}
