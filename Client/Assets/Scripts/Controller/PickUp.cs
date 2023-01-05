using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PickUp : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose = null;
    public SteamVR_Action_Boolean grabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    [SerializeField] FixedJoint joint = null;
    [SerializeField] Custom_Interactable currectInteractable;
    [SerializeField] List<Custom_Interactable> currectInteractables = new List<Custom_Interactable>();

    private void Awake()
    {
        pose = this.GetComponent<SteamVR_Behaviour_Pose>();
        joint = this. GetComponent<FixedJoint>();
    }

    private void Update()
    {
        if (grabGripAction.GetStateDown(pose.inputSource))
        {
            Debug.Log(pose.inputSource + " down");
            Pickup();
        }

        if (grabGripAction.GetStateUp(pose.inputSource))
        {
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ganade"))
            return;

        currectInteractables.Add(other.gameObject.GetComponent<Custom_Interactable>());
    }
    
    private void OnTriggerExit(Collider other)
    {
        currectInteractables.Remove(other.gameObject.GetComponent<Custom_Interactable>());
    }

    public void Pickup()
    {
        currectInteractable = GetNearstInteractable();

        if(!currectInteractable)
            return;

        if(currectInteractable.activeHand)
            currectInteractable.activeHand.Drop();

            currectInteractable.transform.position = transform.position;

            Rigidbody targetBody = currectInteractable.GetComponent<Rigidbody>();
            joint.connectedBody = targetBody;

            currectInteractable.activeHand = this;
    }

    public void Drop()
    {
        if(!currectInteractable)
            return;

        Rigidbody targetBody = currectInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = pose.GetVelocity();
        targetBody.angularVelocity = pose.GetAngularVelocity();

        joint.connectedBody = null;

        currectInteractable.activeHand = null;
        currectInteractable = null;
    }

    private Custom_Interactable GetNearstInteractable()
    {
        Custom_Interactable nearest = null;
        float mindistance = float.MaxValue;
        float distance = 0.0f;

        foreach(Custom_Interactable interactable in currectInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;
            if(distance < mindistance)
            {
                mindistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }
}