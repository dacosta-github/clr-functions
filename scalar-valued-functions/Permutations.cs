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
    /// Permnutations strings.
    /// </summary>
    /// <param name="list"> Strings</param>
    /// <returns>permutations represending the Product words combination</returns>
    private static IList<IList<T>> Permutations<T>(IList<T> list)
    {
        List<IList<T>> perms = new List<IList<T>>();
        if (list.Count == 0)
            return perms; // Empty list.
        int factorial = 1;
        for (int i = 2; i <= list.Count; i++)
            factorial *= i;
        for (int v = 0; v < factorial; v++)
        {
            List<T> s = new List<T>(list);
            int k = v;
            for (int j = 2; j <= list.Count; j++)
            {
                int other = (k % j);
                T temp = s[j - 1];
                s[j - 1] = s[other];
                s[other] = temp;
                k = k / j;
            }
            perms.Add(s);
        }
        return perms;
    }
}
