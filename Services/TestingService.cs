using PermissionProMaui.Models;
using PermissionProMaui.Enums;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for testing and validating the migrated MAUI application functionality.
    /// </summary>
    public class TestingService
    {
        private readonly DatabaseService _databaseService;
        private readonly FileSystemService _fileSystemService;
        private readonly BiometricService _biometricService;
        private readonly ResourceService _resourceService;

        public TestingService()
        {
            _databaseService = new DatabaseService();
            _fileSystemService = new FileSystemService();
            _biometricService = new BiometricService();
            _resourceService = new ResourceService();
        }

        /// <summary>
        /// Runs comprehensive tests on the application.
        /// </summary>
        /// <returns>A test report with results.</returns>
        public async Task<TestReport> RunComprehensiveTestsAsync()
        {
            var report = new TestReport
            {
                Timestamp = DateTime.Now,
                Tests = new List<TestResult>()
            };

            // Database Tests
            await RunDatabaseTests(report);

            // File System Tests
            await RunFileSystemTests(report);

            // Biometric Tests
            await RunBiometricTests(report);

            // Resource Tests
            await RunResourceTests(report);

            // Service Tests
            await RunServiceTests(report);

            // Calculate overall results
            CalculateTestResults(report);

            return report;
        }

        /// <summary>
        /// Runs database-related tests.
        /// </summary>
        /// <param name="report">The test report to update.</param>
        private async Task RunDatabaseTests(TestReport report)
        {
            try
            {
                // Test database creation
                _databaseService.SafeUpdateDatabase();
                report.Tests.Add(new TestResult
                {
                    Category = "Database",
                    TestName = "Database Creation",
                    Status = TestStatus.Passed,
                    Message = "Database created/updated successfully"
                });

                // Test database version
                var version = _databaseService.GetDatabaseVersion();
                report.Tests.Add(new TestResult
                {
                    Category = "Database",
                    TestName = "Database Version",
                    Status = TestStatus.Passed,
                    Message = $"Database version: {version}"
                });

                // Test user settings
                // Note: This should be injected, but for now we'll create a temporary instance
                var appSettingsService = new AppUserSettingsService(new DatabaseService());
                var settings = appSettingsService.GetAppSettings();
                report.Tests.Add(new TestResult
                {
                    Category = "Database",
                    TestName = "User Settings",
                    Status = settings != null ? TestStatus.Passed : TestStatus.Failed,
                    Message = settings != null ? "User settings retrieved successfully" : "Failed to retrieve user settings"
                });

                await Task.CompletedTask; // Make the method properly async
            }
            catch (Exception ex)
            {
                report.Tests.Add(new TestResult
                {
                    Category = "Database",
                    TestName = "Database Tests",
                    Status = TestStatus.Failed,
                    Message = $"Database test failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Runs file system-related tests.
        /// </summary>
        /// <param name="report">The test report to update.</param>
        private async Task RunFileSystemTests(TestReport report)
        {
            try
            {
                // Test local storage path
                var storagePath = _fileSystemService.GetLocalStoragePath();
                report.Tests.Add(new TestResult
                {
                    Category = "File System",
                    TestName = "Local Storage Path",
                    Status = !string.IsNullOrEmpty(storagePath) ? TestStatus.Passed : TestStatus.Failed,
                    Message = $"Local storage path: {storagePath}"
                });

                // Test file operations
                var testFilePath = Path.Combine(storagePath, "test.txt");
                var testContent = "Test content for file system validation";
                
                await _fileSystemService.WriteTextAsync(testFilePath, testContent);
                var readContent = await _fileSystemService.ReadTextAsync(testFilePath);
                
                report.Tests.Add(new TestResult
                {
                    Category = "File System",
                    TestName = "File Read/Write",
                    Status = readContent == testContent ? TestStatus.Passed : TestStatus.Failed,
                    Message = readContent == testContent ? "File read/write operations successful" : "File read/write operations failed"
                });

                // Clean up test file
                _fileSystemService.DeleteFile(testFilePath);
            }
            catch (Exception ex)
            {
                report.Tests.Add(new TestResult
                {
                    Category = "File System",
                    TestName = "File System Tests",
                    Status = TestStatus.Failed,
                    Message = $"File system test failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Runs biometric-related tests.
        /// </summary>
        /// <param name="report">The test report to update.</param>
        private async Task RunBiometricTests(TestReport report)
        {
            try
            {
                // Test biometric availability
                var isAvailable = _biometricService.IsBiometricAvailable();
                report.Tests.Add(new TestResult
                {
                    Category = "Biometric",
                    TestName = "Biometric Availability",
                    Status = TestStatus.Passed,
                    Message = $"Biometric authentication available: {isAvailable}"
                });

                // Test biometric type
                var biometricType = _biometricService.GetBiometricType();
                report.Tests.Add(new TestResult
                {
                    Category = "Biometric",
                    TestName = "Biometric Type",
                    Status = TestStatus.Passed,
                    Message = $"Biometric type: {biometricType}"
                });

                await Task.CompletedTask; // Make the method properly async
            }
            catch (Exception ex)
            {
                report.Tests.Add(new TestResult
                {
                    Category = "Biometric",
                    TestName = "Biometric Tests",
                    Status = TestStatus.Failed,
                    Message = $"Biometric test failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Runs resource-related tests.
        /// </summary>
        /// <param name="report">The test report to update.</param>
        private async Task RunResourceTests(TestReport report)
        {
            try
            {
                // Test resource directories
                _resourceService.EnsureResourceDirectoriesExist();
                report.Tests.Add(new TestResult
                {
                    Category = "Resources",
                    TestName = "Resource Directories",
                    Status = TestStatus.Passed,
                    Message = "Resource directories created successfully"
                });

                // Test embedded resources
                var resourceNames = _resourceService.GetEmbeddedResourceNames();
                report.Tests.Add(new TestResult
                {
                    Category = "Resources",
                    TestName = "Embedded Resources",
                    Status = TestStatus.Passed,
                    Message = $"Found {resourceNames.Length} embedded resources"
                });

                await Task.CompletedTask; // Make the method properly async
            }
            catch (Exception ex)
            {
                report.Tests.Add(new TestResult
                {
                    Category = "Resources",
                    TestName = "Resource Tests",
                    Status = TestStatus.Failed,
                    Message = $"Resource test failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Runs service-related tests.
        /// </summary>
        /// <param name="report">The test report to update.</param>
        private async Task RunServiceTests(TestReport report)
        {
            try
            {
                // Test crypto service
                var cryptoService = new CryptoService();
                var testData = "Test data for encryption";
                var testKey = "testKey123";
                var encrypted = cryptoService.Encrypt(testKey, testData);
                var decrypted = cryptoService.Decrypt(testKey, encrypted, KeyType.Login);
                
                report.Tests.Add(new TestResult
                {
                    Category = "Services",
                    TestName = "Crypto Service",
                    Status = decrypted == testData ? TestStatus.Passed : TestStatus.Failed,
                    Message = decrypted == testData ? "Encryption/decryption successful" : "Encryption/decryption failed"
                });

                // Test QR code service
                var qrCodeService = new QrCodeService();
                var qrCodeData = qrCodeService.GenerateQrCode("Test QR Code Data");
                
                report.Tests.Add(new TestResult
                {
                    Category = "Services",
                    TestName = "QR Code Service",
                    Status = qrCodeData != null && qrCodeData.Length > 0 ? TestStatus.Passed : TestStatus.Failed,
                    Message = qrCodeData != null && qrCodeData.Length > 0 ? "QR code generation successful" : "QR code generation failed"
                });

                await Task.CompletedTask; // Make the method properly async
            }
            catch (Exception ex)
            {
                report.Tests.Add(new TestResult
                {
                    Category = "Services",
                    TestName = "Service Tests",
                    Status = TestStatus.Failed,
                    Message = $"Service test failed: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Calculates overall test results.
        /// </summary>
        /// <param name="report">The test report to update.</param>
        private void CalculateTestResults(TestReport report)
        {
            var totalTests = report.Tests.Count;
            var passedTests = report.Tests.Count(t => t.Status == TestStatus.Passed);
            var failedTests = report.Tests.Count(t => t.Status == TestStatus.Failed);

            report.TotalTests = totalTests;
            report.PassedTests = passedTests;
            report.FailedTests = failedTests;
            report.SuccessRate = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0;
            report.OverallStatus = failedTests == 0 ? TestStatus.Passed : TestStatus.Failed;
        }

        /// <summary>
        /// Generates a detailed test report as a string.
        /// </summary>
        /// <param name="report">The test report.</param>
        /// <returns>A formatted test report string.</returns>
        public string GenerateTestReport(TestReport report)
        {
            var sb = new System.Text.StringBuilder();
            
            sb.AppendLine("=== MAUI Migration Test Report ===");
            sb.AppendLine($"Timestamp: {report.Timestamp:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Overall Status: {report.OverallStatus}");
            sb.AppendLine($"Success Rate: {report.SuccessRate:F1}%");
            sb.AppendLine($"Total Tests: {report.TotalTests}");
            sb.AppendLine($"Passed: {report.PassedTests}");
            sb.AppendLine($"Failed: {report.FailedTests}");
            sb.AppendLine();

            var groupedTests = report.Tests.GroupBy(t => t.Category);
            foreach (var group in groupedTests)
            {
                sb.AppendLine($"--- {group.Key} Tests ---");
                foreach (var test in group)
                {
                    var statusIcon = test.Status == TestStatus.Passed ? "✅" : "❌";
                    sb.AppendLine($"{statusIcon} {test.TestName}: {test.Message}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents a comprehensive test report.
    /// </summary>
    public class TestReport
    {
        /// <summary>
        /// Gets or sets the timestamp when the tests were run.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the list of test results.
        /// </summary>
        public List<TestResult> Tests { get; set; }

        /// <summary>
        /// Gets or sets the total number of tests.
        /// </summary>
        public int TotalTests { get; set; }

        /// <summary>
        /// Gets or sets the number of passed tests.
        /// </summary>
        public int PassedTests { get; set; }

        /// <summary>
        /// Gets or sets the number of failed tests.
        /// </summary>
        public int FailedTests { get; set; }

        /// <summary>
        /// Gets or sets the success rate as a percentage.
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// Gets or sets the overall test status.
        /// </summary>
        public TestStatus OverallStatus { get; set; }
    }

    /// <summary>
    /// Represents an individual test result.
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Gets or sets the test category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the test name.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets or sets the test status.
        /// </summary>
        public TestStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the test message.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Represents the status of a test.
    /// </summary>
    public enum TestStatus
    {
        /// <summary>
        /// Test passed successfully.
        /// </summary>
        Passed,

        /// <summary>
        /// Test failed.
        /// </summary>
        Failed
    }
} 