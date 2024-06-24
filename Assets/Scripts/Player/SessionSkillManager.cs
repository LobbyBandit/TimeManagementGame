using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SessionSkillManager 
{
    public static CharacterSheet characterSheet;
   
    public static float GetPlayerSkill(string skill)
    {
        float skillIndex;
        skillIndex = characterSheet.skills[skill] ;
        return skillIndex;
    }

    //public static int AddPlayerToSkill()
}
