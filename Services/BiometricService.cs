using Microsoft.Maui.Platform;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Cross-platform biometric authentication service.
    /// </summary>
    public class BiometricService
    {
        private readonly IBiometricHandler _handler;

        public BiometricService()
        {
            _handler = CreatePlatformHandler();
        }

        /// <summary>
        /// Creates the appropriate platform-specific biometric handler.
        /// </summary>
        /// <returns>The platform-specific biometric handler.</returns>
        private IBiometricHandler CreatePlatformHandler()
        {
            return new DefaultBiometricHandler();
        }

        /// <summary>
        /// Checks if biometric authentication is available on the current device.
        /// </summary>
        /// <returns>True if biometric authentication is available.</returns>
        public bool IsBiometricAvailable()
        {
            return _handler.IsBiometricAvailable();
        }

        /// <summary>
        /// Authenticates the user using biometric credentials.
        /// </summary>
        /// <param name="title">The title for the authentication dialog.</param>
        /// <param name="subtitle">The subtitle for the authentication dialog.</param>
        /// <param name="callback">Callback to handle authentication result.</param>
        public void Authenticate(string title, string subtitle, Action<bool, string> callback)
        {
            _handler.Authenticate(title, subtitle, callback);
        }

        /// <summary>
        /// Authenticates the user using biometric credentials asynchronously.
        /// </summary>
        /// <param name="title">The title for the authentication dialog.</param>
        /// <param name="subtitle">The subtitle for the authentication dialog.</param>
        /// <returns>A task that represents the authentication result.</returns>
        public async Task<(bool Success, string Message)> AuthenticateAsync(string title, string subtitle)
        {
            var tcs = new TaskCompletionSource<(bool, string)>();

            Authenticate(title, subtitle, (success, message) =>
            {
                tcs.SetResult((success, message));
            });

            return await tcs.Task;
        }

        /// <summary>
        /// Gets the type of biometric authentication available.
        /// </summary>
        /// <returns>The biometric type as a string.</returns>
        public string GetBiometricType()
        {
            return IsBiometricAvailable() ? "Biometric" : "None";
        }
    }

    /// <summary>
    /// Default biometric handler for platforms that don't support biometric authentication.
    /// </summary>
    public class DefaultBiometricHandler : IBiometricHandler
    {
        public bool IsBiometricAvailable()
        {
            return false;
        }

        public void Authenticate(string title, string subtitle, Action<bool, string> callback)
        {
            callback.Invoke(false, "Biometric authentication not supported on this platform");
        }
    }

    /// <summary>
    /// Interface for biometric authentication handlers.
    /// </summary>
    public interface IBiometricHandler
    {
        /// <summary>
        /// Checks if biometric authentication is available.
        /// </summary>
        /// <returns>True if biometric authentication is available.</returns>
        bool IsBiometricAvailable();

        /// <summary>
        /// Authenticates the user using biometric credentials.
        /// </summary>
        /// <param name="title">The title for the authentication dialog.</param>
        /// <param name="subtitle">The subtitle for the authentication dialog.</param>
        /// <param name="callback">Callback to handle authentication result.</param>
        void Authenticate(string title, string subtitle, Action<bool, string> callback);
    }
} 