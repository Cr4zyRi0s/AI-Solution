using System.Collections;
using UnityEngine;

public class PerceptionSourceBase : MonoBehaviour, IPerceptionSource
{
    [SerializeField]
    private PerceptionType _type;
    public PerceptionType Type => _type;

    public bool CanBePerceived(IPerceptionReceiver fromReceiver)
    {
        return true;
    }

    public virtual PerceptionData GetPerception(IPerceptionReceiver fromReceiver)
    {
        return new PerceptionData(transform.position, _type);
    }
}
