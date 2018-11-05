using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public string playerName;

    public AthController ath;
    public Mutation mutation;

    public AudioSource sourceClonage;
    public AudioSource sourceClick;

    public GameObject panelInfo;

    public Color playerColor;

    public Cell selectedCell;
    public GameObject clone;
    public Cell cloneCell;
    public Cell hoverCell;
    public Cell stemCell;

    public List<Cell> cells = new List<Cell>();

    public int playerID;
    public bool isPlaying = false;

    public int bonusPoints;

    // Stats pour fin de partie
    public int totalCellsKilled = 0;
    public int totalGivenDamages = 0;
    public int totalTakenDamages = 0;
    public int totalHeal = 0;
    public int totalBonusPoints = 0;

    private void Start()
    {
        ath = AthController.Instance;
    }


    private void Update()
    {
        if(isPlaying)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
            
            if(clone == null)
                CheckHoverOnCell(mousePosWorld);

            if (Input.GetMouseButtonDown(0))
            {
                if (cloneCell != null && !cloneCell.GetComponent<CheckCollisionCell>().outPetri && cloneCell.GetComponent<CheckCollisionCell>().nbCollide == 0)
                {
                    isPlaying = false;
                    DivideCell();

                }
                else
                {
                    CheckRaycastWithCell(mousePosWorld);
                }
            }

            if (clone != null)
            {
                MoveCloneWithMouse();
            }

            if(Input.GetMouseButtonDown(1))
            {
                DestroyClone();
            }
        }
     
    }

    public void CheckRaycastWithCell(Vector2 mousePosWorld)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosWorld, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && clone == null)
            {
                if (hit.collider.CompareTag("Cell"))
                {
                    if (hit.collider.GetComponent<Cell>().owner == playerID && hit.collider.GetComponent<Cell>().isAlive)
                    {
                        sourceClick.Play();
                        selectedCell = hit.collider.GetComponent<Cell>();
                        CreateClone();
                        hoverCell.hideRange();
                        panelInfo.SetActive(false);
                        hoverCell = null;
                    }

                }
            }

        }
    }

    public void CheckHoverOnCell(Vector2 mousePosWorld)
    {
        Debug.Log("Layer = " + LayerMask.GetMask("ClickLayer"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosWorld, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("ClickLayer"));

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("Collide with = " + hit.collider.name);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Cell"))
                {
                    if(hoverCell != null)
                    {
                        hoverCell.hideRange();
                        hoverCell = null;
                        panelInfo.SetActive(false);
                    }
                    
                    hoverCell = hit.collider.GetComponent<Cell>();
                    hoverCell.showRange();
                    panelInfo.SetActive(true);
                    float offset = 0;

                    if(hoverCell.owner == 1)
                    {
                        offset = 8.5f;
                    }
                    else if (hoverCell.owner == 2)
                    {
                        offset = -3;
                    }
                    Vector3 newPos = new Vector3(hoverCell.transform.parent.position.x + offset, hoverCell.transform.parent.position.y + 2f, 0);
                    panelInfo.GetComponent<PanelInfo>().SetPosition(newPos);
                    panelInfo.GetComponent<PanelInfo>().SetTextParam(hoverCell.life, hoverCell.maxLife, hoverCell.armor, hoverCell.actionRadius, hoverCell.power);
                }
            }
            else
            {
                hoverCell.hideRange();
                panelInfo.SetActive(false);
                hoverCell = null;
            }

        }

        if(hits.Length == 0 && hoverCell != null)
        {
            hoverCell.hideRange();
            panelInfo.SetActive(false);
            hoverCell = null;
        }
    }

    public void DestroyClone()
    {
        Destroy(clone.gameObject);
        clone = null;
        selectedCell = null;
    }

    public void DivideCell()
    {
        sourceClonage.Play();
        clone.GetComponent<CellRenderer>().ChangeAlpha(0);

        Transform cellObj = selectedCell.transform.parent;
        Vector3 directionRot = clone.transform.position - cellObj.transform.position;

        float rotationZ = Mathf.Atan2(directionRot.y, directionRot.x) * Mathf.Rad2Deg;
        cellObj.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        clone.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        cellObj.GetComponent<CellRenderer>().PlayDivideAnim();
    }

    public void PutClone()
    {
        clone.GetComponent<CellRenderer>().ChangeAlpha(selectedCell.transform.parent.GetComponent<CellRenderer>().noyauColor.a);
        cloneCell.owner = playerID;
        cloneCell.onPetri = true;
        cloneCell.actionRadiusTransform.gameObject.SetActive(true);

        Destroy(cloneCell.GetComponent<CheckCollisionCell>());
        cells.Add(cloneCell);
        PrepareMutation();
        clone = null;
        cloneCell = null;
        
    }

    public void PrepareMutation()
    {
        Debug.Log("Bonus Point = " + bonusPoints);
        ath.ShowMutationPanel();
        mutation.Init(cloneCell, selectedCell, bonusPoints);
        bonusPoints = 0;
    }

    public void CreateClone()
    {
        clone = Instantiate(selectedCell.transform.parent.gameObject, selectedCell.transform.position, Quaternion.identity, selectedCell.transform.parent.parent);
        clone.name = clone.name.Replace("(Clone)", "");
        clone.transform.rotation = selectedCell.transform.parent.rotation;

        cloneCell = clone.transform.GetChild(0).GetComponent<Cell>();

        cloneCell.gameObject.AddComponent<CheckCollisionCell>().selectedCell = selectedCell;

        cloneCell.actionRadiusTransform.gameObject.SetActive(false);
        cloneCell.onPetri = false;

        clone.GetComponent<CellRenderer>().ChangeAlpha(0.5f);
    }

    public void MoveCloneWithMouse()
    {
        Transform cellObj = selectedCell.transform.parent;

        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosWorld.z = 0;

        Vector3 direction = mousePosWorld - cellObj.transform.position;
        direction = direction.normalized;
        direction *= selectedCell.transform.localScale.x;

        clone.transform.position = cellObj.transform.position + direction;
    }
}
