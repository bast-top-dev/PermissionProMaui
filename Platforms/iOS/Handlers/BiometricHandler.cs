#if IOS
using Foundation;
using LocalAuthentication;
using UIKit;

namespace PermissionProMaui.Platforms.iOS.Handlers
{
    /// <summary>
    /// Platform-specific biometric authentication handler for iOS.
    /// </summary>
    public class BiometricHandler : IBiometricHandler
    {
        private readonly LAContext _context;

        public BiometricHandler()
        {
            _context = new LAContext();
        }

        /// <summary>
        /// Checks if biometric authentication is available on the device.
        /// </summary>
        /// <returns>True if biometric authentication is available.</returns>
        public bool IsBiometricAvailable()
        {
            NSError error;
            return _context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error);
        }

        /// <summary>
        /// Authenticates the user using biometric credentials.
        /// </summary>
        /// <param name="title">The title for the authentication dialog.</param>
        /// <param name="subtitle">The subtitle for the authentication dialog.</param>
        /// <param name="callback">Callback to handle authentication result.</param>
        public void Authenticate(string title, string subtitle, Action<bool, string> callback)
        {
            var reason = new NSString(subtitle ?? "Please authenticate to continue");

            _context.EvaluatePolicy(
                LAPolicy.DeviceOwnerAuthenticationWithBiometrics,
                reason,
                (success, error) =>
                {
                    if (success)
                    {
                        callback.Invoke(true, "Authentication successful");
                    }
                    else
                    {
                        var errorMessage = error.LocalizedDescription ?? "Authentication failed";
                        callback.Invoke(false, errorMessage);
                    }
                });
        }

        /// <summary>
        /// Gets the type of biometric authentication available.
        /// </summary>
        /// <returns>The biometric type as a string.</returns>
        public string GetBiometricType()
        {
            NSError error;
            if (_context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
            {
                switch (_context.BiometryType)
                {
                    case LABiometryType.TouchId:
                        return "Touch ID";
                    case LABiometryType.FaceId:
                        return "Face ID";
                    default:
                        return "Biometric";
                }
            }
            return "None";
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

        /// <summary>
        /// Gets the type of biometric authentication available.
        /// </summary>
        /// <returns>The biometric type as a string.</returns>
        string GetBiometricType();
    }
}
#endif 