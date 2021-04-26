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
    /// Final match clean strings.
    /// </summary>
    /// <param name="strInput1"> String match 1</param>
    /// <param name="strInput2"> String match 2</param>
    /// <returns>represending match boolean</returns>
    /// <param name="ignoreCase">Specifies whether to ignore case in comparison</param>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static double FinalMatchNumbersDistance(String strInput1, String strInput2, Boolean ignoreCase)
    {
        int defaultMatch = 0;

        if (strInput1 == null)
        {
            strInput1 = "";
        }

        if (strInput2 == null)
        {
            strInput2 = "";
        }

        if ((strInput1 != null) || (strInput2 != null))
        {
            int ic = Convert.ToInt32(ignoreCase);
            double valueStrInput1 = UserDefinedFunctions.MatchNumbersDistance(strInput1, strInput2, ic);
            double valueStrInput2 = UserDefinedFunctions.MatchNumbersDistance(strInput2, strInput1, ic);

            double finalValue1 = (valueStrInput1 + valueStrInput2) / 2;

            // if numerator is zero, return zero else return "Final Match Numbers Distance"
            if (valueStrInput1 == 0.0 || valueStrInput2 == 0.0)
            {
                return 0;
            }       
            return finalValue1;
        }
        return defaultMatch;
    }
}
