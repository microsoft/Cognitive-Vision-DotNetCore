using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ProjectOxford.Vision
{
    internal class UrlBasedRequest
    {
        public string url { get; set; }

        public UrlBasedRequest(string url)
        {
            this.url = url;
        }
    }
}
