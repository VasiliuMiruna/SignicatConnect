using System;
using System.Collections.Generic;
using System.Text;

namespace SignicatIdentification.Models
{
    class AccessTokenModel
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string token_type { get; set; }
    }
}