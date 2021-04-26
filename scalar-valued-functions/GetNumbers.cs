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
    /// Get Numbers From clean strings.
    /// </summary>
    /// <param name="str"> strings </param>
    /// <returns>permutations represending the Product words combination</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString GetNumbers(string str)
    {
        string stringValue = UserDefinedFunctions.CleanUpProductName(str);
        if (stringValue == null)
        {
            stringValue = "";
        }
        string newStringValue = "";

        char[] KeepArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' '};


        foreach (char thischar in stringValue)
        {
            foreach (char keepchar in KeepArray)
            {
                if (keepchar == thischar)
                {
                    newStringValue += thischar;
                }
            }
        }

        return (SqlString)(UserDefinedFunctions.CleanUpProductName(newStringValue));
    }

}
