using Serilog.Core;
using Serilog.Events;
using System;

namespace ConsoleApp1
{
    public class LogCodeEnricher : ILogEventEnricher
    {
       public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {            
            LogEventProperty logCode = propertyFactory.CreateProperty("logcode", "1");
            logEvent.AddPropertyIfAbsent(logCode);
        }
    }
}
