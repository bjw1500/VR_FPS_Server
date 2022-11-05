using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    //아이템 스폰 지점.

    /*맵 제작툴을 돌리면 아이템 스폰 지점의 위치가 텍스트 파일에 저장이 되어야 한다.

    서버는 텍스트 파일을 통해 스폰 위치를 알고, 
    
    그 주변에 아이디 발급이 된 아이템을 생성하여 플레이어들한테 뿌린다.
    */

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
        Destroy(this.gameObject);
    }
}
