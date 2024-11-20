using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingCredits : MonoBehaviour
{
    string testers;

    void Start()
    {
        var x = CSVReader.GetCSV("CreditsNames");
        for (int nameIndex = 1; nameIndex < x.GetUpperBound(0); nameIndex++)
        {
            int allowedToUseRealName = x[nameIndex][2].Contains("n") ? 1 : 0;
            testers += $"\n{x[nameIndex][allowedToUseRealName]}";
        }
        GetComponent<TMP_Text>().text += testers;
    }
}
