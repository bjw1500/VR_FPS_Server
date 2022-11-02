using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Custom_LaserPointer : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose;
    //public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.__actions_default_in_InteractUI;
    public bool active = true;
    public Color color;
    public float thickness = 0.002f;
    public Color clickColor = Color.green;
    public GameObject holder;
    public GameObject pointer;
    bool isActive = false;
    public event PointerEventHandler PointerIn;
    public event PointerEventHandler PointerOut;
    public event PointerEventHandler PointerClick;
    public event PointerEventHandler PointerGrip;
    [SerializeField] private float customdist = 100.0f;
    Transform previousContact = null;
    private Vector3 dotVec = new Vector3(9999.0f, 9999.0f, 9999.0f);        // <! 레이저 끝 점 미국보내기

    private void Start()
    {
        if (pose == null)
            pose = this.GetComponent<SteamVR_Behaviour_Pose>();
        if (pose == null)
            Debug.LogError("No SteamVR_Behaviour_Pose component found on this object", this);

        if (GameMng.I.input.interactWithUI == null)
            Debug.LogError("No ui interaction action has been set on this component.", this);

        holder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        holder.transform.parent = this.transform;
        holder.transform.localScale = new Vector3(thickness, thickness, 100f);
        holder.transform.localPosition = new Vector3(0f, 0f, 50f);
        holder.transform.localRotation = Quaternion.identity;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointer.transform.parent = this.transform;
        pointer.transform.localPosition = dotVec;
        pointer.transform.localScale = new Vector3(0.05f, 0.05f, 0.0001f);
        pointer.transform.localRotation = Quaternion.identity;

        SphereCollider spherecollider = pointer.GetComponent<SphereCollider>();
        BoxCollider collider = holder.GetComponent<BoxCollider>();

        if (collider)
        {
            Object.Destroy(collider);
        }
        if (spherecollider)
        {
            Object.Destroy(spherecollider);
        }

        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.SetColor("_Color", color);
        holder.GetComponent<MeshRenderer>().material = newMaterial;
        newMaterial.SetColor("_Color", Color.blue);
        pointer.GetComponent<MeshRenderer>().material = newMaterial;
    }

    public virtual void OnPointerIn(PointerEventArgs e)
    {
        if (PointerIn != null)
            PointerIn(this, e);
    }

    public virtual void OnPointerClick(PointerEventArgs e)
    {
        if (PointerClick != null)
            PointerClick(this, e);
    }

    public virtual void OnPointerOut(PointerEventArgs e)
    {
        if (PointerOut != null)
            PointerOut(this, e);
    }
    
    public virtual void OnPointerGrip(PointerEventArgs e)
    {
        if(PointerGrip != null)
            PointerGrip(this, e);
    }

    private void Update()
    {
        if (!isActive)
        {
            isActive = true;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        float dist = customdist;

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (previousContact && previousContact != hit.transform)
        {
            PointerEventArgs args = new PointerEventArgs();
            args.fromInputSource = pose.inputSource;
            args.distance = 0f;
            args.flags = 0;
            args.target = previousContact;
            OnPointerOut(args);
            previousContact = null;
        }
        if (bHit && previousContact != hit.transform)
        {
            PointerEventArgs argsIn = new PointerEventArgs();
            argsIn.fromInputSource = pose.inputSource;
            argsIn.distance = hit.distance;
            argsIn.flags = 0;
            argsIn.target = hit.transform;
            OnPointerIn(argsIn);
            previousContact = hit.transform;
        }
        if (!bHit)
        {
            previousContact = null;
        }
        if (bHit && hit.distance < 100f)
        {
            dist = hit.distance;
        }
        if (bHit && GameMng.I.input.interactWithUI.GetStateDown(pose.inputSource))       // <! GetStateUp에서 다운으로 바꿈
        {
            PointerEventArgs argsClick = new PointerEventArgs();
            argsClick.fromInputSource = pose.inputSource;
            argsClick.distance = hit.distance;
            argsClick.flags = 0;
            argsClick.target = hit.transform;
            OnPointerClick(argsClick);
        }
        if (GameMng.I.input.interactWithUI != null && GameMng.I.input.interactWithUI.GetState(pose.inputSource))
        {
            holder.transform.localScale = new Vector3(thickness, thickness, dist);
            holder.GetComponent<MeshRenderer>().material.color = clickColor;
        }
        else
        {
            holder.transform.localScale = new Vector3(thickness, thickness, dist);
            holder.GetComponent<MeshRenderer>().material.color = color;
        }

        pointer.transform.position = bHit ? hit.point : dotVec;     // <! 포인터가 충돌 되어있을떄 충돌위치(ui 위치)에 놓아주고 아니면 미국 보내기
        pointer.transform.rotation = bHit ? hit.transform.rotation : Quaternion.identity;

        holder.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
    }
}
public struct PointerEventArgs
{
    public SteamVR_Input_Sources fromInputSource;
    public uint flags;
    public float distance;
    public Transform target;
}

public delegate void PointerEventHandler(object sender, PointerEventArgs e);
