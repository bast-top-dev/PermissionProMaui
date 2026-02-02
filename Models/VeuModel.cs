using System;
using System.Collections.Generic;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for VEÜ (Verification of Electronic Usage) information
    /// </summary>
    public class VeuModel
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ContactId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Digest information for verification
        /// </summary>
        public string DigestInfo { get; set; }

        /// <summary>
        /// Type of order
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Account BIC
        /// </summary>
        public string AccountBic { get; set; }

        /// <summary>
        /// Total number of orders
        /// </summary>
        public string TotalOrders { get; set; }

        /// <summary>
        /// Amount of the transaction
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Currency of the transaction
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Permission level for the transaction
        /// </summary>
        public string PermissionLevel { get; set; }

        /// <summary>
        /// VEÜ partner identifier
        /// </summary>
        public string VeuPartnerId { get; set; }

        /// <summary>
        /// Name of the originator
        /// </summary>
        public string OriginatorName { get; set; }

        /// <summary>
        /// Timestamp of the originator
        /// </summary>
        public string OriginatorTimestamp { get; set; }

        /// <summary>
        /// Bank code format
        /// </summary>
        public string BankCodeFormat { get; set; }

        /// <summary>
        /// Flag indicating if account number is international
        /// </summary>
        public bool IsAccountNumberInternational { get; set; }

        /// <summary>
        /// Flag indicating if bank code is international
        /// </summary>
        public bool IsBankCodeInternational { get; set; }

        /// <summary>
        /// Flag indicating if already signed by user ID
        /// </summary>
        public bool IsAlreadySignedByUserId { get; set; }

        /// <summary>
        /// Name of the service
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Service option
        /// </summary>
        public string ServiceOption { get; set; }

        /// <summary>
        /// Scope of the transaction
        /// </summary>
        public string Scope { get; set; }
        
        /// <summary>
        /// Name of the message
        /// </summary>
        public string MessageName { get; set; }
        
        /// <summary>
        /// Version of the message
        /// </summary>
        public int MessageVersion { get; set; }
        
        /// <summary>
        /// List of SEPA models
        /// </summary>
        public List<SepaModel> SepaModels { get; set; }
    }
} 