using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mutation : MonoBehaviour {

    public int minMaxLife = 1;

    public Cell mutatedCell;

    public GameObject goCell;

    public Transform cellAttackLogo;
    public Transform cellHealLogo;

    public Image pointRestantImg;

    public Slider sliderLife;
    public Slider sliderArmor;
    public Slider sliderAction;
    public Slider sliderPower;

    public TextMeshProUGUI textSliderLife;
    public TextMeshProUGUI textSliderArmor;
    public TextMeshProUGUI textSliderAction;
    public TextMeshProUGUI textSliderPower;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI powerText;

    public TextMeshProUGUI pointToAttributeText;

    public bool cellIsHealer = false;

    public int life = 0;
    public int maxLife = 0; // max life of this cell
    public int armor = 0;
    public int actionRadius = 0;
    public int power = 0;

    public int lastLifeSlideValue = 0;
    public int lastArmorSlideValue = 0;
    public int lastActionSlideValue = 0;
    public int lastPowerSlideValue = 0;

    public int pointToAttribute = 0;

    public Vector3 contactPoint;
    public Vector3 contactDirection;


    public void Init(Cell mutateCell, Cell parentCell, int bonusPoint)
    {
        Debug.Log("Init !");

        goCell = mutateCell.transform.parent.gameObject;
        GameObject goParentCell = parentCell.transform.parent.gameObject;

        contactPoint = (goCell.transform.position + goParentCell.transform.position) / 2;
        contactDirection = (goCell.transform.position - goParentCell.transform.position).normalized;

        mutatedCell = mutateCell;
        life = mutatedCell.life;
        maxLife = mutatedCell.maxLife;
        armor = mutatedCell.armor;
        actionRadius = mutatedCell.actionRadius;
        power = mutatedCell.power;
        cellIsHealer = mutatedCell.isHealer;

        SetCellTeamColor();


        SetHealer(cellIsHealer);

        lifeText.text = life.ToString();
        armorText.text = armor.ToString();
        actionText.text = actionRadius.ToString();
        powerText.text = power.ToString();

        pointToAttribute = 0;
        pointToAttribute += bonusPoint;
        pointToAttributeText.text = pointToAttribute + " PTS RESTANTS";

        SetPointRestantAmount();

        if (pointToAttribute != 0)
        {
            AthController.Instance.EnableOrDisableValidateMutationButton(false);
        }

        lastLifeSlideValue = 0;
        lastArmorSlideValue = 0;
        lastPowerSlideValue = 0;
        lastActionSlideValue = 0;

        sliderAction.value = 0;
        sliderArmor.value = 0;
        sliderLife.value = 0;
        sliderPower.value = 0;

        textSliderAction.text = sliderAction.value.ToString();
        textSliderArmor.text = sliderArmor.value.ToString();
        textSliderLife.text = sliderLife.value.ToString();
        textSliderPower.text = sliderPower.value.ToString();

        goCell.GetComponentInChildren<ActionRadius>().showRange();
    }

    public void CheckPointToAttribute()
    {
        if(pointToAttribute == 0)
        {
            AthController.Instance.EnableOrDisableValidateMutationButton(true);
        }
        else
        {
            AthController.Instance.EnableOrDisableValidateMutationButton(false);
        }
    }

    public void MutateCell()
    {
        int deltaLife = maxLife - mutatedCell.maxLife;
        if (deltaLife > 0) {
            life += deltaLife;
        }
        if (life > maxLife) {
            life = maxLife;
        }

        mutatedCell.isHealer = cellIsHealer;
        mutatedCell.life = life;
        mutatedCell.maxLife = maxLife;
        mutatedCell.armor = armor;
        mutatedCell.actionRadius = actionRadius;
        mutatedCell.power = power;

        mutatedCell.UpdateScales();

        goCell.GetComponentInChildren<ActionRadius>().hideRange();
    }

    public void SetCellTeamColor()
    {
        cellAttackLogo.GetChild(0).GetComponent<Image>().color = mutatedCell.transform.parent.GetComponent<CellRenderer>().membraneColor;
        cellAttackLogo.GetChild(1).GetComponent<Image>().color = mutatedCell.transform.parent.GetComponent<CellRenderer>().cytoplasmeColor;

        cellHealLogo.GetChild(0).GetComponent<Image>().color = mutatedCell.transform.parent.GetComponent<CellRenderer>().membraneColor;
        cellHealLogo.GetChild(1).GetComponent<Image>().color = mutatedCell.transform.parent.GetComponent<CellRenderer>().cytoplasmeColor;
    }

    public void SetHealer(bool healer)
    {
        cellIsHealer = healer;

        mutatedCell.isHealer = healer;

        if (cellIsHealer)
            mutatedCell.transform.parent.GetComponent<CellRenderer>().noyauColor = new Color32(118, 234, 7, 255);
        else {
            mutatedCell.transform.parent.GetComponent<CellRenderer>().noyauColor = new Color32(255, 30, 0, 255);
        }

        if (healer)
        {
            foreach(Transform child in cellAttackLogo)
            {
                Color color = child.GetComponent<Image>().color;
                color.a = 0.5f;
                child.GetComponent<Image>().color = color;
            }

            foreach (Transform child in cellHealLogo)
            {
                Color color = child.GetComponent<Image>().color;
                color.a = 1f;
                child.GetComponent<Image>().color = color;
            }
        }
        else
        {
            foreach (Transform child in cellAttackLogo)
            {
                Color color = child.GetComponent<Image>().color;
                color.a = 1f;
                child.GetComponent<Image>().color = color;
            }

            foreach (Transform child in cellHealLogo)
            {
                Color color = child.GetComponent<Image>().color;
                color.a = 0.5f;
                child.GetComponent<Image>().color = color;
            }
        }
    }

    public void SetPointRestantAmount()
    {
        float pourcent = (pointToAttribute * 0.3f) / 10;
        pointRestantImg.fillAmount = pourcent;
    }

    public void UpdateLife(Slider slider)
    {
        UpdateValuePoint(ref maxLife, (int)slider.value, ref lastLifeSlideValue, lifeText, textSliderLife, slider, minMaxLife);
        mutatedCell.MaxLife = maxLife;
        goCell.transform.position = contactPoint + contactDirection * mutatedCell.transform.localScale.x / 2;
    }

    public void UpdateArmor(Slider slider)
    {
        UpdateValuePoint(ref armor, (int)slider.value, ref lastArmorSlideValue, armorText, textSliderArmor, slider);
        mutatedCell.Armor = armor;
    }

    public void UpdateAction(Slider slider)
    {
        UpdateValuePoint(ref actionRadius, (int)slider.value, ref lastActionSlideValue, actionText, textSliderAction, slider);
        mutatedCell.actionRadius = actionRadius;
        mutatedCell.UpdateScales();
    }

    public void UpdatePower(Slider slider)
    {
        UpdateValuePoint(ref power, (int)slider.value, ref lastPowerSlideValue, powerText, textSliderPower, slider);
        mutatedCell.power = power;
    }

    public void UpdateValuePoint(ref int attribut, int inputValue, ref int lastSlideValue,TextMeshProUGUI textToUpdate, TextMeshProUGUI textSlider, Slider slider, int minAttribut = 0)
    {
        Debug.Log("Point to attribute = " + pointToAttribute);
        Debug.Log("Value = " + inputValue);


        int deltaPoint = inputValue - lastSlideValue;

        if (deltaPoint > pointToAttribute) {
            deltaPoint = pointToAttribute;
        }

        if (attribut + deltaPoint < minAttribut) {
            deltaPoint = minAttribut - attribut;
        }
        
        pointToAttribute -= deltaPoint;
        attribut += deltaPoint;

        lastSlideValue += deltaPoint;
        slider.value = lastSlideValue;

        textSlider.text = slider.value.ToString();

        pointToAttributeText.text = pointToAttribute + " PTS RESTANTS";
        textToUpdate.text = attribut.ToString();

        SetPointRestantAmount();

        CheckPointToAttribute();
    }
}
