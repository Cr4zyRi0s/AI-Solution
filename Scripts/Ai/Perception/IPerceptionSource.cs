using System.Collections;
using UnityEngine;


public interface IPerceptionSource
{
    public PerceptionType Type { get; }
    public bool CanBePerceived(IPerceptionReceiver fromReceiver);
    public PerceptionData GetPerception(IPerceptionReceiver fromReceiver);
}
