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
    /// Split strings.
    /// </summary>
    /// <param name="str">string to splitter</param>
    /// <param name="delimiter">String delimiter to split</param>
    /// <returns>split represending the Product Words </returns>
    [SqlFunction(Name = "SplitString",
    FillRowMethodName = "FillRow",
    TableDefinition = "Word NVARCHAR(150)")]
    public static IEnumerable SqlArray(SqlString str, SqlChars delimiter)
    {
        if (str == null)
        {
            str = "";
        }
        
        string stringValue = UserDefinedFunctions.CleanUpProductName(str.Value);
        //string stringValue = str.Value.Replace("  ", " ").Trim(); // dbo.Split

        //return single element array if no delimiter is specified
        if (delimiter.Length == 0)
            return new string[1] { stringValue };

        //split the string and return a string array
        return stringValue.Split(delimiter[0]);
    }

    public static void FillRow(object row, out SqlString str)
    {
        //set the column value
        str = new SqlString((string)row);
    }
}
