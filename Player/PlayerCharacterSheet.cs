using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSheet : MonoBehaviour, IDamageable, IEquipable, ICharacterSheet
{
    private event EventHandler OnDeath;

    //private Dictionary<EquipSlotType, Item> equipment;
    public Team team = Team.Living;
    public string Name = "Brenda";
    public float MaxHealth = 10;
    public float Health;
    public float MoveSpeed = 7.5f;
    public float WorkSpeed = 1f;

    [SerializeField] private List<BaseStat> baseStats = null;
    private Dictionary<BaseStatType, int> baseModifiers;
    private Dictionary<BaseStatType, int> additionalBaseModifiers;
    
    private void Start()
    {
        //equipment = new Dictionary<EquipSlotType, Item>();
        baseModifiers = new Dictionary<BaseStatType, int>();
        additionalBaseModifiers = new Dictionary<BaseStatType, int>();

        foreach (BaseStat stat in baseStats)
        {
            stat.SetUp();
            baseModifiers.Add(stat.statType, stat.GetModifier());
            additionalBaseModifiers.Add(stat.statType, 0);
            stat.OnStatChanged += CharacterSheet_OnStatChanged;
        }

        MaxHealth = MaxHealth + (baseModifiers.ContainsKey(BaseStatType.Constitution)? baseModifiers[BaseStatType.Constitution] : 0);
        Health = MaxHealth;

        //Inventory inv = this.GetComponent<Inventory>();
        //if(inv != null)
        //{
        //    inv.AddItem(WorldController.current.itemManager.BuildItem(Item.ItemType.HealthLrg, 2));
        //    inv.AddItem(WorldController.current.itemManager.BuildItem(Item.ItemType.Knife, 1));
        //    inv.AddItem(WorldController.current.itemManager.BuildItem(Item.ItemType.Money, 15));

        //    IInventoryUI inventoryUI = WorldController.current.inventoryUI.GetComponent<IInventoryUI>();
        //    inventoryUI.SetInventory(inv);
        //}
    }

    private void Update()
    {
        #region Temporarily Removed
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    switch (UIReferences.i.InventoryUI.activeSelf)
        //    {
        //        case true:
        //            UIReferences.i.InventoryUI.SetActive(false);
        //            break;
        //        case false:
        //            UIReferences.i.InventoryUI.SetActive(true);
        //            break;
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    switch (UIReferences.i.CharacterUI.activeSelf)
        //    {
        //        case true:
        //            UIReferences.i.CharacterUI.SetActive(false);
        //            break;
        //        case false:
        //            UIReferences.i.CharacterUI.SetActive(true);
        //            break;
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    switch (UIReferences.i.CraftingUI.activeSelf)
        //    {
        //        case true:
        //            UIReferences.i.CraftingUI.SetActive(false);
        //            break;
        //        case false:
        //            UIReferences.i.CraftingUI.SetActive(true);
        //            break;
        //    }
        //}
        #endregion


    }

    public void OnHit(float damage)
    {
        Debug.Log("Hit " + Name + " for " + damage + " damage");

        Health -= damage;

        if(Health <= 0)
        {
            //We are dead
            OnDeath?.Invoke(this.gameObject, EventArgs.Empty);
            Destroy(this.gameObject);
        }
    }

    public string GetName()
    {
        return Name;
    }

    public Team GetTeam()
    {
        return team;
    }

    public void Heal(float amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void ChangeXP(BaseStatType statType, float xp)
    {
        foreach (BaseStat stat in baseStats)
        {
            if(stat.statType == statType)
            {
                stat.ChangeXP(xp);
            }
        }
    }

    public List<BaseStat> GetBaseStats()
    {
        return baseStats;
    }

    public float? GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.MoveSpeed:
                return MoveSpeed;
        }

        return null;
    }

    public void ChangeAdditionalBaseModifier(BaseStatType statType, int modifier)
    {
        additionalBaseModifiers[statType] = additionalBaseModifiers[statType] + modifier;
        updateBaseModifier(statType);
    }

    private void updateBaseModifier(BaseStatType statType)
    {
        foreach (BaseStat stat in baseStats)
        {
            if(stat.statType == statType)
            {
                baseModifiers[stat.statType] = stat.GetModifier() + additionalBaseModifiers[stat.statType];
                Debug.Log(stat.statType + " Modifier = " + baseModifiers[stat.statType]);
            }
        }
    }

    //public Item EquipItem(Item item)
    //{
    //    EquipSlotType slot = item.SlotType;

    //    if (equipment.ContainsKey(slot))
    //    {
    //        Item unequippedItem = equipment[slot];
    //        equipment.Remove(slot);
    //        equipment.Add(slot, item);

    //        return unequippedItem;
    //    }
        
    //    equipment.Add(slot, item);
    //    return null;
    //}

    //public Item UnEquipItem(EquipSlotType type)
    //{
    //    if (equipment.ContainsKey(type) == true)
    //    {
    //        Item item = equipment[type];
    //        equipment.Remove(type);
    //        return item;
    //    }

    //    return null;
    //}

    private void CharacterSheet_OnStatChanged(object sender, StatChangedEventArgs args)
    {
        updateBaseModifier(args.stat.statType);
    }

    public void SetDeathNotification(EventHandler onDeath = null)
    {
        OnDeath += onDeath;
    }
}

