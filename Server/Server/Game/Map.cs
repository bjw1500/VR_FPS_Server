using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public struct Vector3
{
	public float x;
	public float y;
	public float z;

	public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }


	public static Vector3 operator +(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator -(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}


}


public class Map
{
	public int ItemSpawnPointCount { get; set; }
	public int ObjectSpawnPointCount { get; set; }
    public Vector3[] ItemSpawnPoints { get; set; }
	public Vector3[] ObjectSpawnPoints { get; set; }
	

	public void LoadMap(int mapId, string pathPrefix = "../../../../../Client/Assets/Resources/Map")
	{
		string mapName = "Map" + mapId.ToString("000");

		// Collision 관련 파일
		string text = File.ReadAllText($"{pathPrefix}/{mapName}.txt");
		StringReader reader = new StringReader(text);

		string ItemSpawnline = reader.ReadLine();
		string[] itemCounts = ItemSpawnline.Split(':');
		ItemSpawnPointCount = int.Parse(itemCounts[1]);
		//ItemSpawnPointCount = int.Parse(reader.ReadLine());
		

		ItemSpawnPoints = new Vector3[ItemSpawnPointCount];

		for (int i = 0; i < ItemSpawnPointCount; i++)
		{
			string line = reader.ReadLine();
			string[] parts = line.Split(',');
			ItemSpawnPoints[i] = new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));

		}


		string hitboxSpawnline = reader.ReadLine();
		string[] hitboxCounts = hitboxSpawnline.Split(':');
		ObjectSpawnPointCount = int.Parse(hitboxCounts[1]);

		ObjectSpawnPoints = new Vector3[ObjectSpawnPointCount];

		for (int i = 0; i < ObjectSpawnPointCount; i++)
		{
			string line = reader.ReadLine();
			string[] parts = line.Split(',');
			ObjectSpawnPoints[i] = new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));

		}


	}

}

