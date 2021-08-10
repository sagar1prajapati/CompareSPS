using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CompareSPS
{
    class Program
    {
        public static string SourceFilePath = ConfigurationManager.AppSettings["SOURCE_FILES_PATH"];
        public static string DestinationFilePath = ConfigurationManager.AppSettings["DESTINATION_FILES_PATH"];
        static void Main(string[] args)
        {
           
            List<string> sourceFiles = new List<string>();
            List<string> targetFiles = new List<string>();
            
            int index = 0;

            string[] localFilesPaths = Directory.GetFiles(Program.SourceFilePath, "*.sql");
            foreach (var file in localFilesPaths)
            {
                sourceFiles.Add(Path.GetFileName(file));
                if (index == 0)
                {
                    CollectStoreProcedureInStringFormat(file);
                }
            }

            #region New File Found On Local
            string[] targetFilesPaths = Directory.GetFiles(Program.DestinationFilePath, "*.sql");
                foreach (var file in targetFilesPaths)
                {
                    targetFiles.Add(Path.GetFileName(file));
                }

                var newFilesOnLocal = FilesNewFoundOnLocal(sourceFiles, targetFiles);
                Console.WriteLine("New Files Found On Sorce Folder :");
                foreach (var file in newFilesOnLocal)
                {
                    Console.WriteLine("\t" + file);

                    PasteFilesToOutputPath.CopyPasteNewFilesFoundOnLocal(file);
                }

            #endregion
                
            #region New Files Found On Production
            Console.WriteLine("\n");
            var NewFilesOnProduction = FilesNewFoundOnProduction(sourceFiles, targetFiles);

            Console.WriteLine("New Files Found On Target Folder :");
            foreach (var file in NewFilesOnProduction)
            {
                Console.WriteLine("\t" + file);
                PasteFilesToOutputPath.CopyPasteNewFilesFoundOnDestination(file);
            }
            #endregion

            #region Common Files Found on Local AND Production

            var commonFilesBetweenLocalAndPro = CommonFileFoundBetweenLocalAndProduction(sourceFiles, targetFiles);

            //Console.WriteLine("Common Files Found Between Source And Target :");
            //foreach (var file in commonFilesBetweenLocalAndPro)
            //{
            //    Console.WriteLine("\t" + file);
            //}



            #endregion

            #region DifferanceFoundInProceduresFromSourceAndTaget

            Console.WriteLine("\n");
            var ConflictingFilesFound = DifferanceFoundInProceduresFromSourceAndTaget(commonFilesBetweenLocalAndPro);

            Console.WriteLine("Conflicting Common Files Found Between Store Procedures Found in Source And Target :");
            foreach (var file in ConflictingFilesFound)
            {
                Console.WriteLine("\t" + file);
                PasteFilesToOutputPath.CopyPasteConflictingFilesOnOutFiles(file);
            }
            Console.ReadLine();
            #endregion
        }

        #region Private Function 
        public static void IterateAllFilesName(List<string> localFiles)
        {
            foreach (var file in localFiles)
            {
                Console.WriteLine(file);
            }
        }

        public static List<string> FilesNewFoundOnProduction(List<string> localFiles, List<string> productionFiles)
        {
            var NewFilesOnProduction = new List<string>();

            foreach (var item in productionFiles)
            {
                if (!localFiles.Contains(item))
                {
                    NewFilesOnProduction.Add(item);
                }
            }

            return NewFilesOnProduction;
        }

        public static List<string> FilesNewFoundOnLocal(List<string> localFiles, List<string> productionFiles)
        {
            var NewFilesOnLocal = new List<string>();
            foreach (var item in localFiles)
            {
                if (!productionFiles.Contains(item))
                {
                    NewFilesOnLocal.Add(item);
                }
            }
            return NewFilesOnLocal;

        }

        public static void FilesNewFoundOnLocal(List<string> localFiles)
        {
            foreach (var file in localFiles)
            {
                Console.WriteLine(file);
            }
        }

        public static void readFile(string fileName)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                 //new System.IO.StreamReader(@"c:\test.txt");
                 new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
        }

        public static void CollectStoreProcedureInStringFormat(string fileName)
        {
            int counter = 0;
            string line = null;


            StringBuilder storeProcedureInString = new StringBuilder();

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                 //new System.IO.StreamReader(@"c:\test.txt");
                 new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                storeProcedureInString.Append(line);
                counter++;
            }

            file.Close();
            //System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            //System.Console.ReadLine();

            storeProcedureInString.ToString();
            //  Console.WriteLine(storeProcedureInString);

            string SP = CleanStoreProcedure(storeProcedureInString.ToString());
            //Console.WriteLine(SP);
        }
        public static string CleanStoreProcedure(string procedure)
        {
            string StoreProcedureInUpperFormat = procedure.ToUpper();
            int i = StoreProcedureInUpperFormat.IndexOf("CREATE PROC");
            return StoreProcedureInUpperFormat;
        }

        public static IEnumerable<string> CommonFileFoundBetweenLocalAndProduction(List<string> localFiles, List<string> productionFiles)
        {
            var CommonList = localFiles.Intersect(productionFiles);
            return CommonList;
        }


        public static IEnumerable<string> DifferanceFoundInProceduresFromSourceAndTaget(IEnumerable<string> commonList)
        {
            List<string> conflictingProcedures = new List<string>();

            foreach (var item in commonList)
            {
                //string SourceFilePath = Program.SourceFilePath + "\\"+item ;
                //SourceFilePath = SourceFilePath.Trim();
               string sourceFilePath = FormatPath.GetFilePath(Program.SourceFilePath, item);
               string Procedure = ExtractProcedure.ExtractPureProcedure(sourceFilePath);

               string targetFilePath = FormatPath.GetFilePath(Program.DestinationFilePath, item);
               string Procedure2 = ExtractProcedure.ExtractPureProcedure(targetFilePath);

                if (!String.Equals(Procedure, Procedure2))
                {
                    //Console.WriteLine("Differance Found");
                    conflictingProcedures.Add(item);
                }
                
            }
            return conflictingProcedures;
        }

        #endregion 
    }
}
