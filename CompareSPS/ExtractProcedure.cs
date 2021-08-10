using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompareSPS
{
    public class ExtractProcedure
    {
        public static string ExtractPureProcedure(string textFile)
        {
            string OnlyPureProcedure = null;
            if (File.Exists(textFile))
            {
                // Read entire text file content in one string    
                string Procedure = File.ReadAllText(textFile);
                Procedure = Procedure.ToUpper();
                Procedure = Procedure.Trim();
                string firstLetter = ExtractProcedure.FindFirstWord(Procedure);
                int EndIndex = FindIndexOfWord(Procedure, firstLetter);
                //Console.WriteLine("Create Procedure Index " + EndIndex);
                //Console.WriteLine("First World is" + firstLetter);
                OnlyPureProcedure = ExtractOnlyProcedureTextFromRawProcedure(Procedure, EndIndex);

            }
            return OnlyPureProcedure;
        }


        public static string FindFirstWord(string text)
        {
            string FirstWorld = null;
            if (text.Contains("CREATE PROCEDURE"))
            {
                FirstWorld = "CREATE PROCEDURE";
            }
            else if (text.Contains("CREATE PROC"))
            {
                FirstWorld = "CREATE PROC";
            }
            else if (text.Contains("ALTER PROCEDURE"))
            {
                FirstWorld = "ALTER PROCEDURE";
            }
            else if (text.Contains("ALTER PROC"))
            {
                FirstWorld = "ALTER PROC";
            }
            return FirstWorld;
        }
        public static int FindIndexOfWord(string fullProcedure, string text)
        {
            if(text != null)
            {
                int ProcedureStartingCharacterIndex = fullProcedure.IndexOf(text);
                return ProcedureStartingCharacterIndex;
            }
            else
            {
                return 0;
            }

        }

        public static string ExtractOnlyProcedureTextFromRawProcedure(string RawProcedure, int startIndex)
        {
            int EndIndex = RawProcedure.Length;
            int length = RawProcedure.Length - startIndex;
            string extracted = RawProcedure.Substring(startIndex, length);
            return extracted;
        }
    }
}
