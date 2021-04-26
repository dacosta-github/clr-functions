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
    /// Split and Sorter clean strings.
    /// </summary>
    /// <param name="str">string to splitter</param>
    /// <param name="delimiter">string delimiter to split</param>
    /// <param name="numRemoveLetters">number of words to remove</param>
    /// <returns>sorting represending the order by Product Name </returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static string SortString(string str, string delimiter, int numRemoveLetters)
    {
        string stringValue = UserDefinedFunctions.CleanUpProductName(str);
        //string stringValue = str;

        // split and sort Strings
        List<string> stringsList = new List<string>(stringValue.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries));
        stringsList.Sort();
        StringBuilder res = new StringBuilder();

        // remove string < Len 1
        foreach (string item in stringsList)
        {
            if (item.Length > numRemoveLetters)
                res.Append(item + " ");
        }

        // Result 
        return res.ToString().TrimEnd();
    }
}
