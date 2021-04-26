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
    /// <param name="strInput"> String match</param>
    /// <param name="strPattern">String delimiter to split</param>
    /// <returns>represending match boolean</returns>
    /// <param name="ignoreCase">Specifies whether to ignore case in comparison</param>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static double MatchNumbersDistance(String strInput, String strPatternValue, int ignoreCase)
    {
        int defaultMatch = 0;
        SqlString nstr1 = null;
        SqlString nstr2 = null;
        float strclean = 0;
        int str1len = 0;
        Boolean ic = false;

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
            nstr1 = UserDefinedFunctions.GetNumbers(strInput);
            nstr2 = UserDefinedFunctions.GetNumbers(strPatternValue);

            if (ignoreCase == 1)
            {
                string nstr1sd = null;
                nstr1sd = Convert.ToString(nstr1);

                string nstr2sd = null;
                nstr2sd = Convert.ToString(nstr2);

                strclean = UserDefinedFunctions.CleanMatchCountString(nstr1sd, nstr2sd, ic);
            } else
            {
                string nstr1s = null;
                nstr1s = Convert.ToString(nstr1);

                string nstr2s = null;
                nstr2s = Convert.ToString(nstr2);

                strclean = UserDefinedFunctions.MatchCountStringNumbers(nstr1s, nstr2s, ic);
            }

            double Str1Clean = 0;
            Str1Clean = strclean * 1.0;

            string sstr1 = Convert.ToString(nstr1);
            string sstr2 = null;

            sstr2 = sstr1.Replace(" ", "");
            str1len = (sstr1.Length) - (sstr2.Length) + 1;

            if (sstr1.Length != 0)
            {
                double str1lend = 0.0;
                str1lend = Convert.ToDouble(str1len);

                double result = 0.0;
                result = Str1Clean / str1lend;

                return result;

            } else
            {
                return 1.0;
            }
        }
        return defaultMatch;
    }
}
