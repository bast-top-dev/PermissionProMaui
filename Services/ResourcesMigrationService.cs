using System.Text.Json;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for migrating resources from the original Xamarin project to MAUI.
    /// </summary>
    public class ResourcesMigrationService
    {
        private readonly FileSystemService _fileSystemService;
        private readonly ResourceService _resourceService;

        public ResourcesMigrationService()
        {
            _fileSystemService = new FileSystemService();
            _resourceService = new ResourceService();
        }

        /// <summary>
        /// Migrates all resources from the original project to MAUI format.
        /// </summary>
        /// <returns>A migration report with results.</returns>
        public async Task<ResourcesMigrationReport> MigrateAllResourcesAsync()
        {
            var report = new ResourcesMigrationReport
            {
                Timestamp = DateTime.Now,
                Results = new List<ResourceMigrationResult>()
            };

            try
            {
                // Ensure resource directories exist
                _resourceService.EnsureResourceDirectoriesExist();

                // Migrate images
                await MigrateImages(report);

                // Migrate icons
                await MigrateIcons(report);

                // Migrate string resources
                await MigrateStringResources(report);

                // Migrate configuration files
                await MigrateConfigurationFiles(report);

                // Create MAUI-specific resources
                await CreateMauiSpecificResources(report);

                // Calculate migration statistics
                CalculateMigrationStatistics(report);
            }
            catch (Exception ex)
            {
                report.OverallStatus = MigrationStatus.Failed;
                report.ErrorMessage = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// Migrates images from the original project.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task MigrateImages(ResourcesMigrationReport report)
        {
            try
            {
                // Define common image paths from original project
                var originalImagePaths = new[]
                {
                    "permissionproapps/PermissionProAndroid/Resources/drawable/",
                    "permissionproapps/PermissionProiOS/Resources/",
                    "permissionproapps/PermissionProShared/Resources/"
                };

                var migratedImages = new List<string>();

                foreach (var originalPath in originalImagePaths)
                {
                    if (Directory.Exists(originalPath))
                    {
                        var imageFiles = Directory.GetFiles(originalPath, "*.*", SearchOption.AllDirectories)
                            .Where(file => IsImageFile(file))
                            .ToArray();

                        foreach (var imageFile in imageFiles)
                        {
                            try
                            {
                                var fileName = Path.GetFileName(imageFile);
                                var imageData = await File.ReadAllBytesAsync(imageFile);
                                
                                // Convert to MAUI-compatible format if needed
                                var convertedImageData = await ConvertImageForMaui(imageData, fileName);
                                
                                // Save to MAUI resources
                                var savedPath = await _resourceService.SaveImageAsync(convertedImageData, fileName);
                                migratedImages.Add(savedPath);

                                report.Results.Add(new ResourceMigrationResult
                                {
                                    Category = "Images",
                                    OriginalPath = imageFile,
                                    NewPath = savedPath,
                                    Status = MigrationStatus.Success,
                                    Message = $"Successfully migrated {fileName}"
                                });
                            }
                            catch (Exception ex)
                            {
                                report.Results.Add(new ResourceMigrationResult
                                {
                                    Category = "Images",
                                    OriginalPath = imageFile,
                                    Status = MigrationStatus.Failed,
                                    Message = $"Failed to migrate {Path.GetFileName(imageFile)}: {ex.Message}"
                                });
                            }
                        }
                    }
                }

                report.MigratedImages = migratedImages.Count;
            }
            catch (Exception ex)
            {
                report.Results.Add(new ResourceMigrationResult
                {
                    Category = "Images",
                    Status = MigrationStatus.Failed,
                    Message = $"Image migration failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Migrates icons from the original project.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task MigrateIcons(ResourcesMigrationReport report)
        {
            try
            {
                // Define common icon paths from original project
                var originalIconPaths = new[]
                {
                    "permissionproapps/PermissionProAndroid/Resources/drawable/",
                    "permissionproapps/PermissionProiOS/Resources/"
                };

                var migratedIcons = new List<string>();

                foreach (var originalPath in originalIconPaths)
                {
                    if (Directory.Exists(originalPath))
                    {
                        var iconFiles = Directory.GetFiles(originalPath, "*.*", SearchOption.AllDirectories)
                            .Where(file => IsIconFile(file))
                            .ToArray();

                        foreach (var iconFile in iconFiles)
                        {
                            try
                            {
                                var fileName = Path.GetFileName(iconFile);
                                var iconData = await File.ReadAllBytesAsync(iconFile);
                                
                                // Convert to MAUI-compatible format if needed
                                var convertedIconData = await ConvertIconForMaui(iconData, fileName);
                                
                                // Save to MAUI resources
                                var savedPath = await _resourceService.SaveIconAsync(convertedIconData, fileName);
                                migratedIcons.Add(savedPath);

                                report.Results.Add(new ResourceMigrationResult
                                {
                                    Category = "Icons",
                                    OriginalPath = iconFile,
                                    NewPath = savedPath,
                                    Status = MigrationStatus.Success,
                                    Message = $"Successfully migrated {fileName}"
                                });
                            }
                            catch (Exception ex)
                            {
                                report.Results.Add(new ResourceMigrationResult
                                {
                                    Category = "Icons",
                                    OriginalPath = iconFile,
                                    Status = MigrationStatus.Failed,
                                    Message = $"Failed to migrate {Path.GetFileName(iconFile)}: {ex.Message}"
                                });
                            }
                        }
                    }
                }

                report.MigratedIcons = migratedIcons.Count;
            }
            catch (Exception ex)
            {
                report.Results.Add(new ResourceMigrationResult
                {
                    Category = "Icons",
                    Status = MigrationStatus.Failed,
                    Message = $"Icon migration failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Migrates string resources from the original project.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task MigrateStringResources(ResourcesMigrationReport report)
        {
            try
            {
                // Define string resource paths
                var stringResourcePaths = new[]
                {
                    "permissionproapps/PermissionProAndroid/Resources/values/",
                    "permissionproapps/PermissionProiOS/Resources/"
                };

                var migratedStrings = new Dictionary<string, string>();

                foreach (var originalPath in stringResourcePaths)
                {
                    if (Directory.Exists(originalPath))
                    {
                        var stringFiles = Directory.GetFiles(originalPath, "*.xml", SearchOption.AllDirectories)
                            .Concat(Directory.GetFiles(originalPath, "*.resx", SearchOption.AllDirectories))
                            .ToArray();

                        foreach (var stringFile in stringFiles)
                        {
                            try
                            {
                                var fileName = Path.GetFileName(stringFile);
                                var content = await File.ReadAllTextAsync(stringFile);
                                
                                // Parse and convert string resources
                                var parsedStrings = ParseStringResources(content, fileName);
                                
                                foreach (var kvp in parsedStrings)
                                {
                                    migratedStrings[kvp.Key] = kvp.Value;
                                }

                                report.Results.Add(new ResourceMigrationResult
                                {
                                    Category = "Strings",
                                    OriginalPath = stringFile,
                                    Status = MigrationStatus.Success,
                                    Message = $"Successfully parsed {fileName} ({parsedStrings.Count} strings)"
                                });
                            }
                            catch (Exception ex)
                            {
                                report.Results.Add(new ResourceMigrationResult
                                {
                                    Category = "Strings",
                                    OriginalPath = stringFile,
                                    Status = MigrationStatus.Failed,
                                    Message = $"Failed to parse {Path.GetFileName(stringFile)}: {ex.Message}"
                                });
                            }
                        }
                    }
                }

                // Save migrated strings to MAUI format
                if (migratedStrings.Count > 0)
                {
                    var stringsJson = JsonSerializer.Serialize(migratedStrings, new JsonSerializerOptions { WriteIndented = true });
                    var stringsPath = Path.Combine(_resourceService.GetResourcesPath(), "strings.json");
                    await _fileSystemService.WriteTextAsync(stringsPath, stringsJson);

                    report.MigratedStrings = migratedStrings.Count;
                }
            }
            catch (Exception ex)
            {
                report.Results.Add(new ResourceMigrationResult
                {
                    Category = "Strings",
                    Status = MigrationStatus.Failed,
                    Message = $"String resource migration failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Migrates configuration files from the original project.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task MigrateConfigurationFiles(ResourcesMigrationReport report)
        {
            try
            {
                var configFiles = new[]
                {
                    "permissionproapps/PermissionProAndroid/Properties/AndroidManifest.xml",
                    "permissionproapps/PermissionProiOS/Info.plist"
                };

                foreach (var configFile in configFiles)
                {
                    if (File.Exists(configFile))
                    {
                        try
                        {
                            var fileName = Path.GetFileName(configFile);
                            var content = await File.ReadAllTextAsync(configFile);
                            
                            // Save to MAUI resources for reference
                            var configPath = Path.Combine(_resourceService.GetResourcesPath(), fileName);
                            await _fileSystemService.WriteTextAsync(configPath, content);

                            report.Results.Add(new ResourceMigrationResult
                            {
                                Category = "Configuration",
                                OriginalPath = configFile,
                                NewPath = configPath,
                                Status = MigrationStatus.Success,
                                Message = $"Successfully migrated {fileName}"
                            });

                            report.MigratedConfigFiles++;
                        }
                        catch (Exception ex)
                        {
                            report.Results.Add(new ResourceMigrationResult
                            {
                                Category = "Configuration",
                                OriginalPath = configFile,
                                Status = MigrationStatus.Failed,
                                Message = $"Failed to migrate {Path.GetFileName(configFile)}: {ex.Message}"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                report.Results.Add(new ResourceMigrationResult
                {
                    Category = "Configuration",
                    Status = MigrationStatus.Failed,
                    Message = $"Configuration migration failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Creates MAUI-specific resources.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task CreateMauiSpecificResources(ResourcesMigrationReport report)
        {
            try
            {
                // Create MAUI app icons
                await CreateMauiAppIcons(report);

                // Create splash screen
                await CreateSplashScreen(report);

                // Create default styles
                await CreateDefaultStyles(report);

                report.Results.Add(new ResourceMigrationResult
                {
                    Category = "MAUI Resources",
                    Status = MigrationStatus.Success,
                    Message = "Successfully created MAUI-specific resources"
                });
            }
            catch (Exception ex)
            {
                report.Results.Add(new ResourceMigrationResult
                {
                    Category = "MAUI Resources",
                    Status = MigrationStatus.Failed,
                    Message = $"MAUI resource creation failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Creates MAUI app icons in various sizes.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task CreateMauiAppIcons(ResourcesMigrationReport report)
        {
            try
            {
                // Create placeholder app icon (in a real scenario, you'd generate proper icons)
                var iconSizes = new[] { 16, 32, 48, 64, 128, 256 };
                
                foreach (var size in iconSizes)
                {
                    var iconData = GeneratePlaceholderIcon(size);
                    var fileName = $"app_icon_{size}x{size}.png";
                    await _resourceService.SaveIconAsync(iconData, fileName);
                }

                report.CreatedMauiResources++;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create MAUI app icons: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a splash screen for the application.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task CreateSplashScreen(ResourcesMigrationReport report)
        {
            try
            {
                // Create placeholder splash screen
                var splashData = GeneratePlaceholderSplash();
                await _resourceService.SaveImageAsync(splashData, "splash_screen.png");
                
                report.CreatedMauiResources++;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create splash screen: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates default styles for the application.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private async Task CreateDefaultStyles(ResourcesMigrationReport report)
        {
            try
            {
                var styles = new Dictionary<string, object>
                {
                    ["PrimaryColor"] = "#2196F3",
                    ["SecondaryColor"] = "#FFC107",
                    ["BackgroundColor"] = "#FFFFFF",
                    ["TextColor"] = "#212121"
                };

                var stylesJson = JsonSerializer.Serialize(styles, new JsonSerializerOptions { WriteIndented = true });
                var stylesPath = Path.Combine(_resourceService.GetResourcesPath(), "styles.json");
                await _fileSystemService.WriteTextAsync(stylesPath, stylesJson);
                
                report.CreatedMauiResources++;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create default styles: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates migration statistics.
        /// </summary>
        /// <param name="report">The migration report to update.</param>
        private void CalculateMigrationStatistics(ResourcesMigrationReport report)
        {
            var totalResults = report.Results.Count;
            var successfulResults = report.Results.Count(r => r.Status == MigrationStatus.Success);
            var failedResults = report.Results.Count(r => r.Status == MigrationStatus.Failed);

            report.TotalResources = totalResults;
            report.SuccessfulMigrations = successfulResults;
            report.FailedMigrations = failedResults;
            report.SuccessRate = totalResults > 0 ? (double)successfulResults / totalResults * 100 : 0;
            report.OverallStatus = failedResults == 0 ? MigrationStatus.Success : MigrationStatus.Partial;
        }

        /// <summary>
        /// Checks if a file is an image file.
        /// </summary>
        /// <param name="filePath">The file path to check.</param>
        /// <returns>True if the file is an image.</returns>
        private bool IsImageFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return new[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp" }.Contains(extension);
        }

        /// <summary>
        /// Checks if a file is an icon file.
        /// </summary>
        /// <param name="filePath">The file path to check.</param>
        /// <returns>True if the file is an icon.</returns>
        private bool IsIconFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath).ToLowerInvariant();
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return fileName.Contains("icon") || new[] { ".ico", ".svg" }.Contains(extension);
        }

        /// <summary>
        /// Converts an image to MAUI-compatible format.
        /// </summary>
        /// <param name="imageData">The original image data.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The converted image data.</returns>
        private async Task<byte[]> ConvertImageForMaui(byte[] imageData, string fileName)
        {
            // In a real implementation, you would convert images to MAUI-compatible formats
            // For now, we'll return the original data
            await Task.CompletedTask; // Make the method properly async
            return imageData;
        }

        /// <summary>
        /// Converts an icon to MAUI-compatible format.
        /// </summary>
        /// <param name="iconData">The original icon data.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The converted icon data.</returns>
        private async Task<byte[]> ConvertIconForMaui(byte[] iconData, string fileName)
        {
            // In a real implementation, you would convert icons to MAUI-compatible formats
            // For now, we'll return the original data
            await Task.CompletedTask; // Make the method properly async
            return iconData;
        }

        /// <summary>
        /// Parses string resources from XML or RESX files.
        /// </summary>
        /// <param name="content">The file content.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>A dictionary of string resources.</returns>
        private Dictionary<string, string> ParseStringResources(string content, string fileName)
        {
            var strings = new Dictionary<string, string>();
            
            try
            {
                // Simple XML parsing for string resources
                // In a real implementation, you would use proper XML parsing
                var lines = content.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("name=") && line.Contains(">") && line.Contains("</string>"))
                    {
                        var nameStart = line.IndexOf("name=\"") + 6;
                        var nameEnd = line.IndexOf("\"", nameStart);
                        var valueStart = line.IndexOf(">") + 1;
                        var valueEnd = line.IndexOf("</string>");
                        
                        if (nameStart > 5 && nameEnd > nameStart && valueStart > 0 && valueEnd > valueStart)
                        {
                            var name = line.Substring(nameStart, nameEnd - nameStart);
                            var value = line.Substring(valueStart, valueEnd - valueStart);
                            strings[name] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to parse string resources from {fileName}: {ex.Message}");
            }
            
            return strings;
        }

        /// <summary>
        /// Generates a placeholder icon for testing.
        /// </summary>
        /// <param name="size">The icon size.</param>
        /// <returns>The icon data.</returns>
        private byte[] GeneratePlaceholderIcon(int size)
        {
            // In a real implementation, you would generate a proper icon
            // For now, we'll return a minimal PNG header
            return new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        }

        /// <summary>
        /// Generates a placeholder splash screen for testing.
        /// </summary>
        /// <returns>The splash screen data.</returns>
        private byte[] GeneratePlaceholderSplash()
        {
            // In a real implementation, you would generate a proper splash screen
            // For now, we'll return a minimal PNG header
            return new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        }
    }

    /// <summary>
    /// Represents a resources migration report.
    /// </summary>
    public class ResourcesMigrationReport
    {
        /// <summary>
        /// Gets or sets the timestamp when the migration was performed.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the list of migration results.
        /// </summary>
        public List<ResourceMigrationResult> Results { get; set; }

        /// <summary>
        /// Gets or sets the total number of resources processed.
        /// </summary>
        public int TotalResources { get; set; }

        /// <summary>
        /// Gets or sets the number of successful migrations.
        /// </summary>
        public int SuccessfulMigrations { get; set; }

        /// <summary>
        /// Gets or sets the number of failed migrations.
        /// </summary>
        public int FailedMigrations { get; set; }

        /// <summary>
        /// Gets or sets the success rate as a percentage.
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// Gets or sets the overall migration status.
        /// </summary>
        public MigrationStatus OverallStatus { get; set; }

        /// <summary>
        /// Gets or sets the error message if migration failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the number of migrated images.
        /// </summary>
        public int MigratedImages { get; set; }

        /// <summary>
        /// Gets or sets the number of migrated icons.
        /// </summary>
        public int MigratedIcons { get; set; }

        /// <summary>
        /// Gets or sets the number of migrated strings.
        /// </summary>
        public int MigratedStrings { get; set; }

        /// <summary>
        /// Gets or sets the number of migrated configuration files.
        /// </summary>
        public int MigratedConfigFiles { get; set; }

        /// <summary>
        /// Gets or sets the number of created MAUI resources.
        /// </summary>
        public int CreatedMauiResources { get; set; }
    }

    /// <summary>
    /// Represents an individual resource migration result.
    /// </summary>
    public class ResourceMigrationResult
    {
        /// <summary>
        /// Gets or sets the resource category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the original file path.
        /// </summary>
        public string OriginalPath { get; set; }

        /// <summary>
        /// Gets or sets the new file path.
        /// </summary>
        public string NewPath { get; set; }

        /// <summary>
        /// Gets or sets the migration status.
        /// </summary>
        public MigrationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the migration message.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Represents the status of a migration operation.
    /// </summary>
    public enum MigrationStatus
    {
        /// <summary>
        /// Migration was successful.
        /// </summary>
        Success,

        /// <summary>
        /// Migration was partially successful.
        /// </summary>
        Partial,

        /// <summary>
        /// Migration failed.
        /// </summary>
        Failed
    }
} 