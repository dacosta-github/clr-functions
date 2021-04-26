using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Collections.Generic;
using System.Collections;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Remove letter from clean strings and remove duplicates words
    /// </summary>
    /// <param name="str">string value input</param>
    /// <param name="delimiter">delimiter string</param>
    /// <param name="nChars">number of chars</param>
    /// <returns>string with words but without a specific number of letters</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static string RemoveLetterString(string str, string delimiter, int nChars)
    {
        // string stringValue = CleanUpProductName(str);
        string stringValue = UserDefinedFunctions.RemoveDuplicateString(str);

        List<string> stringsList = new List<string>(stringValue.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries));
        StringBuilder res = new StringBuilder();

        foreach (string item in stringsList)
        {
            res.Append(item + " ");
            int i = 0;
            while (i <= item.Length && (item.Substring(i, Math.Min(i + nChars, item.Length - i))).Length >= nChars)
            {
                res.Append(item.Remove(i, nChars) + " ");
                i++;
            }
        }
        // Result 
        return res.ToString().Trim();
    }
}
