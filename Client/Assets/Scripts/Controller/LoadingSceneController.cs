using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] RectTransform loadingBar;

    bool Test = false;

    public static bool flag = false;

    private void Start()
    {
        if (flag == false)
            StartCoroutine(LoadingScene());
        else
        {
            StartCoroutine(MapLoading());
            flag = false;
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void LoadMap(int mapId)
    {
        flag = true;
        string mapName = "Map" + mapId.ToString("000");
        nextScene = mapName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadingScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            Debug.Log(op.progress);
            if (op.progress < 0.9f)
            {
                loadingBar.Rotate(new Vector3(0, 0, 360.0f * Time.deltaTime));
            }
            else
            {
                loadingBar.Rotate(new Vector3(0, 0, 360.0f * Time.deltaTime));
                yield return new WaitForSeconds(2.0f);
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }

    IEnumerator MapLoading()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return null;

            if (async.progress < 0.9f)
            {
                loadingBar.Rotate(new Vector3(0, 0, 360.0f * Time.deltaTime));
            }
            else
            {
                loadingBar.Rotate(new Vector3(0, 0, 360.0f * Time.deltaTime));
                yield return new WaitForSeconds(2.0f);
                async.allowSceneActivation = true;
                Debug.Log($"{nextScene}이 생성되었습니다.");

                //싱글 게임이라면 패킷 보내지 않고 스킵.
                if (GameMng.I.SingleGame == true)
                {
                    yield break;
                }

                //맵 로딩이 끝났다는 패킷 보내기
                C_MapLoadingFinish finish = new C_MapLoadingFinish();
                Managers.Network.Send(finish);
                yield break;
            }
        }
    }
}
