﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.Inerfaces
{
    interface IWebConnection
    {
        void SetURL(string url);
        void SetHeaderEncoding(string encoding);
        WebResponse GetResponse();
    }
}
