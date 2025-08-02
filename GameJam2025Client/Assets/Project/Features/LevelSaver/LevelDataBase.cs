using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.Core.Scripts;
using UnityEngine;

public class LevelDatabase: SingletonBehaviour<LevelDatabase>
{
    private string filePath;
    private LevelDatabaseWrapper databaseWrapper;
    private int nextId = 1;
    private string fileName = "levels.json";

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        databaseWrapper = new LevelDatabaseWrapper();
        LoadDatabase();
    }
    

    private void LoadDatabase()
    {
        
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            databaseWrapper = JsonUtility.FromJson<LevelDatabaseWrapper>(json) ?? new LevelDatabaseWrapper();

            if (databaseWrapper.levels != null && databaseWrapper.levels.Count > 0)
            {
                nextId = databaseWrapper.levels.Max(level => level.id) + 1;
            }
        }
        else
        {
            databaseWrapper = new LevelDatabaseWrapper();
        }
    }

    private void SaveDatabase()
    {
        string json = JsonUtility.ToJson(databaseWrapper, true);
        File.WriteAllText(filePath, json);
    }

    public int AddLevel(List<Vector2> points, List<List<Vector2>> cycles)
    {
        LevelData newLevel = new LevelData
        {
            id = nextId++,
            points = points.Select(p => new SerializableVector2(p)).ToList(),
            cycles = cycles.Select(c => new SerializableCycle(c)).ToList()
        };

        databaseWrapper = new LevelDatabaseWrapper();
        databaseWrapper.levels.Add(newLevel);
        SaveDatabase();
        return newLevel.id;
    }

    public (List<Vector2> points, List<List<Vector2>> cycles) GetLevel(int id)
    {
        LevelData levelData = databaseWrapper.levels.FirstOrDefault(level => level.id == id);

        if (levelData != null)
        {
            List<Vector2> points = levelData.points
                .Select(p => p.ToVector2())
                .ToList();

            List<List<Vector2>> cycles = levelData.cycles
                .Select(c => c.ToVector2List())
                .ToList();

            return (points, cycles);
        }

        return (null, null);
    }

    public bool RemoveLevel(int id)
    {
        LevelData levelToRemove = databaseWrapper.levels.FirstOrDefault(level => level.id == id);
        if (levelToRemove != null)
        {
            databaseWrapper.levels.Remove(levelToRemove);
            SaveDatabase();
            return true;
        }

        return false;
    }
}