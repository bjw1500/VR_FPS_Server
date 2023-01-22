using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PickUp : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose = null;
    //public SteamVR_Action_Boolean grabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    [SerializeField] FixedJoint joint = null;
    [SerializeField] public Custom_Interactable currectInteractable;
    [SerializeField] List<Custom_Interactable> currectInteractables = new List<Custom_Interactable>();
    [SerializeField] int power = 3;

    private void Awake()
    {
        pose = this.GetComponent<SteamVR_Behaviour_Pose>();
        joint = this. GetComponent<FixedJoint>();
    }

    private void Update()
    {
        //if (grabGripAction.GetStateDown(pose.inputSource))
        //{
        //    Debug.Log(pose.inputSource + " down");
        //    Pickup();
        //}

        //if (grabGripAction.GetStateUp(pose.inputSource))
        //{
        //    Drop();
        //}

        if (GameMng.I.input.getStategrabGrip)
        {
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Item"))
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

    public void Pickup(Custom_Interactable target)
    {
        currectInteractable = target;

        if (!currectInteractable)
            return;

        if (currectInteractable.activeHand)
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

        Debug.Log("수류탄을 던집니다.");


        Collider col = currectInteractable.GetComponent<Collider>();
        col.enabled = true;
        Rigidbody targetBody = currectInteractable.GetComponent<Rigidbody>();
        targetBody.useGravity = true;
        targetBody.velocity = power * pose.GetVelocity();
        targetBody.angularVelocity = power * pose.GetAngularVelocity();

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