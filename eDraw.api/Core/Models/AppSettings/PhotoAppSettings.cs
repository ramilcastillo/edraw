using System.IO;
using System.Linq;


namespace eDraw.api.Core.Models.AppSettings
{
    public class PhotoAppSettings
    {
        public int MaxBytes { get; set; }

        public string[] AcceptedFileTypes { get; set; }

        public bool IsSupported(string fileName)
        {
            var result = AcceptedFileTypes
                .Any(s => s == Path.GetExtension(fileName).ToLower());
            return result;
        }
    }
}
