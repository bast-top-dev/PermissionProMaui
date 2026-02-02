namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for SEPA (Single Euro Payments Area) payment information
    /// </summary>
    public class SepaModel
    {
        /// <summary>
        /// Total amount of the payment
        /// </summary>
        public string TotalAmount { get; set; }

        /// <summary>
        /// Currency of the payment
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Purpose code for the payment
        /// </summary>
        public string PurposeCode { get; set; }

        /// <summary>
        /// Flag indicating if purpose code is present
        /// </summary>
        public bool HasPruposeCode { get; set; }

        /// <summary>
        /// Creditor's IBAN
        /// </summary>
        public string CreditorIban { get; set; }

        /// <summary>
        /// Creditor's BIC
        /// </summary>
        public string CreditorBic { get; set; }

        /// <summary>
        /// Creditor's name
        /// </summary>
        public string CreditorName { get; set; }

        /// <summary>
        /// Debitor's IBAN
        /// </summary>
        public string DebitorIban { get; set; }

        /// <summary>
        /// Debitor's BIC
        /// </summary>
        public string DebitorBic { get; set; }

        /// <summary>
        /// Debitor's name
        /// </summary>
        public string DebitorName { get; set; }

        /// <summary>
        /// Remittance information
        /// </summary>
        public string[] RmtInf { get; set; }

        /// <summary>
        /// Account header text
        /// </summary>
        public string AccountHeaderText { get; set; }

        /// <summary>
        /// Requested execution date
        /// </summary>
        public string RequestedExecutionDate { get; set; }

        /// <summary>
        /// ESR reference number
        /// </summary>
        public string EsrRefNumber { get; set; }

        /// <summary>
        /// Control sum
        /// </summary>
        public string CrtlSum { get; set; }
    }
} 