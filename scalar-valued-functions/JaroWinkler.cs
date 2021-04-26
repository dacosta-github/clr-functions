using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Calculate the Jaro-Winkler Similarity of two strings
    /// </summary>
    /// <param name="firstWord">First string to calculate the distance</param>
    /// <param name="secondWord">Second string to calculate the distance</param>
    /// <returns>int represending the Jaro-Winkler Similarity</returns>
    [Microsoft.SqlServer.Server.SqlFunction] //System.Data.SqlTypes.SqlDouble
    public static Double JaroWinklerDistance(String firstWord, String secondWord)
    {
        double defaultMismatchScore = 0.0;
        double defaultMatchScore = 1.0;

        if ((firstWord != null) && (secondWord != null))
        {
            //Use CleanUpProductName and ToUpperInvariant to clean String
            String firstString = UserDefinedFunctions.CleanUpProductName(firstWord.ToUpperInvariant());
            String secondString = UserDefinedFunctions.CleanUpProductName(secondWord.ToUpperInvariant());

            if ((firstString != null) && (secondString != null))
            {
                if (firstString == secondString)
                {
                    return (Double)defaultMatchScore;
                }
                else
                {
                    //Bonus weighting for string starting with the same characters (e.g.: prefix scaling factor)
                    double PREFIX_SCALING_FACTOR = 0.1;

                    double cJaroDistance = JaroDistance(firstString, secondString);
                    int prefixLength = CommonPrefix(firstString, secondString);

                    //Find the Jaro-Winkler Distance: Jd + (l * p * ( 1 - Jd));
                    double cJaroWinklerDistance = cJaroDistance + ((prefixLength * PREFIX_SCALING_FACTOR) * (1 - cJaroDistance));

                    // Return value JaroWinklerDistance
                    return cJaroWinklerDistance;
                }
            }
            return defaultMismatchScore;
        }
        return defaultMismatchScore;
    }

    /// <summary>
    /// Calculate the Sort-Jaro-Winkler Similarity of two strings
    /// </summary>
    /// <param name="firstWord">First string to calculate the distance</param>
    /// <param name="secondWord">Second string to calculate the distance</param>
    /// <returns>int represending the Jaro-Winkler Similarity</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static Double SortJaroWinklerDistance(String firstWord, String secondWord, int numRemoveLetters)
    {
        double defaultMismatchScore = 0.0;
        if ((firstWord != null) && (secondWord != null))
        {
            //Use SortString Function to sort String
            String sortedFirstString = UserDefinedFunctions.SortString(firstWord, " ", numRemoveLetters);
            String sortedSecondString = UserDefinedFunctions.SortString(secondWord, " ", numRemoveLetters);

            double cSortJaroWinklerDistance = UserDefinedFunctions.JaroWinklerDistance(sortedFirstString, sortedSecondString);

            // Return value Sorted JaroWinklerDistance
            return cSortJaroWinklerDistance;
        }
        return defaultMismatchScore;
    }

    /// <summary>
    /// Calculate the Permuted-Jaro-Winkler Similarity of two strings
    /// </summary>
    /// <param name="firstWord">First permuted string to calculate the distance</param>
    /// <param name="secondWord">Second permuted string to calculate the distance</param>
    /// <param name="numberWord">limited of permuted strings</param>
    /// <returns>int represending the Jaro-Winkler Similarity</returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static Double PermutedJaroWinklerDistance(String firstWord, String secondWord)
    {
        
        double similarity = 0;
        double defaultMismatchScore = 0.0;
        if ((firstWord != null) && (secondWord != null))
        {
            //Use CleanUpProductName and ToUpperInvariant to clean String
            String firstString = UserDefinedFunctions.CleanUpProductName(firstWord.ToUpperInvariant());
            String secondString = UserDefinedFunctions.CleanUpProductName(secondWord.ToUpperInvariant());
            int numberWord = UserDefinedFunctions.CountStringSize(firstString, secondString);

            List<string> ls1 = firstString.Split(' ').ToList();
            List<string> ls2 = secondString.Split(' ').ToList();

            // Permuta apenas strings com tamanho inferior a numberWords (e.g, 4)
            if (ls2.Count <= numberWord && ls1.Count <= numberWord && numberWord <= 4)
            {
                IList<IList<string>> perm1 = UserDefinedFunctions.Permutations(ls1);
                IList<IList<string>> perm2 = UserDefinedFunctions.Permutations(ls2);

                foreach (var item1 in perm1)
                {
                    string str1 = string.Join(" ", item1.ToArray());
                    foreach (var item2 in perm2)
                    {
                        string str2 = string.Join(" ", item2.ToArray());
                        similarity = Math.Max(similarity, UserDefinedFunctions.JaroWinklerDistance(str1, str2));
                    }
                }
                // Return value permuted JaroWinklerDistance
                return similarity;
            }
            else
            {
                similarity = UserDefinedFunctions.JaroWinklerDistance(firstString, secondString);
                return similarity;
            }
        }
        return defaultMismatchScore;
    }

    /// <summary>
    ///Determine Common Characters
    ///@param firstWord  - First String
    ///@param secondWord - Second String
    ///@param matchWindow - Second String
    ///@return 
    private static StringBuilder GetCommonCharacters(String firstWord, String secondWord, int matchWindow)
    {
        if ((firstWord != null) && (secondWord != null))
        {

            int firstLen = firstWord.Trim().Length;
            int secondLen = secondWord.Trim().Length;
            StringBuilder returnCommons = new StringBuilder();
            StringBuilder copy = new StringBuilder(secondWord);

            for (int i = 0; i < firstLen; i++)
            {

                char ch = firstWord[i];
                bool foundIT = false;

                //for (int j = Math.Max(0, i - matchWindow); !foundIT && j < Math.Min(i + matchWindow, secondLen); j++)
                for (int j = (i - matchWindow > 1 ? i - matchWindow : 0); !foundIT && j < (i + matchWindow <= secondLen ? Math.Min(i + matchWindow + 1, secondLen) : secondLen); j++)
                {
                    if (copy[j] == ch)
                    {
                        foundIT = true;
                        returnCommons.Append(ch);
                        copy[j] = '#';
                    }
                }
            }
            return returnCommons;
        }
        return null;
    }

    /// <summary>
    /// Determine the maximum window size to use when looking for matches.
    /// The window is basically a little less than the half the longest
    /// string's length.
    /// Equation: [ Max(A, B) / 2 ] -1
    /// @param firstWord  - First String
    /// @param secondWord - Second String
    /// @return Max window size
    private static int MatchingWindow(String firstWord, String secondWord)
    {
        int firstWordLen = firstWord.Trim().Length;
        int secondWordLen = secondWord.Trim().Length;

        return (Math.Max(firstWordLen, secondWordLen) / 2) - 1;
    }

    /// <summary>
    /// Find the all of the matching and transposed characters in two strings
    /// @param firstWord  - First String
    /// @param secondWord - Second String
    /// @return number of transposed characters
    private static int GetTranspositions(String firstWord, String secondWord)
    {
        int firstWordLen = firstWord.Length > secondWord.Length ? firstWord.Length : secondWord.Length;
        int transposations = 0;
        for (int i = 0; i < firstWordLen; i++)
        {
            if ((i >= firstWord.Length - 1 ? ' ' : firstWord[i]) !=
                (i >= secondWord.Length - 1 ? ' ' : secondWord[i]))
            {
                transposations++;
            }
        }
        return transposations / 2;
    }

    /// <summary>
    /// Determine the Jaro Distance.  Winkler stole some of Jaro's
    /// thunder by adding some more math onto his algorithm and getting
    /// equal parts credit!  However, we still need Jaro's Distance
    /// to get the Jaro-Winkler Distance.
    /// Equation: 1/3 * ((|A| / m) + (|B| / m) + ((m - t) / m))
    /// Where: |A| = length of first string
    ///        |B| = length of second string
    ///         m  = number of matches
    ///         t  = number of transposes
    /// @param numMatches Number of matches
    /// @param numTransposes Number of transposes
    /// @param firstWordLen Length of String one
    /// @param secondWordLen Length of String two
    /// @return Jaro Distance
    private static double JaroDistance(String firstWord, String secondWord)
    {
        int matchWindow = MatchingWindow(firstWord, secondWord);

        //get common characters
        StringBuilder firstWordCommonCharacters = GetCommonCharacters(firstWord, secondWord, matchWindow);
        StringBuilder secondWordCommonCharacters = GetCommonCharacters(secondWord, firstWord, matchWindow);

        // get length strings
        int firstWordLen = firstWord.Trim().Length;
        int secondWordLen = secondWord.Trim().Length;

        int firstWordCommonCharactersLen = firstWordCommonCharacters.Length;
        int secondWordCommonCharactersLen = secondWordCommonCharacters.Length;

        if (firstWordCommonCharactersLen == 0 || firstWordCommonCharacters == null)
        {
            return 0;
        }

        if (firstWordCommonCharactersLen != secondWordCommonCharactersLen || secondWordCommonCharacters == null)
        {
            return 0;
        }

        //GetTranspositions
        int transposation = GetTranspositions(firstWordCommonCharacters.ToString(), secondWordCommonCharacters.ToString());

        //d_jaro = 1/3 * ( m/|s1| + m/|s2| + (m-t)/m )
        //double jaroDistance = 1/3 * (
        //                     (firstWordCommonCharactersLen / firstLen) 
        //                        + 
        //                     (firstWordCommonCharactersLen / secondLen) 
        //                        + 
        //                     ((firstWordCommonCharactersLen - transposation) / firstWordCommonCharactersLen));

        //d_jaro = 1/3 * ( m/|s1| + m/|s2| + (m-t)/m )
        double jaroDistance = (firstWordCommonCharactersLen / (3.0 * firstWordLen))
                                +
                              (firstWordCommonCharactersLen / (3.0 * secondWordLen))
                                +
                              ((firstWordCommonCharactersLen - transposation) / (3.0 * firstWordCommonCharactersLen));

        return jaroDistance;

    }

    /// <summary>
    /// Find the Winkler Common Prefix of two strings.  In Layman's terms,
    /// find the number of character that match at the beginning of the
    /// two strings, with a maximum of 4.
    /// @param firstWord First string
    /// @param secondWord Second string
    /// @return Integer between 0 and 4 representing the number of
    /// matching characters at the beginning of both strings.
    private static int CommonPrefix(String firstWord, String secondWord)
    {

        int commonPrefix = 0;
        //Find the shortest string (we don't want an index out of bounds
        //exception).
        int boundary = (firstWord.Length <= secondWord.Length) ? firstWord.Length : secondWord.Length;
        //iterate until the boundary is hit (shortest string length)
        for (int i = 0; i < boundary; i++)
        {
            //If the character at the current position matches
            if (firstWord[i] == secondWord[i])
            {
                //increment the common prefix
                commonPrefix++;
            }
            else
            {
                //otherwise, continue no further, we are done.
                break;
            }
            //If we hit our max number of matches, bust out of the loop.
            if (commonPrefix == 4)
            {
                break;
            }
        }
        //return the number of matches at the beginning of both strings
        return commonPrefix;
    }
}