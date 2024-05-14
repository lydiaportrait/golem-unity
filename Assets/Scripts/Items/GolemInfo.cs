using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : UnityEvent<int,GlobalDefinitions.DamageTypes>
{

}
public class LocationEvent : UnityEvent<Location>
{

}
public class GolemInfo : MonoBehaviour
{
    public DamageEvent damageTaken = new DamageEvent();
    public DamageEvent damageResisted = new DamageEvent();
    public UnityEvent GolemDied = new UnityEvent();
    public LocationEvent RemoveListeners = new LocationEvent();
    public UnityEvent AddToItemRoller = new UnityEvent();
    public LocationEvent AddToLocationRoller = new LocationEvent();
    public LocationEvent EncounterAborted = new LocationEvent();
    public GolemConstruct golemConstruct;
    
    public bool isDead = false;
    public Location currentLocation;
    public List<GolemEffectBase> golemEffects = new List<GolemEffectBase>(); 
    public void CreateGolemEffects(List<GolemEffect> effects)
    {
        foreach(GolemEffect effect in effects)
        {
            GameObject g = Instantiate(ItemDatabase.Instance.GetGolemEffectByID(effect.EffectID), transform);
            GolemEffectInfo info = g.GetComponent<GolemEffectInfo>();
            golemEffects.Add(g.GetComponent<GolemEffectBase>());
            info.roll = effect.roll;
            info.roll2 = effect.roll2;
            info.id = effect.EffectID;
        }
    }
    public int GetStatValue(GlobalDefinitions.Stats stat)
    {
        Stat cStat = golemConstruct.stats[(int)stat];
        return Mathf.FloorToInt((cStat.currentBaseValue + cStat.currentModifier) * cStat.multiplier);
    }
    public void TakeDamage(int damage, GlobalDefinitions.DamageTypes damageType)
    {
        Stat resiStat;
        string dmgString = damageType.ToString();
        if (dmgString == "Physical")
            resiStat = golemConstruct.stats[(int)GlobalDefinitions.Stats.PhysicalResist];
        else if(dmgString == "Fire")
            resiStat = golemConstruct.stats[(int)GlobalDefinitions.Stats.FireResist];
        else if (dmgString == "Cold")
            resiStat = golemConstruct.stats[(int)GlobalDefinitions.Stats.ColdResist];
        else
            resiStat = golemConstruct.stats[(int)GlobalDefinitions.Stats.LightningResist];
        float resi = (resiStat.currentBaseValue + resiStat.currentModifier) * resiStat.multiplier;
        float resiPercent = resi - (float)Math.Truncate(resi);
        int finalResi = (int)resi;
        int resiThatHappened;
        if (UnityEngine.Random.Range(0, 1f) <= resiPercent)
            finalResi++;
        damage -= finalResi;
        if (damage < 0)
        {
            resiThatHappened = finalResi + damage;
            damage = 0;
        }
        else
            resiThatHappened = finalResi;
        if (finalResi > 0 && damageResisted != null)
            damageResisted.Invoke(resiThatHappened,damageType);
        golemConstruct.currentHP -= damage;
        if (damageTaken != null)
            damageTaken.Invoke(damage, damageType);
        if (golemConstruct.currentHP <= 0)
            isDead = true;
    }
    public void Die()
    {
        if (GolemDied != null)
            GolemDied.Invoke();
        Destroy(gameObject);
    }
    public void SetupItemDelegates()
    {
        if (AddToItemRoller != null)
            AddToItemRoller.Invoke();
    }
    public void RemoveAllLocatonListeners(Location loc)
    {
        if (RemoveListeners != null)
            RemoveListeners.Invoke(loc);
    }
    public void SetupLocationDelegates(Location loc)
    {
        if (AddToLocationRoller != null)
            AddToLocationRoller.Invoke(loc);
    }
    public void EncounterWasAborted(Location loc)
    {
        if (EncounterAborted != null)
            EncounterAborted.Invoke(loc);
    }
}
