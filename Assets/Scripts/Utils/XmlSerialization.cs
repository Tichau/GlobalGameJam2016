using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// Set of tools to de/serialize data from/to xml files
/// </summary>
public class XmlSerialization
{
    public static List<T> LoadListFromFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }
        var root = new XmlRootAttribute { ElementName = "Datatable" };
        var serializer = new XmlSerializer(typeof(List<T>), root);
        var stream = new FileStream(path, FileMode.Open);
        List<T> data = serializer.Deserialize(stream) as List<T>;
        stream.Close();
        return data;
    }

    public static void SaveListToFile<T>(string path, List<T> data) where T : class
    {
        var root = new XmlRootAttribute { ElementName = "Datatable" };
        var serializer = new XmlSerializer(typeof(List<T>), root);
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }

    public static T LoadFromTextAsset<T>(TextAsset asset) where T : class
    {
        if (asset == null)
        {
            return null;
        }

        var serializer = new XmlSerializer(typeof(T));
        T data = serializer.Deserialize(new StringReader(asset.text)) as T;
        return data;
    }

    public static T LoadFromFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(path, FileMode.Open);
        T data = serializer.Deserialize(stream) as T;
        stream.Close();
        return data;
    }

    public static void SaveToFile<T>(string path, T data)
    {
        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }
}

