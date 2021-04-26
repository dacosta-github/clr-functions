using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Get Numbers From clean strings.
    /// </summary>
    /// <param name="str"> strings </param>
    /// <param name="numRemoveLetters"> number of letters to remove </param>
    /// <returns>permutations represending the Product words combination</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString GetString(string str, int numRemoveLetters)
    {
        string stringValue = UserDefinedFunctions.CleanUpProductName(str);
        //string stringValue = str.Replace("  ", " ").Trim(); // dbo.Split

        string stringSortValue = UserDefinedFunctions.CleanUpProductName(UserDefinedFunctions.SortString(stringValue, " ", numRemoveLetters));

        if (stringSortValue == null)
        {
            stringSortValue = "0";
        }

        StringBuilder res = new StringBuilder();

        // Words
        var matches = Regex.Matches(stringSortValue, @"[\D']*");

        foreach (Match match in matches)
        {
            res.Append(match.Value.TrimStart().TrimEnd() + " ").ToString();
        }

        // Result 
        if (res == null || res.ToString().Trim() == "")
        {
            return "";
        }
        else
        {
            return res.ToString().Trim();
        }
    }
}