public enum BaseStatType
{
    Strength,
    Dexterity,
    Constitution,
    Intelligence,
    Wisdom,
    Charisma
}

[Serializable]
public class BaseStat
{
    private static int XP_FOR_FIRST_LEVEL = 15;
    private static float XP_LEVEL_MULTIPLIER = 1.25f;

    public event EventHandler<StatChangedEventArgs> OnStatChanged;
    public BaseStatType statType;
    [Range(0, 20)]
    public int value;
    public float currentXP;
    private float XPToLvl;

    public void SetUp()
    {
        SetXpToLevel();
        currentXP = 0.1f;
    }

    public void ChangeXP(float amount)
    {
        currentXP += amount;
        if (currentXP >= XPToLvl || currentXP <= 0)
        {
            Level();
        }
    }

    public float GetXpToLvl()
    {
        return XPToLvl;
    }

    private void Level()
    {
        if(currentXP >= XPToLvl)
        {
            value++;
            value = Mathf.Clamp(value, 0, 20);
            currentXP -= XPToLvl;
            SetXpToLevel();
        }
        else if(currentXP <= 0)
        {
            value--;
            value = Mathf.Clamp(value, 0, 20);
            SetXpToLevel();
            currentXP += XPToLvl;
        }

        OnStatChanged?.Invoke( this, new StatChangedEventArgs{ stat = this } );
    }

    private void SetXpToLevel()
    {
        XPToLvl = XP_FOR_FIRST_LEVEL;

        for (int i = 0; i < value; i++)
        {
            XPToLvl = (XPToLvl * XP_LEVEL_MULTIPLIER);
        }
    }

    public int GetModifier()
    {
        if(value == 20)
        {
            return 5;
        }
        else if(value >= 18)
        {
            return 4;
        }
        else if(value >= 16)
        {
            return 3;
        }
        else if (value >= 14)
        {
            return 2;
        }
        else if (value >= 12)
        {
            return 1;
        }
        else if (value >= 10)
        {
            return 0;
        }
        else if (value >= 8)
        {
            return -1;
        }
        else if (value >= 6)
        {
            return -2;
        }
        else if (value >= 4)
        {
            return -3;
        }
        else if (value >= 2)
        {
            return -4;
        }
        else
        {
            return -5;
        }
    }
}

public enum StatType
{
    MoveSpeed
}

public class StatChangedEventArgs : EventArgs
{
    public BaseStat stat;
}

public enum Team
{
    Living,
    Dead
}
