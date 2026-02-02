using System.Reflection;
using System.Text;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for managing application resources such as images, icons, and other assets.
    /// </summary>
    public class ResourceService
    {
        private readonly FileSystemService _fileSystemService;

        public ResourceService()
        {
            _fileSystemService = new FileSystemService();
        }

        /// <summary>
        /// Gets the path to the application's resources directory.
        /// </summary>
        /// <returns>The resources directory path.</returns>
        public string GetResourcesPath()
        {
            return Path.Combine(_fileSystemService.GetLocalStoragePath(), "Resources");
        }

        /// <summary>
        /// Gets the path to the application's images directory.
        /// </summary>
        /// <returns>The images directory path.</returns>
        public string GetImagesPath()
        {
            return Path.Combine(GetResourcesPath(), "Images");
        }

        /// <summary>
        /// Gets the path to the application's icons directory.
        /// </summary>
        /// <returns>The icons directory path.</returns>
        public string GetIconsPath()
        {
            return Path.Combine(GetResourcesPath(), "Icons");
        }

        /// <summary>
        /// Ensures that all resource directories exist.
        /// </summary>
        public void EnsureResourceDirectoriesExist()
        {
            _fileSystemService.EnsureDirectoryExists(GetResourcesPath());
            _fileSystemService.EnsureDirectoryExists(GetImagesPath());
            _fileSystemService.EnsureDirectoryExists(GetIconsPath());
        }

        /// <summary>
        /// Copies embedded resources to the local file system.
        /// </summary>
        /// <param name="resourceName">The name of the embedded resource.</param>
        /// <param name="destinationPath">The destination file path.</param>
        public async Task CopyEmbeddedResourceAsync(string resourceName, string destinationPath)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream(resourceName);
                
                if (stream != null)
                {
                    var directory = Path.GetDirectoryName(destinationPath);
                    _fileSystemService.EnsureDirectoryExists(directory);
                    
                    using var fileStream = File.Create(destinationPath);
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to copy embedded resource {resourceName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets an embedded resource as a byte array.
        /// </summary>
        /// <param name="resourceName">The name of the embedded resource.</param>
        /// <returns>The resource as a byte array, or null if not found.</returns>
        public async Task<byte[]> GetEmbeddedResourceAsync(string resourceName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream(resourceName);
                
                if (stream != null)
                {
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get embedded resource {resourceName}: {ex.Message}");
            }
            
            return null;
        }

        /// <summary>
        /// Gets an embedded resource as a string.
        /// </summary>
        /// <param name="resourceName">The name of the embedded resource.</param>
        /// <param name="encoding">The text encoding (default: UTF8).</param>
        /// <returns>The resource as a string, or null if not found.</returns>
        public async Task<string> GetEmbeddedResourceStringAsync(string resourceName, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream(resourceName);
                
                if (stream != null)
                {
                    using var reader = new StreamReader(stream, encoding);
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get embedded resource string {resourceName}: {ex.Message}");
            }
            
            return null;
        }

        /// <summary>
        /// Gets all embedded resource names in the assembly.
        /// </summary>
        /// <returns>An array of embedded resource names.</returns>
        public string[] GetEmbeddedResourceNames()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                return assembly.GetManifestResourceNames();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get embedded resource names: {ex.Message}");
                return new string[0];
            }
        }

        /// <summary>
        /// Saves an image to the local file system.
        /// </summary>
        /// <param name="imageData">The image data as a byte array.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The full path to the saved image.</returns>
        public async Task<string> SaveImageAsync(byte[] imageData, string fileName)
        {
            var imagePath = Path.Combine(GetImagesPath(), fileName);
            await _fileSystemService.WriteBytesAsync(imagePath, imageData);
            return imagePath;
        }

        /// <summary>
        /// Loads an image from the local file system.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The image data as a byte array, or null if not found.</returns>
        public async Task<byte[]> LoadImageAsync(string fileName)
        {
            var imagePath = Path.Combine(GetImagesPath(), fileName);
            
            if (_fileSystemService.FileExists(imagePath))
            {
                return await _fileSystemService.ReadBytesAsync(imagePath);
            }
            
            return null;
        }

        /// <summary>
        /// Deletes an image from the local file system.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void DeleteImage(string fileName)
        {
            var imagePath = Path.Combine(GetImagesPath(), fileName);
            _fileSystemService.DeleteFile(imagePath);
        }

        /// <summary>
        /// Gets all image files in the images directory.
        /// </summary>
        /// <param name="searchPattern">The search pattern (default: "*.*").</param>
        /// <returns>An array of image file paths.</returns>
        public string[] GetImageFiles(string searchPattern = "*.*")
        {
            return _fileSystemService.GetFiles(GetImagesPath(), searchPattern);
        }

        /// <summary>
        /// Saves an icon to the local file system.
        /// </summary>
        /// <param name="iconData">The icon data as a byte array.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The full path to the saved icon.</returns>
        public async Task<string> SaveIconAsync(byte[] iconData, string fileName)
        {
            var iconPath = Path.Combine(GetIconsPath(), fileName);
            await _fileSystemService.WriteBytesAsync(iconPath, iconData);
            return iconPath;
        }

        /// <summary>
        /// Loads an icon from the local file system.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The icon data as a byte array, or null if not found.</returns>
        public async Task<byte[]> LoadIconAsync(string fileName)
        {
            var iconPath = Path.Combine(GetIconsPath(), fileName);
            
            if (_fileSystemService.FileExists(iconPath))
            {
                return await _fileSystemService.ReadBytesAsync(iconPath);
            }
            
            return null;
        }

        /// <summary>
        /// Deletes an icon from the local file system.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void DeleteIcon(string fileName)
        {
            var iconPath = Path.Combine(GetIconsPath(), fileName);
            _fileSystemService.DeleteFile(iconPath);
        }

        /// <summary>
        /// Gets all icon files in the icons directory.
        /// </summary>
        /// <param name="searchPattern">The search pattern (default: "*.*").</param>
        /// <returns>An array of icon file paths.</returns>
        public string[] GetIconFiles(string searchPattern = "*.*")
        {
            return _fileSystemService.GetFiles(GetIconsPath(), searchPattern);
        }

        /// <summary>
        /// Clears all cached resources.
        /// </summary>
        public void ClearCache()
        {
            try
            {
                var cachePath = _fileSystemService.GetCachePath();
                if (_fileSystemService.DirectoryExists(cachePath))
                {
                    _fileSystemService.DeleteDirectory(cachePath);
                    _fileSystemService.EnsureDirectoryExists(cachePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to clear cache: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the total size of cached resources in bytes.
        /// </summary>
        /// <returns>The total cache size in bytes.</returns>
        public long GetCacheSize()
        {
            try
            {
                var cachePath = _fileSystemService.GetCachePath();
                if (!_fileSystemService.DirectoryExists(cachePath))
                    return 0;

                var files = _fileSystemService.GetFiles(cachePath, "*.*");
                return files.Sum(file => _fileSystemService.GetFileSize(file));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get cache size: {ex.Message}");
                return 0;
            }
        }
    }
} 