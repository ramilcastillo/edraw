using System.ComponentModel;

namespace eDraw.api.Services
{
    public static class EdrawEnum
    {
        public enum InvoiceStatus
        {
            [Description("Action Required")]
            ActionRequired,
            [Description("In Progress")]
            InProgress,
            [Description("Approve")]
            Approve,
            [Description("Reject")]
            Reject
        };
        public enum UserRole
        {
            [Description("Bank")]
            Bank,
            [Description("GeneralContractor")]
            GeneralContractor,
            [Description("HomeOwner")]
            HomeOwner,
            [Description("SubContractor")]
            SubContractor
        };
    }
}
