using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region Contents
    
    static ObjectManager _obj = new ObjectManager();
    static NetworkManager _network = new NetworkManager();

    public static ObjectManager Object { get { return _obj; } }
    public static NetworkManager Network { get { return _network; } }
    
    #endregion

    #region Core
    static DataManager _data = new DataManager();
    static PoolManager _pool = new PoolManager();
    static ResourceManager _resource = new ResourceManager();
    static SceneManagerEx _scene = new SceneManagerEx();
    static SoundManager _sound = new SoundManager();
    static UIManager _ui = new UIManager();

    public static DataManager Data { get { return _data; } }
    public static PoolManager Pool { get { return _pool; } }
    public static ResourceManager Resource { get { return _resource; } }
    public static SceneManagerEx Scene { get { return _scene; } }
    public static SoundManager Sound { get { return _sound; } }
    public static UIManager UI { get { return _ui; } }

    #endregion

    void Update()
    {
        _network.Update();
    }

    public static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();

            DontDestroyOnLoad(go);

            _network.Init();
            _data.Init();
            _pool.Init();
            _sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
