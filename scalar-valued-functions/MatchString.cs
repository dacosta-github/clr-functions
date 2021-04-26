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
    /// Match clean strings.
    /// </summary>
    /// <param name="strInput"> String match</param>
    /// <param name="strPattern">String delimiter to split</param>
    /// <returns>represending match boolean</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean MatchString(SqlString strInput, SqlString strPatternValue)
    {
        if (strInput == null)
        {
            strInput = "";
        }

        if (strPatternValue == null)
        {
            strPatternValue = "";
        }
        
        if (strInput.IsNull || strPatternValue.IsNull)
        {
            return SqlBoolean.False;
        }

        //SqlString strPattern = UserDefinedFunctions.CleanUpProductName(strPatternValue.Value);
        SqlString strPattern = strPatternValue.Value;

        if (strPattern.IsNull || strInput.IsNull)
        {
            return SqlBoolean.False;
        }
        return (SqlBoolean)Regex.IsMatch(strInput.Value, strPattern.Value, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
}
