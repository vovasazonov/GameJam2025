using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.Core.Scripts;
using UnityEngine;

public class LevelDatabase : SingletonBehaviour<LevelDatabase>
{
    private string filePath;
    private LevelDatabaseWrapper databaseWrapper;
    private int nextId = 1;
    private string fileName = "levels.json";
    [SerializeField] private TextAsset levels;

    public void Start()
    {
        databaseWrapper = new LevelDatabaseWrapper();
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        LoadDatabase();
    }

    private void LoadDatabase()
    {
        // Всегда инициализируем новую обертку
        databaseWrapper = new LevelDatabaseWrapper();

        // Загружаем данные только если файл существует

        string json = levels.text;
        var loadedWrapper = JsonUtility.FromJson<LevelDatabaseWrapper>(json);

        if (loadedWrapper != null && loadedWrapper.levels != null)
        {
            databaseWrapper.levels = loadedWrapper.levels;

            // Находим максимальный ID
            if (databaseWrapper.levels.Count > 0)
            {
                nextId = databaseWrapper.levels.Max(level => level.id) + 1;
            }
        }


        // Гарантируем, что список уровней всегда инициализирован
        if (databaseWrapper.levels == null)
        {
            databaseWrapper.levels = new List<LevelData>();
        }
    }

    private void SaveDatabase()
    {
        // Всегда сохраняем все уровни
        string json = JsonUtility.ToJson(databaseWrapper, true);
        File.WriteAllText(filePath, json);
        Debug.Log(filePath);
    }

    public int AddLevel(List<Vector2> points, List<List<Vector2>> cycles)
    {
        // Проверка и инициализация входных параметров
        if (points == null) points = new List<Vector2>();
        if (cycles == null) cycles = new List<List<Vector2>>();

        // Создаем новый уровень
        LevelData newLevel = new LevelData
        {
            id = nextId++,
            points = points.Select(p => new SerializableVector2(p)).ToList(),
            cycles = cycles.Select(c => new SerializableCycle(c)).ToList()
        };

        // Добавляем новый уровень к существующим
        databaseWrapper.levels.Add(newLevel);

        // Сохраняем всю базу целиком
        SaveDatabase();

        return newLevel.id;
    }

    public (List<Vector2> points, List<List<Vector2>> cycles) GetLevel(int id)
    {
        if (databaseWrapper?.levels == null) return (null, null);

        LevelData levelData = databaseWrapper.levels.FirstOrDefault(level => level.id == id);
        if (levelData == null) return (null, null);

        // Преобразование points
        List<Vector2> points = levelData.points?.Select(p => p.ToVector2()).ToList()
                               ?? new List<Vector2>();

        // Преобразование cycles
        List<List<Vector2>> cycles = new List<List<Vector2>>();
        if (levelData.cycles != null)
        {
            foreach (var cycle in levelData.cycles)
            {
                if (cycle != null)
                {
                    cycles.Add(cycle.ToVector2List());
                }
            }
        }

        return (points, cycles);
    }

    public bool RemoveLevel(int id)
    {
        if (databaseWrapper?.levels == null) return false;

        LevelData levelToRemove = databaseWrapper.levels.FirstOrDefault(level => level.id == id);
        if (levelToRemove != null)
        {
            databaseWrapper.levels.Remove(levelToRemove);
            SaveDatabase();
            return true;
        }

        return false;
    }

    // Новый метод для получения всех ID уровней
    public List<int> GetAllLevelIds()
    {
        if (databaseWrapper?.levels == null) return new List<int>();
        return databaseWrapper.levels.Select(level => level.id).ToList();
    }
}