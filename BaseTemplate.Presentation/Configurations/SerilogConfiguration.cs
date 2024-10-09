using Serilog.Sinks.MSSqlServer;

namespace BaseTemplate.Presentation.Configurations
{
    public class SerilogConfiguration
    {
        public static MSSqlServerSinkOptions SinkOptions
        {
            get
            {
                var sinkOpts = new MSSqlServerSinkOptions();
                sinkOpts.TableName = "Logs";
                sinkOpts.AutoCreateSqlTable = true;

                return sinkOpts;
            }
        }
        public static ColumnOptions ColumnOptions
        {
            get
            {
                var columnOpts = new ColumnOptions();
                //columnOpts.Store.Remove(StandardColumn.Properties);
                columnOpts.Store.Add(StandardColumn.LogEvent);
                //columnOpts.LogEvent.DataLength = 2048;
                columnOpts.PrimaryKey = columnOpts.Id;
                columnOpts.TimeStamp.NonClusteredIndex = true;
                columnOpts.AdditionalColumns = new List<SqlColumn>
                {
                    new SqlColumn("UserId",System.Data.SqlDbType.UniqueIdentifier,true),
                    new SqlColumn("UserName",System.Data.SqlDbType.NVarChar,true,150),
                };
                return columnOpts;
            }
        }

    }
}
