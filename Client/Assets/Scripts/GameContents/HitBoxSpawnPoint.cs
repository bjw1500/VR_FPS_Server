using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init()
    {
        //게임 시작되면 삭제.
        Destroy(transform.gameObject);
    }
}