using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Util
{
    public static int Next(int minValue, int maxValue)
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            int range = maxValue - minValue;
            int mask = range;
            mask |= mask >> 1;
            mask |= mask >> 2;
            mask |= mask >> 4;
            mask |= mask >> 8;
            mask |= mask >> 16;

            while (true)
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                int value = BitConverter.ToInt32(randomNumber, 0) & mask;
                if (value < range)
                    return value + minValue;
            }
        }
    }


    public static float NextFloat(float minValue, float maxValue)
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            uint randomValue = BitConverter.ToUInt32(randomNumber, 0);
            float normalizedValue = (float)randomValue / UInt32.MaxValue; // Normalize the value to the range [0, 1]
            return (maxValue - minValue) * normalizedValue + minValue;
        }
    }

    public static string GetOrdinalSuffix(int number)
    {
        int lastDigit = number % 10;
        int lastTwoDigits = number % 100;

        if (lastDigit == 1 && lastTwoDigits != 11)
        {
            return "st";
        }
        else if (lastDigit == 2 && lastTwoDigits != 12)
        {
            return "nd";
        }
        else if (lastDigit == 3 && lastTwoDigits != 13)
        {
            return "rd";
        }
        else
        {
            return "th";
        }
    }

    public static string GetPluralSuffix(int number)
    {
        if (number > 1 || number == 0) return "ies";
        else return "y";
    }
    public static List<string> ReadWordsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' not found.");
        }

        return File.ReadAllLines(filePath).ToList();
    }

    public static string GetRandomWord(List<string> words)
    {
        if (words == null || words.Count == 0)
        {
            throw new ArgumentException("Words list is empty or null.");
        }
        int index = Next(0, words.Count);
        return words[index];
    }
}
