using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = "User ID=Serilog;Password=Serilog;Host=localhost;Port=5432;Database=Logs";

            string tableName = "logs";

            //Used columns (Key is a column name) 
            //Column type is writer's constructor parameter
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter() },
                {"message_template", new MessageTemplateColumnWriter() },
                {"logcode", new SinglePropertyColumnWriter("logcode", PropertyWriteMethod.Raw, NpgsqlDbType.Integer) },
                {"level", new LevelColumnWriter() },
                {"timestamp", new TimestampColumnWriter() },
                {"exception", new ExceptionColumnWriter() },
                {"log_event", new LogEventSerializedColumnWriter() }
            };

            var logger = new LoggerConfiguration()
                                .MinimumLevel.Verbose()                                
                                .Enrich.FromLogContext()
                                //.Enrich.With<LogCodeEnricher>()
                                .WriteTo.Console()
                                .WriteTo.PostgreSQL(connectionstring,
                                                    tableName,
                                                    columnWriters,
                                                    LogEventLevel.Verbose,
                                                    null,
                                                    null,
                                                    30,
                                                    null,
                                                    false,
                                                    "public",
                                                    true)
                                .CreateLogger();

            logger.Information("{logcode} the message", 123);

            // Flush and Dispose the logger.
            ((IDisposable)logger).Dispose();            
        }
    }
}