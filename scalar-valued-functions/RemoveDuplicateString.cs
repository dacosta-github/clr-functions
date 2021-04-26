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
    /// Get clean strings and remove duplicate words.
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>string without duplicates words</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static string RemoveDuplicateString(string str)
    {
        string stringValue = UserDefinedFunctions.CleanUpProductName(str);

        // Keep track of words found in this Dictionary.
        var d = new Dictionary<string, bool>();

        // Build up string into this StringBuilder.
        StringBuilder b = new StringBuilder();

        // Split the input and handle spaces and punctuation.
        string[] a = stringValue.Split(new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);

        // Loop over each word
        foreach (string current in a)
        {
            // Lowercase each word
            string lower = current.ToLower();

            // If we haven't already encountered the word,
            // append it to the result.
            if (!d.ContainsKey(lower))
            {
                b.Append(current).Append(' ');
                d.Add(lower, true);
            }
        }
        // Return the duplicate words removed
        return b.ToString().Trim();
    }
}
