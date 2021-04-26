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
    //Savtetochange
    /// <summary>
    /// Match clean strings.
    /// </summary>
    /// <param name="strInput"> String match</param>
    /// <param name="strPattern">String delimiter to split</param>
    /// <returns>represending match boolean</returns>
    /// <param name="ignoreCase">Specifies whether to ignore case in comparison</param>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static int MatchCountStringNumbers(String strInput, String strPatternValue, Boolean ignoreCase)
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

            string strF = ignoreCase ? strInput.ToLower() : strInput;
            string strS = ignoreCase ? strPatternValue.ToLower() : strPatternValue;

            List<string> stringsList = new List<string>(strF.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            int count = 0;
            foreach (string item in stringsList)
            {
                //Ignora palavras isoladas 
                if (Regex.IsMatch(strS, string.Format(@"\b{0}\b", Regex.Escape(item))))
                    count++;
            }

            return count;

        }
        return defaultMatch;
    }
}
