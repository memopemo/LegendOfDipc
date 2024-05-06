
using System;
using System.Collections.Generic;
using UnityEngine;

class CSVReader
{
    public static string[][] GetCSV(string csvResourceFileName)
    {
        List<List<string>> csv = new();

        var dataset = Resources.Load<TextAsset>(csvResourceFileName);
        var lines = dataset.text.Split('\n');
        var lists = new List<string[]>();
        for (int i = 0; i < lines.Length; i++)
        {
            lists.Add(lines[i].Split(','));
        }
        Debug.Log(lists.ToArray());
        return lists.ToArray();
        
    }
}