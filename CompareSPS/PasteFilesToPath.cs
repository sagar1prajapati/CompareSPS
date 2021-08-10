using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;

namespace CompareSPS
{
   public static class PasteFilesToOutputPath
    {
        public static string SourceFilePath = ConfigurationManager.AppSettings["SOURCE_FILES_PATH"];
        public static string DestinationFilePath = ConfigurationManager.AppSettings["DESTINATION_FILES_PATH"];
        public static string EXTRACTED_FILES = ConfigurationManager.AppSettings["EXTRACTED_FILES"];

        public static void CopyPasteNewFilesFoundOnLocal(string FileName)
        {
            CreateNewFolderIfNotExist(FormatPath.GetFilePath(EXTRACTED_FILES, "NEW_FOUND_ON_SOURCE"));

            string SourcePath = FormatPath.GetFilePath(SourceFilePath, FileName);
            string DestinationPath = FormatPath.GetFilePath(FormatPath.GetFilePath(EXTRACTED_FILES, "NEW_FOUND_ON_SOURCE"), FileName);

            CopyFiles(SourcePath, DestinationPath);  
        }

        public static void CopyPasteNewFilesFoundOnDestination(string FileName)
        {
            CreateNewFolderIfNotExist(FormatPath.GetFilePath(EXTRACTED_FILES, "NEW_FOUND_ON_DESTINATION"));
           
            string SourcePath = FormatPath.GetFilePath(DestinationFilePath, FileName);
            string DestinationPath = FormatPath.GetFilePath(FormatPath.GetFilePath(EXTRACTED_FILES, "NEW_FOUND_ON_DESTINATION"), FileName);

            CopyFiles(SourcePath, DestinationPath);
        }
        public static void CopyPasteConflictingFilesOnOutFiles(string FileName)
        {
            string conflictFileSourcePath = FormatPath.GetFilePath(EXTRACTED_FILES, "CONFLICT_FILES", "SOURCE");
            string conflictFileDestinationPath = FormatPath.GetFilePath(EXTRACTED_FILES, "CONFLICT_FILES", "DESTINATION");

            CreateNewFolderIfNotExist(conflictFileSourcePath);
            CreateNewFolderIfNotExist(conflictFileDestinationPath);
            
            string SourcePath = FormatPath.GetFilePath(SourceFilePath, FileName);  
            string ConfilctFileSourcePath = FormatPath.GetFilePath(conflictFileSourcePath, FileName);  
            CopyFiles(SourcePath, ConfilctFileSourcePath);  // Store Source Files On Conflict Source

            string TargetPath = FormatPath.GetFilePath(DestinationFilePath, FileName);
            string ConfilctFileDestinationPath = FormatPath.GetFilePath(conflictFileDestinationPath, FileName); 
            CopyFiles(TargetPath, ConfilctFileDestinationPath);   // Store Source Files On Conflict Destination
        }
        public static void CopyFiles(string SourcePath,string DestinationPath)
        {
            try
            {
                File.Copy(SourcePath, DestinationPath, true);
                Thread.Sleep(100);
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
        }

        public static void CreateNewFolderIfNotExist(string FolderName)
        {
            try
            {
                if (!Directory.Exists(FolderName))
                {
                    Directory.CreateDirectory(FolderName);
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
        }

    }
}
