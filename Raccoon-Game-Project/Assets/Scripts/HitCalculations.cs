using UnityEngine;
public static class HitCalculation
{
    public static int HurtPlayerAmount(int damage)
    {
        int armor = SaveManager.GetSave().CurrentArmor;
        bool[] hasDefenseBuffActive = {DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Defense0), DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Defense1), DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Defense2)};
        
        if (armor >= 0) //-1 and below means no armor.
        {
            damage /= armor + 2; // X/2, X/3, X/4, X/5.
        }
        
        for(int i = 0; i < 3; i++)
        {
            if(hasDefenseBuffActive[i])
            {
                damage -= i+1; //for example, if defense2 is active, subtract 2.
            }
        }
        damage = Mathf.Clamp(damage, 1, 50); //atleast take 1 damage.
        return damage;

        
    }
    public static int PlayerHurtEnemyAmount(int damage)
    {
        bool[] hasAttackBuffActive = {DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Attack0), DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Attack1)};

        //add damage based on attack buffs
        for(int i = 0; i < 2; i++)
        {
            if(hasAttackBuffActive[i])
            {
                damage += i+1; //for example, if attack2 is active, add 2.
            }
        }
        damage = Mathf.Clamp(damage, 1, 50); //atleast take 1 damage.
        return damage;
    }
}