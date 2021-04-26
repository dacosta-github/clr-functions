using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    [return: Microsoft.SqlServer.Server.SqlFacet(MaxSize = 512)]
    public static string CleanUpProductName([Microsoft.SqlServer.Server.SqlFacet(MaxSize=512)] string instr)
    {
        string r = "";
        int i;
        string next = "";
        string previous = "A";
        string cur = "";

        if (instr != null)
        {
            if (instr.Length > 0)
            {
                instr = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(instr.Trim()));


                for (i = 0; i < instr.Length; i++)
                {
                    if (r.Length > 1)
                        previous = r.Substring(r.Length - 1, 1);

                    if (i < (instr.Length - 1))
                        next = instr.Substring(i + 1, 1);
                    else
                        next = "";

                    cur = instr.Substring(i, 1);

                    if (char.IsLetterOrDigit(instr, i))
                    {
                        r += cur;
                    }
                    else
                    {
                        //Remove any char from first and last position that is not a letter or number
                        if ((r.Length != 0) && (char.IsLetterOrDigit(previous, 0)))
                        {
                            if ((cur == "-") || (cur == "+") || (cur == "/") || (cur == " ") || (cur == "."))
                            {
                                if ((next != " ") || ((cur == "-") || (cur == "+") || (cur == "/")))
                                {
                                    if (!(((previous == "-") || (previous == "+") || (previous == "/") || (previous == " ")) ||
                                          ((next == "-") || (next == "+") || (next == "/") || (next == " "))))
                                    {
                                        r += cur;
                                    }
                                    else
                                    {
                                        if (!(((previous == "-") || (previous == "+") || (previous == "/") || (next == "-") || (next == "+") || (next == "/"))
                                            && (cur == " ")))
                                        {
                                            r += cur;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (r.Length > 0)
                    if (!char.IsLetterOrDigit(r, r.Length - 1))
                        r = r.Substring(0, r.Length - 1);
            }
        }
        return (r.Trim());
    }
}
