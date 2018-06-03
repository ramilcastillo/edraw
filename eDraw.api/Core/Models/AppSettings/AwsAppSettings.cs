namespace eDraw.api.Core.Models.AppSettings
{
    public class AwsAppSettings
    {
        public string BucketName { get; set; }
        public string SubFolderInvoices { get; set; }
        public string SubFolderW9 { get; set; }
        public string SubFolderProfile { get; set; }
        public string BucketLocation { get; set; }
        public string PublicDomain { get; set; }
    }
}
