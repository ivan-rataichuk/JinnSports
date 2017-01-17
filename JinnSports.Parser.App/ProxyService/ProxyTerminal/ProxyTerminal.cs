﻿using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JinnSports.Parser.App.ProxyService.ProxyConnections;
using JinnSports.Parser.App.Exceptions;
using System.Diagnostics;
using System.Threading;

namespace JinnSports.Parser.App.ProxyService.ProxyTerminal
{
    public class ProxyTerminal : IProxyTerminal
    {
        IProxyAsync proxyAsyncCommand;
        private ProxyConnection pc;

        public ProxyTerminal()
        {
            this.pc = new ProxyConnection();
        }

        public HttpWebResponse GetProxyResponse(Uri uri)
        {
            proxyAsyncCommand = new ProxyAsync(this.pc, uri);
            return proxyAsyncCommand.GetProxyAsync();
        }

    }
}
