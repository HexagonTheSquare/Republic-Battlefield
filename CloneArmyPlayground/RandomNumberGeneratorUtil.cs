using System;
using System.Security.Cryptography;

public static class RandomNumberGeneratorUtil
{
    public static int Next(int minValue, int maxValue)
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int value = BitConverter.ToInt32(randomNumber, 0);
            return Math.Abs(value % (maxValue - minValue)) + minValue;
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
}
