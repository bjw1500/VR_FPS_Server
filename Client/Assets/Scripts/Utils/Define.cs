using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum MoveDir
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        SingleGame,
        SingleLobby,
        LoadingScene,
        Map000,
        Map001,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum VRType
    {
        Oculus,
        Vive,
    }
    public enum GunState { Ready, Empty, Reloading }

}
