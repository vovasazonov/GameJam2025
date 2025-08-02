using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public struct SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2(Vector2 vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}

[System.Serializable]
public class SerializableCycle
{
    public List<SerializableVector2> points = new List<SerializableVector2>();

    // Конструктор для преобразования Vector2 в SerializableVector2
    public SerializableCycle(List<Vector2> vectorList)
    {
        points = vectorList.Select(v => new SerializableVector2(v)).ToList();
    }

    // Метод для обратного преобразования
    public List<Vector2> ToVector2List()
    {
        return points.Select(sv => sv.ToVector2()).ToList();
    }
}

[System.Serializable]
public class LevelData
{
    public int id;
    public List<SerializableVector2> points = new List<SerializableVector2>();
    public List<SerializableCycle> cycles = new List<SerializableCycle>();
}

[System.Serializable]
public class LevelDatabaseWrapper
{
    public List<LevelData> levels = new List<LevelData>();
}



