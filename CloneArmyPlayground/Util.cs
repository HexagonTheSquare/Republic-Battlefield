using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Util
{

    public static List<string> ReadWordsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' not found.");
        }

        return File.ReadAllLines(filePath).ToList();
    }

    private static Random rng = new Random();

    public static int Next(int min, int max) => rng.Next(min, max);
    public static float NextFloat(float min, float max) => (float)(rng.NextDouble() * (max - min) + min);

    public static string GetOrdinalSuffix(int num)
    {
        if (num % 100 >= 11 && num % 100 <= 13)
            return "th";
        int comp = num % 10; 
        return comp switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
    }

    public static string GetPluralSuffix(int count) => count == 1 ? "y" : "ies";

    public static string GetRandomCloneRank()
    {
        string[] ranks = { "Private", "Corporal", "Sergeant", "Lieutenant", "Captain" };
        return ranks[Next(0, ranks.Length)];
    }

    public static string GetRandomDroidDesignation()
    {
        string[] designations = { "Commander", "Security", "Standard", "Tactical" };
        return designations[Next(0, designations.Length)];
    }

    public static string GetRandomDroidType()
    {
        string[] types = { "B1 Battle Droid", "B2 Super Battle Droid", "Tactical Droid", "Commando Droid" };
        return types[Next(0, types.Length)];
    }

    public static string GetRandomWord(List<string> words)
    {
        return words[Next(0, words.Count)];
    }

    public static string GetUniqueWord(List<string> words)
    {
        if (words.Count == 0) return "Unnamed";
        int index = Next(0, words.Count);
        string word = words[index];
        words.RemoveAt(index);
        return word;
    }
}
