using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Condition
{
    public System.Func<bool>[] expressions { protected set; get; }
    public bool lastEvaluation = true;

    protected bool flipped;

    List<Condition> conditions;

    public Condition(bool flipped = false)
    {
        this.flipped = flipped;
    }
    public Condition(Func<bool> expression,bool flipped = false)
    {
        this.flipped = flipped;
        expressions =new Func<bool>[] { expression };
    }
    public Condition(Func<bool>[] expressions, bool flipped = false)
    {
        this.flipped = flipped;
        this.expressions = expressions;
    }
    public Condition(params Condition[] conditions)
    {
        flipped = false;
        this.conditions = new List<Condition>();

        foreach (Condition c in conditions)
        {
            if(c != null)
                this.conditions.Add(c);
        }
    }

    public virtual bool Evaluate()
    {
        bool currEvaluation;
        if (expressions != null)
        {
            currEvaluation = true;
            foreach (Func<bool> func in expressions)
            {
                if (!func.Invoke())
                {
                    currEvaluation = false;
                    break;
                }
                //currEvaluation = currEvaluation && func.Invoke();
            }
            lastEvaluation = currEvaluation ^ flipped;
            return lastEvaluation;
        }
        else if (conditions != null)
        {
            currEvaluation = true;
            foreach (Condition cond in conditions)
            {
                if (!cond.Evaluate())
                {
                    currEvaluation = false;
                    break;
                }
                //currEvaluation = currEvaluation && cond.Evaluate();
            }
            lastEvaluation = currEvaluation ^ flipped;
            return lastEvaluation;
        }
        else
        {
            Debug.LogWarning("Invalid expression.");
            return false;
        }
    }

    public virtual void Reset()
    { }

    public override string ToString()
    {
        if (conditions != null)
        {
            string ret = "";
            foreach (Condition c in conditions)
                if(c != null)
                    ret += c.ToString();
            return ret;
        }
        else
            return GetType().Name;
    }

    public static Condition operator &(Condition a, Condition b) => new Condition(a, b);
}
