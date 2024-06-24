using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeAction : MonoBehaviour
{

    public GlobalSkillList skillList;

    [Range(1,250)]
    public int requiredSkillLevel;

    public float minutesToComplete;
    float _minutesToComplete;

    float playerSkillLevel;

    bool canDoAction;

    public void SetupSkillCheck()
    {
       CheckSkillLevel();
        if (!canDoAction)
            Debug.Log("Skill Level Too Low");

       CalculateMinutesToComplete();

        Debug.Log("Minutes To Complete  :  " + _minutesToComplete / 2.5);
    }


    void CheckSkillLevel()
    {
        int _skill = (int)skillList;
        playerSkillLevel = SessionSkillManager.GetPlayerSkill(SkillList._skills[_skill]);

        if (playerSkillLevel >= requiredSkillLevel) 
            canDoAction = true;
    }

    void CalculateMinutesToComplete()
    {
        _minutesToComplete = minutesToComplete;
        _minutesToComplete *= 2.5f;
        AddBonuses();

    }

    void AddBonuses()
    {
        _minutesToComplete = Mathf.Lerp(_minutesToComplete, _minutesToComplete / 4, playerSkillLevel / 250);
    }

}
