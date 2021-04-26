using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Linq;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Calculates the Levenshtein Distance between two strings.
    /// It is minimum of single character insert/delete/update operations needed to transfrom
    /// first string into the second string
    /// </summary>
    /// <param name="firstString">First string to calculate the distance</param>
    /// <param name="secondString">Second string to calculate the distance</param>
    /// <param name="ignoreCase">Specifies whether to ignore case in comparison</param>
    /// <returns>int represending the Levenshtein Distance</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static int LevenshteinDistance(string firstString, string secondString, Boolean ignoreCase)
    {
        int defaultMatch = 0;
        if ((firstString != null) || (secondString != null))
        {
            string firstStringValue = UserDefinedFunctions.CleanUpProductName(firstString);
            string secondStringValue = UserDefinedFunctions.CleanUpProductName(secondString);
            
            string strF = ignoreCase ? firstStringValue.ToLower() : firstStringValue;
            string strS = ignoreCase ? secondStringValue.ToLower() : secondStringValue;
           
            int lenF = strF.Length;
            int lenS = strS.Length;
            int[,] d = new int[lenF + 1, lenS + 1];

            for (int i = 0; i <= lenF; i++)
                d[i, 0] = i;
            for (int j = 0; j <= lenS; j++)
                d[0, j] = j;

            for (int j = 1; j <= lenS; j++)
            {
                for (int i = 1; i <= lenF; i++)
                {
                    if (strF[i - 1] == strS[j - 1])
                        d[i, j] = d[i - 1, j - 1];
                    else
                        d[i, j] = Math.Min(Math.Min(
                          d[i - 1, j] + 1,        // a deletion
                          d[i, j - 1] + 1),       // an Insertion
                          d[i - 1, j - 1] + 1);   // a substitution ( match or mismatch)
                }
            }
            return d[lenF, lenS];
        }
        return defaultMatch;
    }

    /// <summary>
    /// Calculates the Sort Levenshtein Distance between two strings.
    /// It is minimum of single character insert/delete/update operations needed to transfrom
    /// first string into the second string
    /// </summary>
    /// <param name="firstString">First sort string to calculate the distance</param>
    /// <param name="secondString">Second sort string to calculate the distance</param>
    /// <param name="ignoreCase">Specifies whether to ignore case in comparison</param>
    /// <returns>int represending the Sort Levenshtein Distance</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static int SortLevenshteinDistance(string firstString, string secondString, Boolean ignoreCase, int numRemoveLetters)
    {
        int defaultMatch = 0;
        if ((firstString != null) || (secondString != null))
        {
            //Use SortString Function to sort String and remove letters
            string sortFirstString = UserDefinedFunctions.SortString(firstString, " ", numRemoveLetters);
            string sortSecondString = UserDefinedFunctions.SortString(secondString, " ", numRemoveLetters);

            int cSortLevenshteinDistance = UserDefinedFunctions.LevenshteinDistance(sortFirstString, sortSecondString, ignoreCase);

            return cSortLevenshteinDistance;

        }
        return defaultMatch;
    }
};
