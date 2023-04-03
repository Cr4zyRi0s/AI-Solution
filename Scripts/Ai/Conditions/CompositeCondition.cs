using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeCondition : Condition
{
    public CompositeCondition(System.Func<bool>[] expressions, bool flipped = false)
    {
        this.expressions = expressions;
        this.flipped = flipped;
    }
    public CompositeCondition(Condition[] conditions)
    {
        this.expressions = new System.Func<bool>[conditions.Length];
        this.flipped = false;
        
        
    }


}
