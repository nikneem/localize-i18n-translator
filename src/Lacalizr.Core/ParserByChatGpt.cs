using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class I18nParser
{
    private Dictionary<string, object> _translations;

    public I18nParser(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file {filePath} was not found.");
        }

        string jsonContent = File.ReadAllText(filePath);
        _translations = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent)
                        ?? new Dictionary<string, object>();
    }

    public Dictionary<string, object> GetTranslations()
    {
        return _translations;
    }

    public string GetTranslation(string key)
    {
        var keys = key.Split('.');
        object current = _translations;

        foreach (var k in keys)
        {
            if (current is Dictionary<string, object> dict && dict.TryGetValue(k, out var next))
            {
                current = next;
            }
            else
            {
                return key; // Return key itself if translation not found
            }
        }

        return current is string str ? str : key;
    }

    public void ExportToJson(string outputPath)
    {
        string jsonContent = JsonSerializer.Serialize(_translations, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, jsonContent);
    }
}