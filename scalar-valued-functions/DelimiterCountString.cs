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
    /// Count Delimiter strings.
    /// </summary>
    /// <param name="str1"> String 1</param>
    /// <param name="delimiter"> String 2</param>
    /// <returns>Max Size both two strings</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static int DelimiterCountString(string str1, string delimiter)
    {
        int str1CleanLength = 0;
        int str1Length = 0;
        int delimiterLength = 0;
        int SUFIX_FACTOR = 1;

        if (str1 == null)
        {
            str1 = "";
        }

        str1 = str1.Replace(" ", " ");
        str1CleanLength = str1.Length;
       
        //return single element ( 1 ) if no delimiter is specified
        if (delimiter.Length == 0)
            return SUFIX_FACTOR;

        str1Length = str1.Replace(delimiter, "").Length;
        
        delimiterLength = delimiter.Length;

        return ((str1CleanLength - str1Length) / delimiterLength) + SUFIX_FACTOR;

    }
}
