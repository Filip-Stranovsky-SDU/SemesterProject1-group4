using System;
using System.IO;

namespace WorldOfZuul;

class ASCIImage
{
    public static string LoadImage(string filePath)
    {
        return File.ReadAllText(filePath);
    }

}