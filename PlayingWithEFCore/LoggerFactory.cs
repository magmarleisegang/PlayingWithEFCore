using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingWithEFCore
{
   internal static class LoggerFactoryHelper
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
