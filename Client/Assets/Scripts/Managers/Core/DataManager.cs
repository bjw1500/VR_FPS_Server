using Google.Protobuf.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, StatInfo> StatDict { get; private set; } = new Dictionary<string, StatInfo>();
    public Dictionary<string, ItemData> ItemDict { get; private set; } = new Dictionary<string, ItemData>();

    public void Init()
    {
        StatDict = LoadJson<StatData, string, StatInfo>("StatData").MakeDict();
        ItemDict = LoadJson<ItemLoader, string, ItemData>("ItemData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        //JsonUtility 왜 안됨???????????
        //TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        //      var test = 4;
        //      return JsonUtility.FromJson<Loader>(textAsset.text);

        var jsonSerializerSettings = new JsonSerializerSettings();
        jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        //string text = File.ReadAllText($"{ConfigManager.Config.dataPath}/{path}.json");
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(textAsset.text, jsonSerializerSettings);
    }
}
