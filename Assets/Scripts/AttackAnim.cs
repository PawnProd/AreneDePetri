using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour {

    public Vector3 targetScale;

	// Use this for initialization
	void Start () {
		
	}
	
    public void BeginAnim()
    {
        StartCoroutine(CO_Scale());
    }

    IEnumerator CO_Scale()
    {
        targetScale = targetScale * 0.37f;
        float diff = targetScale.x - transform.localScale.x;
        float ratio = diff / 0.1f;
        float timeToAchieved = ratio * 0.001f;
        float alphaChange = 1 * timeToAchieved;
        Debug.Log("Diff = " + diff);
        Debug.Log("Ratio = " + ratio);
        Debug.Log("Timeto = " + timeToAchieved);
        Debug.Log("Alpha Change = " + alphaChange);
        while (transform.localScale.x < targetScale.x && transform.localScale.y < targetScale.y)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0);
            Color color = GetComponent<SpriteRenderer>().color;
            color.a -= alphaChange;
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.001f);
        }

        Destroy(gameObject);
    }
}
