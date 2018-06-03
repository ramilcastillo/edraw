using System.Collections.Generic;

namespace eDraw.api.Core.Models
{
    public class EmailProperties
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string EmailHeading { get; set; }

        public bool HasButton { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public List<string> ReceipentsEmail { get; set; }
    }
}
