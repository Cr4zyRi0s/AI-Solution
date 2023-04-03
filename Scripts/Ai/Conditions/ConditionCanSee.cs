using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionCanSee : Condition
{
//    private AIController controller;
//    private GameObject obj;
//    private float range;

//    private float cosValue;
//    private RaycastHit rHit;
//    private int layerMask;

//    public ConditionCanSee(AIController controller, GameObject obj,float visibleConeAngle, float range = Mathf.Infinity ,bool flipped = false) : base(flipped)
//    {
//        this.obj = obj;
//        this.controller = controller;
//        this.range = range;

//        layerMask = LayerMask.GetMask("Damage");

//        cosValue = Mathf.Cos(Mathf.Deg2Rad * visibleConeAngle);
//        /*
//        expressions = new Func<bool>[] { () =>
//                                            Calculate() > cosValue
//                                            && Physics.Raycast(controller.transform.position,
//                                            this.obj.transform.position - controller.transform.position,
//                                            out rHit,
//                                            range)
//                                            && rHit.collider.gameObject.Equals(obj) };*/
//        expressions = new Func<bool>[] { () => Expression() };
//    }

//    private bool Expression()
//    {
//        return Calculate() > cosValue;
//               /* && Physics.Raycast(controller.transform.position,
//                this.obj.transform.position - controller.transform.position,
//                out rHit,
//                1000f)
//                && rHit.collider.gameObject.Equals(obj);*/
//    }

//    public void SetObserveObject(GameObject obj)
//    {
//        this.obj = obj;
//    }

//    public override bool Evaluate()
//    {
//        if (obj != null)
//            return base.Evaluate();
//        else
//            return false;
//    }

//    private float Calculate()
//    {
//        float ret = Vector3.Dot(controller.transform.forward.normalized, (obj.transform.position - controller.transform.position).normalized);
////        Debug.Log(ret);
//        return ret; 
//    }
}
