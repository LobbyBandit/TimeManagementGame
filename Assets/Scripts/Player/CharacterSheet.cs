using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{
    static float skillScaler = 5;

    [Header("                   Attributes")]
    [Space(10)]
    [Range(0, 10)]
    public int Strength;
    [Range(0, 10)]
    public int Dexterity;
    [Range(0, 10)]
    public int Vitality;
    [Range(0, 10)]
    public int Wisdom;
    [Range(0, 10)]
    public int Intelligence;
    [Range(0, 10)]
    public int Arcana;

    public int MaxAllocatedPoints;

    [HideInInspector]
    public Dictionary<string, float> skills = new Dictionary<string, float>();

    
    [Header("                   Skills")]
    [Space(10)]

    [Header("Strength")]
    [Space(5)]
    public int CarryingCapacity;

    [Header("Dexterity")]
    [Space(5)]
    public int MovingSpeed;
    public int InteractionSpeed;
    public int Tinkering;
    public int Lockpiking;

    [Header("Wisdom")]
    [Space(5)]
    public int Survival;
    public int Herbalism;

    [Header("Vitality")]
    [Space(5)]
    public int MaxHealth;
    public int StaminaRecovery;

    [Header("Intelligence")]
    [Space(5)]
    public int Reading;

    [Header("Arcana")]
    [Space(5)]
    public int Comprehention;
    public int ManaPool;

    private void Awake()
    {
       SessionSkillManager.characterSheet = this;  
    }


    private void Start()
    {
       SkillsGameStartSetup();

       AddSkills();

        float gettingskill = skills["CarryingCapacity"];
        Debug.Log(gettingskill);
    }

    bool FirstTimeInitialized;

    void SkillsGameStartSetup()
    {
        if (FirstTimeInitialized)
            return;
        CarryingCapacity = Mathf.FloorToInt(SetSkill(Strength, Intelligence));
        MovingSpeed = Mathf.FloorToInt(SetSkill(Dexterity, Intelligence));
        InteractionSpeed = Mathf.FloorToInt(SetSkill(Dexterity, Intelligence));
        Tinkering = Mathf.FloorToInt(SetSkill(Intelligence, Dexterity));
        MovingSpeed = Mathf.FloorToInt(SetSkill(Dexterity, Strength));
        Lockpiking = Mathf.FloorToInt(SetSkill(Dexterity, Intelligence));
        Survival = Mathf.FloorToInt(SetSkill(Wisdom, Strength));
        Herbalism = Mathf.FloorToInt(SetSkill(Wisdom, Intelligence));
        MaxHealth = Mathf.FloorToInt(SetSkill(Vitality, Wisdom));
        StaminaRecovery = Mathf.FloorToInt(SetSkill(Vitality, Dexterity));
        Reading = Mathf.FloorToInt(SetSkill(Intelligence, Wisdom));
        Comprehention = Mathf.FloorToInt(SetSkill(Arcana, Intelligence));
        ManaPool = Mathf.FloorToInt(SetSkill(Arcana, Vitality));


        FirstTimeInitialized = true;
    }       //Very Messy

    public void AddSkills()
    {
        //Strength 
        skills.Add("CarryingCapacity", SetSkill(Strength, Intelligence) * ((float)CarryingCapacity / skillScaler));

        //Dexterity
        skills.Add("MovingSpeed", SetSkill(Dexterity, Strength) * ((float)MovingSpeed / skillScaler));
        skills.Add("InteractionSpeed", SetSkill(Dexterity, Intelligence) * ((float)InteractionSpeed / skillScaler));
        skills.Add("Tinkering", SetSkill(Intelligence, Dexterity) * ((float)Tinkering / skillScaler));    
        skills.Add("Lockpicking", SetSkill(Dexterity, Intelligence) * ((float)Lockpiking / skillScaler));

    }

    float SetSkill(int AttributeMain,int AttributeSecondary)
    {
        float skill;
        skill = (((float)AttributeMain * 2) + ((float)AttributeSecondary / 2)) / 2 ;

       // skill *= ((float)_skill / 5);
        return skill;
    }


    
    private void Update()
    {
  SessionSkillManager.characterSheet = this;        
    }
    

    /*
    private void UpdateSessionSkillManager()
    {
        SessionSkillManager.characterSheet = this;
    }
    */

    private void OnValidate()
    {
        MaxAllocatedPoints = Strength + Dexterity + Intelligence + Vitality + Wisdom + Arcana;
    }
}
