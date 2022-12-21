using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hpbar : MonoBehaviour
{
    const float maxHp = 100;
    const float maxAmount = 0.4f;

    private float currentHp;
    [SerializeField] float amountResult;

    

    public float changeHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            CalcBar();
        }
    }

    [SerializeField] UnityEngine.UI.Image bar;

    void Awake()
    {
        currentHp = maxHp;
    }
    void Start()
    {
        bar.fillAmount = maxAmount;
        GameMng.I.hpbar = this;
    }

    void CalcBar()
    {
        amountResult = currentHp / maxHp * maxAmount;

        bar.fillAmount = amountResult >= maxAmount ? maxAmount : amountResult;
    }
}
