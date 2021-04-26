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
    /// <param name="ignoreCase">Specifies whether to ignore case in comparison</param>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static int CleanMatchCountString(String strInput, String strPatternValue, Boolean ignoreCase)
    {
        int defaultMatch = 0;

        if (strInput == null)
        {
            strInput = "";
        }

        if (strPatternValue == null)
        {
            strPatternValue = "";
        }

        if ((strInput != null) || (strPatternValue != null))
        {
            string firstStringValue = UserDefinedFunctions.CleanUpProductName(strInput);
            string secondStringValue = UserDefinedFunctions.CleanUpProductName(strPatternValue);

            string strF = ignoreCase ? firstStringValue.ToLower() : firstStringValue;
            string strS = ignoreCase ? secondStringValue.ToLower() : secondStringValue;

            List<string> stringsList = new List<string>(strF.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            int count = 0;
            foreach (string item in stringsList)
            {
                //Ignores single words 
                if (Regex.IsMatch(strS, string.Format(@"\b{0}\b", Regex.Escape(item))))
                    count++;
            }

            return count;

        }
        return defaultMatch;
    }
}
