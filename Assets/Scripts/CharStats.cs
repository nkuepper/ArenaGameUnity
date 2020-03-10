using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour
{
    #region StatGetSets
    public float HealthStat { get; set; }
    public float DamageStat { get; set; }
    public float StamStat { get; set; }
    public float StamRegenStat { get; set; }
    public float StamDrain { get; set; }
    public float SpeedStat { get; set; }
    public float AttackRange { get; set; }
    public float AttackCooldown { get; set; }
    public float MaxHit { get; set; }
    public float DashStamDrain { get; set; }
    public float ExpWorth { get; set; }
    public float ExpNeeded { get; set; }
    public float Level { get; set; }
    public float Armor { get; set; }
    #endregion

    #region StatStatics
    public float mHealthStat { get; }
    public float mDamageStat { get; }
    public float mStamStat { get; }
    public float mStamRegenStat { get; }
    public float mStamDrain { get; }
    public float mSpeedStat { get; }
    public float mAttackRange { get; }
    public float mAttackCooldown { get; }
    public float mMaxHit { get; }
    public float mDashStamDrain { get; }
    public float mExpWorth { get; }
    public float mExpNeeded { get; }
    public float mLevel { get; }
    public float mArmor { get; }
    #endregion

    private Text TxtLevel;

    public CharStats()
    { 
        mHealthStat = 10f;
        mDamageStat = 1f;
        mStamStat = 300f;
        mStamRegenStat = 1f;
        mStamDrain = 50f;
        mSpeedStat = 10f;
        mAttackRange = 0.5f;
        mAttackCooldown = 1f;
        mMaxHit = 3f;
        mDashStamDrain = 60f;
        mExpWorth = 50f;
        mExpNeeded = 100f;
        mLevel = 1f;
        mArmor = 0f;

        HealthStat = mHealthStat;
        DamageStat = mDamageStat;
        StamStat = mStamStat;
        StamRegenStat = mStamRegenStat;
        StamDrain = mStamDrain;
        SpeedStat = mSpeedStat;
        AttackRange = mAttackRange;
        AttackCooldown = mAttackCooldown;
        MaxHit = mMaxHit;
        DashStamDrain = mDashStamDrain;
        ExpWorth = mExpWorth;
        ExpNeeded = mExpNeeded;
        Level = mLevel;
        Armor = mArmor;
    }

    private void Start()
    {
        if (name == "Player")
            TxtLevel = transform.GetChild(2).GetChild(2).GetComponent<Text>();
    }

    public void GiveExp(float amount)
    {
        if (amount > (ExpNeeded- ExpWorth))
        {
            ExpNeeded = Mathf.Pow(ExpNeeded, 1 + Level/5) + ExpWorth;
            LevelUp();
        }
        ExpWorth += amount;
    }

    public void LevelUp()
    {
        HealthStat += Random.value;
        DamageStat += Random.value;
        StamStat += Random.value * 10;
        SpeedStat = Mathf.Clamp(SpeedStat + Random.value, SpeedStat, 50f); // set a maximum

        TxtLevel.text = (int.Parse(TxtLevel.text) + 1).ToString();
    }
}
