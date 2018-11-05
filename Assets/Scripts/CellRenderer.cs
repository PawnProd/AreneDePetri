using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRenderer : MonoBehaviour {

    public GameObject noyau;
    public GameObject membrane;
    public GameObject reflet;
    public GameObject cytoplasme;

    public Color noyauColor;
    public Color membraneColor;
    public Color cytoplasmeColor;

    public Color noyauDeadColor;
    public Color membraneDeadColor;
    public Color cytoplasmeDeadColor;

    [HideInInspector]
    public Color refletColor;

    public Vector3 scale;

    public bool divide = false;

    private float timer = 0;


    // Use this for initialization
    void Start () {
        refletColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
        if(divide)
        {
            timer += Time.deltaTime;
            if (timer >= cytoplasme.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.3f)
            {
                divide = false;
                timer = 0;
                GameController.Instance.activPlayer.PutClone();
            }
           
        }
        SetColor();
        SetSize();
	}

    public void SetColor()
    {
        noyau.GetComponent<SpriteRenderer>().color = noyauColor;
        membrane.GetComponent<SpriteRenderer>().color = membraneColor;
        cytoplasme.GetComponent<SpriteRenderer>().color = cytoplasmeColor;
        reflet.GetComponent<SpriteRenderer>().color = refletColor;
    }

    public void SetDead()
    {
        noyauColor = noyauDeadColor;
        membraneColor = membraneDeadColor;
        cytoplasmeColor = cytoplasmeDeadColor;
        reflet.SetActive(false);

        noyau.GetComponent<Animator>().SetBool("IsDead", true);
        membrane.GetComponent<Animator>().SetBool("IsDead", true);
        cytoplasme.GetComponent<Animator>().SetBool("IsDead", true);
        reflet.GetComponent<Animator>().SetBool("IsDead", true);
    }


    public void SetPosition(Vector3 position)
    {
        cytoplasme.transform.position = position;
        membrane.transform.position = position;
        noyau.transform.position = position;
        reflet.transform.position = position;
    }

    public void SetSize()
    {
        noyau.transform.localScale = scale;
        membrane.transform.localScale = scale;
        cytoplasme.transform.localScale = scale;
        reflet.transform.localScale = scale;
    }

    public void PlayDivideAnim()
    {
        noyau.GetComponent<Animator>().SetTrigger("Divide");
        membrane.GetComponent<Animator>().SetTrigger("Divide");
        cytoplasme.GetComponent<Animator>().SetTrigger("Divide");
        reflet.GetComponent<Animator>().SetTrigger("Divide");

        divide = true;
    }

    public void PlayTakeDamageAnim()
    {
        Debug.Log("Take Damage ! ");
        noyau.GetComponent<Animator>().SetTrigger("TakeDamage");
        membrane.GetComponent<Animator>().SetTrigger("TakeDamage");
        cytoplasme.GetComponent<Animator>().SetTrigger("TakeDamage");
        reflet.GetComponent<Animator>().SetTrigger("TakeDamage");
    }

    public void ChangeAlpha(float alpha)
    {
        noyauColor.a = alpha;
        membraneColor.a = alpha;
        cytoplasmeColor.a = alpha;
        refletColor.a = alpha;
    }
}
