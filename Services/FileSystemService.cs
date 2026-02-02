using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling file system operations
    /// </summary>
    public class FileSystemService
    {
        private readonly string _appDataPath;

        public FileSystemService()
        {
            _appDataPath = FileSystem.AppDataDirectory;
        }

        /// <summary>
        /// Gets the local storage path
        /// </summary>
        /// <returns>The local storage path</returns>
        public string GetLocalStoragePath()
        {
            return _appDataPath;
        }

        /// <summary>
        /// Gets the cache path
        /// </summary>
        /// <returns>The cache path</returns>
        public string GetCachePath()
        {
            return Path.Combine(_appDataPath, "Cache");
        }

        /// <summary>
        /// Ensures a directory exists, creating it if necessary
        /// </summary>
        /// <param name="path">The directory path</param>
        public void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Checks if a directory exists
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <returns>True if the directory exists</returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Deletes a directory
        /// </summary>
        /// <param name="path">The directory path</param>
        public void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>True if the file exists</returns>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Gets the size of a file
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The file size in bytes</returns>
        public long GetFileSize(string path)
        {
            if (File.Exists(path))
            {
                return new FileInfo(path).Length;
            }
            return 0;
        }

        /// <summary>
        /// Gets all files in a directory
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <returns>Array of file paths</returns>
        public string[] GetFiles(string path)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path);
            }
            return new string[0];
        }

        /// <summary>
        /// Gets all files in a directory with a specific pattern
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <param name="pattern">The search pattern</param>
        /// <returns>Array of file paths</returns>
        public string[] GetFiles(string path, string pattern)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path, pattern);
            }
            return new string[0];
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="path">The file path</param>
        public void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Writes text to a file asynchronously
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="content">The content to write</param>
        public async Task WriteTextAsync(string path, string content)
        {
            await File.WriteAllTextAsync(path, content);
        }

        /// <summary>
        /// Reads text from a file asynchronously
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The file content</returns>
        public async Task<string> ReadTextAsync(string path)
        {
            if (File.Exists(path))
            {
                return await File.ReadAllTextAsync(path);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes bytes to a file asynchronously
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="bytes">The bytes to write</param>
        public async Task WriteBytesAsync(string path, byte[] bytes)
        {
            await File.WriteAllBytesAsync(path, bytes);
        }

        /// <summary>
        /// Reads bytes from a file asynchronously
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The file bytes</returns>
        public async Task<byte[]> ReadBytesAsync(string path)
        {
            if (File.Exists(path))
            {
                return await File.ReadAllBytesAsync(path);
            }
            return new byte[0];
        }

        /// <summary>
        /// Writes an error log to the file system
        /// </summary>
        /// <param name="errorMessage">The error message to write</param>
        /// <param name="errorType">The type of error</param>
        /// <param name="timestamp">The timestamp of the error</param>
        public async Task WriteErrorLogAsync(string errorMessage, string errorType, string timestamp)
        {
            try
            {
                var logDirectory = Path.Combine(_appDataPath, "Logs");
                Directory.CreateDirectory(logDirectory);

                var logFile = Path.Combine(logDirectory, $"error_log_{DateTime.Now:yyyyMMdd}.txt");
                var logEntry = $"[{timestamp}] {errorType}: {errorMessage}\n";

                await File.AppendAllTextAsync(logFile, logEntry);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to write error log to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads error logs from the file system
        /// </summary>
        /// <param name="date">The date to read logs for</param>
        /// <returns>The log content as a string</returns>
        public async Task<string> ReadErrorLogsAsync(DateTime date)
        {
            try
            {
                var logDirectory = Path.Combine(_appDataPath, "Logs");
                var logFile = Path.Combine(logDirectory, $"error_log_{date:yyyyMMdd}.txt");

                if (File.Exists(logFile))
                {
                    return await File.ReadAllTextAsync(logFile);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to read error logs: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Saves application data to a file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="content">Content to save</param>
        public async Task SaveDataAsync(string fileName, string content)
        {
            try
            {
                var filePath = Path.Combine(_appDataPath, fileName);
                await File.WriteAllTextAsync(filePath, content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save data: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads application data from a file
        /// </summary>
        /// <param name="fileName">Name of the file to load</param>
        /// <returns>The file content as a string</returns>
        public async Task<string> LoadDataAsync(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_appDataPath, fileName);
                if (File.Exists(filePath))
                {
                    return await File.ReadAllTextAsync(filePath);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load data: {ex.Message}");
                return string.Empty;
            }
        }
    }
} 