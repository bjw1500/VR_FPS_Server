using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
    // % (Ctrl), # (Shift), & (Alt)

    [MenuItem("Tools/GenerateMap %#g")]
    private static void GenerateMap()
    {
        GenerateByPath("Assets/Resources/Map");
        // GenerateByPath2("../Common/MapData");
    }

    private static void GenerateByPath(string pathPrefix)
    {
        //맵 폴더에 있는 프리팹을 읽는다.
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject go in gameObjects)
        {
            //프리팹 안에 있는 아이템 스포너를 전부 찾아준다.
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("ItemSpawnPoint");
			GameObject[] objectPoints = GameObject.FindGameObjectsWithTag("HitBoxSpawnPoint");

            //찾아 주었다면 그 위치값들을 전부 텍스트 파일에 저장해준다.

            /*
			 * 텍스트 파일 구조 형식
			 * 아이템 스포너 총 갯수
			 * 아이템 스포너의 위치값 ...
			 * 아이템 스포너의 위치값 ...
			 */

            int countSpawnPoint = spawnPoints.Length;
			int countObject = objectPoints.Length;

            using (var writer = File.CreateText($"{pathPrefix}/{go.name}.txt"))
            {
                //제일 첫줄에 스포너의 총 갯수 적어준다.
               writer.WriteLine("ObjectCount:" + countObject);

				foreach (GameObject spawnPoint in spawnPoints)
                {
					Vector3 point = spawnPoint.transform.position;
					writer.Write(point.x);
					writer.Write(",");
					writer.Write(point.y);
					writer.Write(",");
					writer.Write(point.z);
					writer.WriteLine();
                }
				//두 번째 줄에 히트박스의 갯수 적어준다.
				writer.WriteLine("ObjectCount:" + countObject);

				foreach (GameObject spawnHitBox in objectPoints)
				{
					Vector3 point = spawnHitBox.transform.position;
					writer.Write(point.x);
					writer.Write(",");
					writer.Write(point.y);
					writer.Write(",");
					writer.Write(point.z);
					writer.WriteLine();
				}
            }
        }
    }
}