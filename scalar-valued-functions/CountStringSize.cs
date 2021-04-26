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
    /// Match strings.
    /// </summary>
    /// <param name="str1"> String 1</param>
    /// <param name="str2"> String 2</param>
    /// <returns>Max Size both two strings</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    static int CountStringSize(string str1, string str2)
    {
        int t1 = 0;
        int t2 = 0;
        t1 = Regex.Matches(str1, @"[A-Za-z0-9]+").Count;
        t2 = Regex.Matches(str2, @"[A-Za-z0-9]+").Count;

        return Math.Max(t1, t2);

    }
}
