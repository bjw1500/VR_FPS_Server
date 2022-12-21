using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        Debug.Log($"{type.ToString()}을 불러옵니다.");
        nextScene = GetSceneName(type);
        SceneManager.LoadScene("LoadingScene");
        CoroutineHandler.Start_Coroutine(LoadingScene());
        //SceneManager.LoadScene(GetSceneName(type));
    }

    public static string nextScene;    // <! 다음 씬

    [SerializeField] GameObject loadingBar;


    public void LoadMap(int mapId)
    {
        Managers.Clear();
        //로딩 화면으로 전환.
        SceneManager.LoadScene("LoadingScene");
        CoroutineHandler.Start_Coroutine(MapLoading(mapId));
    }

    IEnumerator TestMapLoading(int mapId)
    {

        AsyncOperation async = SceneManager.LoadSceneAsync(GetSceneName(Define.Scene.Game));
        yield return async;

        
        string mapName = "Map" + mapId.ToString("000");
        GameObject go = Managers.Resource.Instantiate($"Map/{mapName}");
        if (go == null)
            Debug.Log("잘못된 MapId를 받아 맵을 생성하지 못했습니다.");
        else
            Debug.Log($"{mapName}이 생성되었습니다.");
        //맵 로딩이 끝났다는 패킷 보내기
        C_MapLoadingFinish finish = new C_MapLoadingFinish();
        Managers.Network.Send(finish);
    }

    IEnumerator MapLoading(int mapId)
    {

        AsyncOperation async = SceneManager.LoadSceneAsync(GetSceneName(Define.Scene.Game));
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return null;

            if (async.progress < 0.9f)
            {
                loadingBar.transform.Rotate(new Vector3(0, 0, 50.0f * Time.deltaTime));
            }
            else
            {
                async.allowSceneActivation = true;
                string mapName = "Map" + mapId.ToString("000");
                GameObject go = Managers.Resource.Instantiate($"Map/{mapName}");
                if (go == null)
                    Debug.Log("잘못된 MapId를 받아 맵을 생성하지 못했습니다.");
                else
                    Debug.Log($"{mapName}이 생성되었습니다.");
                //맵 로딩이 끝났다는 패킷 보내기
                C_MapLoadingFinish finish = new C_MapLoadingFinish();
                Managers.Network.Send(finish);
                yield break;
            }
        }
    }

    /**
     * @brief Loading 이미지 돌리기
     */
    IEnumerator LoadingScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                loadingBar.transform.Rotate(new Vector3(0, 0, 50.0f * Time.deltaTime));
            }
            else
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
