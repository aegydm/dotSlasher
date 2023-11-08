using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//https://github.com/seintcat
// JsonUtility but array can saved.
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    public static string ToJson<T>(T value) => ToJson(new T[] { value });
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T value, bool prettyPrint) => ToJson(new T[] { value }, prettyPrint);
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    public static void SaveJsonFile<T>(string path, T value) => SaveJsonFile(path, new T[] { value });
    public static void SaveJsonFile<T>(string path, T[] array)
    {
        StreamWriter saveFile;
        if (File.Exists(path))
            File.Delete(path);

        saveFile = new StreamWriter(File.Create(path));
        saveFile.Write(ToJson(array));
        saveFile.Flush();
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}