using System.Collections.Generic;
public interface IPlannerGoal
{
    public abstract Dictionary<string, object> GoalConditions { get; } 
}
