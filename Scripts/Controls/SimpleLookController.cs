using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLookController : MonoBehaviour
{
    //public AIAgent agent;
    //public NavigationZone navWorld;

    //private MouseLook mouseLook;
    //private Transform cameraTransform;

    //private bool targetSet;

    //private Vector3 targetPosition;
    //private Vector3 targetNodePosition;
    ////private NavigationWorld.Edge[] pendingPath;
    //private NavigationZone.Sample[] pendingPath;


    //private void Awake()
    //{
    //    mouseLook = new MouseLook();        
    //}

    //private void Start()
    //{
    //    cameraTransform = gameObject.transform.Find("Main Camera");
    //    mouseLook.Init(gameObject.transform, cameraTransform);
             
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //mouseLook.LookRotation(gameObject.transform, cameraTransform);

    //    //if (Input.GetKeyDown(KeyCode.Mouse0))
    //    //{
    //    //    RaycastHit rhit;
    //    //    Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
    //    //    if (Physics.Raycast(ray, out rhit, 1000f)) {
    //    //        targetPosition = rhit.point;  
    //    //        agent.AssignNavigationTarget(targetPosition);
    //    //        if (agent.isPathPending)
    //    //        {
    //    //            targetNodePosition = navWorld.GetNearestNodeToPoint(rhit.point).position;
    //    //            targetSet = true;
    //    //        }
    //    //    }
    //    //}

    //    //if(Input.GetKeyDown(KeyCode.Mouse1))
    //    //{
    //    //    targetSet = false;  
    //    //}
    //}

    //private void OnDrawGizmos()
    //{
    //    if (Application.isPlaying && targetSet)
    //    {
    //        Gizmos.color = Color.white;
    //        Gizmos.DrawLine(transform.position, targetPosition);
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawSphere(targetPosition + Vector3.up, .5f);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawSphere(targetNodePosition + Vector3.up, .5f);
    //    }
    //}
}
