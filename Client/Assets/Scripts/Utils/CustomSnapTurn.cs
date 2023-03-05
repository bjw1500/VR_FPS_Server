using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CustomSnapTurn : MonoBehaviour
{
    public float snapAngle = 90.0f;

    public bool showTurnAnimation = true;

    public AudioSource snapTurnSource;
    public AudioClip rotateSound;

    public GameObject rotateRightFX;
    public GameObject rotateLeftFX;

    public bool fadeScreen = true;
    public float fadeTime = 0.1f;
    public Color screenFadeColor = Color.black;

    public float distanceFromFace = 1.3f;
    public Vector3 additionalOffset = new Vector3(0, -0.3f, 0);

    public static float teleportLastActiveTime;

    private bool canRotate = true;

    public bool getCanRotate { get { return canRotate;} }

    public float canTurnEverySeconds = 0.4f;

    public Camera cam;
    public Transform player;

    private void Start()
    {
        AllOff();
        if(GameMng.I.snapturn == null)
        {
            GameMng.I.snapturn = this;
        }
    }

    private void AllOff()
    {
        if (rotateLeftFX != null)
            rotateLeftFX.SetActive(false);

        if (rotateRightFX != null)
            rotateRightFX.SetActive(false);
    }

    private Coroutine rotateCoroutine;
    public void RotatePlayer(float angle)
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            AllOff();
        }

        rotateCoroutine = StartCoroutine(DoRotatePlayer(angle));
    }

    //-----------------------------------------------------
    private IEnumerator DoRotatePlayer(float angle)
    {
        canRotate = false;

        snapTurnSource.panStereo = angle / 90;
        snapTurnSource.PlayOneShot(rotateSound);

        if (fadeScreen)
        {
            SteamVR_Fade.Start(Color.clear, 0);

            Color tColor = screenFadeColor;
            tColor = tColor.linear * 0.6f;
            SteamVR_Fade.Start(tColor, fadeTime);
        }

        yield return new WaitForSeconds(fadeTime);

        Vector3 feetPositionGuess = player.position + Vector3.ProjectOnPlane(cam.transform.position - player.position, player.up);
        Vector3 playerFeetOffset = player.position - feetPositionGuess; // - player.
        player.position -= playerFeetOffset;
        player.transform.Rotate(Vector3.up, angle);
        playerFeetOffset = Quaternion.Euler(0.0f, angle, 0.0f) * playerFeetOffset;
        player.position += playerFeetOffset;

        GameObject fx = angle > 0 ? rotateRightFX : rotateLeftFX;

        if (showTurnAnimation)
            ShowRotateFX(fx);

        if (fadeScreen)
        {
            SteamVR_Fade.Start(Color.clear, fadeTime);
        }

        float startTime = Time.time;
        float endTime = startTime + canTurnEverySeconds;

        while (Time.time <= endTime)
        {
            yield return null;
            UpdateOrientation(fx);
        };

        fx.SetActive(false);
        canRotate = true;
    }

    void ShowRotateFX(GameObject fx)
    {
        if (fx == null)
            return;

        fx.SetActive(false);

        UpdateOrientation(fx);

        fx.SetActive(true);

        UpdateOrientation(fx);
    }

    private void UpdateOrientation(GameObject fx)
    {
        //position fx in front of face
        this.transform.position = cam.transform.position + (cam.transform.forward * distanceFromFace);
        this.transform.rotation = Quaternion.LookRotation(cam.transform.position - this.transform.position, Vector3.up);
        this.transform.Translate(additionalOffset, Space.Self);
        this.transform.rotation = Quaternion.LookRotation(cam.transform.position - this.transform.position, Vector3.up);
    }
}
