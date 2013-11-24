using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace Library.Common
{
    /// <summary>
    /// helper class provides some functions to operate files and directories
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// copy a file into the destination directory
        /// </summary>
        /// <param name="filePath"> full file path which includes the file name</param>
        /// <param name="targetDirectory">the directory the file will be copyed into</param>
        public static void CopyFile(string filePath,string targetDirectory)
        {
            FileInfo file = new FileInfo(filePath);
            string targetPath = targetDirectory + "\\" + file.Name;

            /* if the target file already exists,make sure the target file
            isn't  read only */
            if (File.Exists(targetPath))
            {
                var xFile = new FileInfo(targetPath);
                xFile.IsReadOnly = false;
            }
            File.Copy(filePath, targetPath, true);
        }

        /// <summary>
        /// copy some files into destination directory
        /// </summary>
        /// <param name="filePaths">full file path list</param>
        /// <param name="targetDirectory">the directory the files wil be copyed into</param>
        public static void CopyFiles(IEnumerable<string> filePaths, string targetDirectory)
        {
            if (filePaths.Any())
            {
                foreach (string path in filePaths)
                {
                    CopyFile(path, targetDirectory);
                }
            }
        }

        /// <summary>
        /// save bytes to a file
        /// </summary>
        /// <param name="filePath">full file path</param>
        /// <param name="buffer"></param>
        public static void SaveFile(string filePath, byte[] buffer)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                fileStream.Flush();
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// save a stream to a file
        /// </summary>
        /// <param name="filePath">full file path</param>
        /// <param name="stream"></param>
        public static void SaveFile(string filePath, Stream stream)
        {
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length - 1);

            SaveFile(filePath, bytes);
        }

        /// <summary>
        /// get bytes from a file
        /// </summary>
        /// <param name="filePath">full file path</param>
        /// <returns></returns>
        public static byte[] GetFileBytes(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// delete a file
        /// </summary>
        /// <param name="filePath">full file path</param>
        public static void DeleteFile(string filePath)
        {
            FileInfo file = new FileInfo(filePath);

            if (file.IsReadOnly)
                file.SetAccessControl(new FileSecurity(filePath, AccessControlSections.Owner));

            file.Delete();
        }

        /// <summary>
        /// get a file size
        /// </summary>
        /// <param name="fileLength">file length</param>
        /// <returns></returns>
        public static string GetFileSize(long fileLength)
        {
            if (fileLength >= 1073741824)
            {
                Decimal size = Decimal.Divide(fileLength, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (fileLength >= 1048576)
            {
                Decimal size = Decimal.Divide(fileLength, 1048576);
                return String.Format("{0:##.##} MB", size);
            }
            else if (fileLength >= 1024)
            {
                Decimal size = Decimal.Divide(fileLength, 1024);
                return String.Format("{0:##.##} KB", size);
            }
            else if (fileLength > 0 & fileLength < 1024)
            {
                Decimal size = fileLength;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }

    }
}
