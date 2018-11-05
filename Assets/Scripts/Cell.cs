using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public CellRenderer render;

    public GameObject prefabAttack;

    public AudioSource sourceDead;


    public int life = 0;
    public int maxLife = 0; // max life of this cell
    public int leftArmor = 0;
    public int armor = 0;
    public int actionRadius = 0;
    public int power = 0;
    public bool isHealer = false;

    public float minScale = 1f;
    public float maxScale = 4f;

    public float minRange = 0.2f;
    public float maxRange = 8f;
    
    public float alphaMinLife = 0.2f;

    public bool onPetri = false;

    public bool isAlive;

    public int owner;

    public Color deadCellColor;

    public List<Cell> friendlyCells;
    public List<Cell> ennemyCells;

    public Transform actionRadiusTransform;
    Vector3 actionRadiusScale;

    public GiveawayCell giveaway;

    // Use this for initialization
    void Start() {
        UpdateScales();
        render = transform.parent.GetComponent<CellRenderer>();
        isAlive = true;

        if(GetComponent<GiveawayCell>() != null)
        {
            giveaway = GetComponent<GiveawayCell>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (isAlive) {
            render.scale = transform.localScale;
            if (life <= 0) {
                if (giveaway != null) {
                    giveaway.givePointsToKiller();
                }
                DyingCell();
            }
        }
    }

    public void ReconstructArmor() {
        leftArmor = armor;
    }

    // Fonction pour soigner les cells alliées à porté
    public void Heal() {
        if (isAlive) {
            List<int> indexToRemove = new List<int>();
            for (int i = 0; i < friendlyCells.Count; ++i)
            {
                Cell cell = friendlyCells[i];

                if (cell == null)
                {
                    indexToRemove.Add(i);
                }
                else
                {
                    cell.TakeHealing(power);
                }
            }

            foreach (int index in indexToRemove)
            {
                friendlyCells.RemoveAt(index);
            }
        }

        

        AttackAnim attackAnim = Instantiate(prefabAttack, transform.position, Quaternion.identity, transform.parent).GetComponent<AttackAnim>();
        attackAnim.GetComponent<SpriteRenderer>().color = new Color32(118, 234, 7, 255);
        attackAnim.targetScale = actionRadiusTransform.localScale;
        attackAnim.BeginAnim();

    }

    // Fonction pour attaquer les cells ennemies à porté
    public void Attack() {
        List<int> indexToRemove = new List<int>();
        bool allDead = true;
        for (int i = 0; i < ennemyCells.Count; ++i)
        {
            Cell cell = ennemyCells[i];

            if(cell == null)
            {
                indexToRemove.Add(i);
            }
            else
            {
                if(cell.isAlive)
                {
                    allDead &= false;
                }
                cell.TakeDamages(power);
            }
        }

        foreach(int index in indexToRemove)
        {
            ennemyCells.RemoveAt(index);
        }
        if(!allDead)
        {
            Debug.Log("Pop Onde !");
            AttackAnim attackAnim = Instantiate(prefabAttack, transform.position, Quaternion.identity, transform.parent).GetComponent<AttackAnim>();
            attackAnim.targetScale = actionRadiusTransform.localScale;
            attackAnim.BeginAnim();
        }

    }

    // Fonction pour encaisser des domages
    public void TakeDamages(int amount) {
        if(life > 0)
        {
            if (isAlive)
                transform.parent.GetComponent<CellRenderer>().PlayTakeDamageAnim();

            if (leftArmor - amount >= 0)
            {
                leftArmor -= amount;
            }
            else
            {
                life -= (amount - leftArmor);
                leftArmor = 0;
            }

            if (owner == 1)
            {
                GameController.Instance.player2.totalGivenDamages += amount - leftArmor;
                GameController.Instance.player1.totalTakenDamages += amount - leftArmor;
            }
            else
            {
                GameController.Instance.player1.totalGivenDamages += amount - leftArmor;
                GameController.Instance.player2.totalTakenDamages += amount - leftArmor;
            }


            ModifyAlpha();

            if (life <= 0)
            {
                if (giveaway != null)
                {
                    giveaway.givePointsToKiller();
                }
                DyingCell();
            }
        }
        
    }

    // Fonction permettant de se faire soigner
    public void TakeHealing(int amount) {
        life += amount;
        int tmp = 0;
        if (life > maxLife) {
            tmp = life - maxLife;
            life = maxLife;
        }
        ModifyAlpha();
        GameController.Instance.activPlayer.totalHeal += amount - tmp;
    }

    // Dans le doute de si y'en a besoin pour le List.Remove
    public override bool Equals(object obj) {
        if (obj == null) return false;
        Cell objAsCell = obj as Cell;
        if (objAsCell == null) return false;
        else return objAsCell == this;
    }

    public void showRange() {
        if (onPetri)
            actionRadiusTransform.GetComponent<ActionRadius>().showRange();
    }

    public void hideRange() {
        if (onPetri)
            actionRadiusTransform.GetComponent<ActionRadius>().hideRange();
    }

    private float LifeToAlpha() {
        float percent = life / maxLife;
        float tmp = (1 - alphaMinLife) * life / maxLife;
        return alphaMinLife + tmp;
    }

    private void ModifyAlpha() {
        transform.parent.GetComponent<CellRenderer>().ChangeAlpha(LifeToAlpha());
    }

    private void DyingCell() {
        isAlive = false;
        if (owner == 1) {
            GameController.Instance.player2.totalCellsKilled += 1;
        } else if (owner == 2) {
            GameController.Instance.player1.totalCellsKilled += 1;
        } else if (owner == 3) {
            GameController.Instance.activPlayer.totalCellsKilled += 1;
        }
        sourceDead.Play();
        transform.parent.GetComponent<CellRenderer>().SetDead();
        
    }

    public void UpdateScales() {
        float veryMaxLife = GameController.Instance.activPlayer.stemCell.maxLife;
        float alphaSquare = (maxScale * maxScale - minScale * minScale) / veryMaxLife;
        float alpha = Mathf.Sqrt(alphaSquare);
        float lifeOffset = minScale * minScale / alphaSquare;

        float scaling = alpha * Mathf.Sqrt(maxLife + lifeOffset);
        transform.localScale = new Vector3(scaling, scaling, 1);

        // scale of the radius of the effects of the cell (attack or heal)
        float range = minRange + (maxRange - minRange) * actionRadius / veryMaxLife;
        float scale = scaling + 2 * range;
        actionRadiusScale = new Vector3(scale, scale, 0);
        actionRadiusTransform.localScale = actionRadiusScale;
    }
    
    public int MaxLife {
        get {
            return maxLife;
        }
        set {
            maxLife = value;
            UpdateScales();
        }
    }

    //public int Life {
    //    get {
    //        return life;
    //    }
    //    set {
    //        life = value;
    //        // TODO update cell appearence
    //    }
    //}

    //public int LeftArmor {
    //    get {
    //        return leftArmor;
    //    }
    //    set {
    //        leftArmor = value;
    //        // TODO update cell
    //    }
    //}

    public int Armor {
        get {
            return armor;
        }
        set {
            armor = value;
            leftArmor = armor;
        }
    }

    //public int ActionRadius {
    //    get {
    //        return actionRadius;
    //    }
    //    set {
    //        actionRadius = value;
    //        // TODO update cell appearence
    //    }
    //}

    //public int Power {
    //    get {
    //        return power;
    //    }
    //    set {
    //        power = value;
    //        // TODO update cell appearence
    //    }
    //}
    
}
