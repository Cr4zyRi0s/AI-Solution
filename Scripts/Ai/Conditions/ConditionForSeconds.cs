using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionForSeconds : Condition
{
    float timer;
    float endTime;
    float duration;

    bool firstEvaluation = true;
    public ConditionForSeconds(float duration,bool flipped = false) : base (flipped)
    {
        this.duration = duration;
        SetEndTime();
        expressions = new Func<bool>[] { () => timer <= this.endTime };
        timer = Time.time;
    }

    public override bool Evaluate()
    {
        timer = Time.time;

        if (firstEvaluation)
        {
            SetEndTime();
            firstEvaluation = false;
            Debug.Log("Time = " + timer + " Duration = " + endTime);
        }

        return base.Evaluate();
    }

    private void SetEndTime()
    {
        endTime = Time.time + duration;
    }

    public override void Reset()
    {
        firstEvaluation = true;
    }

    public override string ToString()
    {
        return "Condition: ForSeconds(" + (duration) +")";
    }
}
