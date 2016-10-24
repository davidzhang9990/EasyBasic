using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Threading.Tasks;
using log4net;

namespace Common.EntityFramework.Config
{
    public class DbLogFormatter : DatabaseLogFormatter
    {
        public static ILog Logger { get; set; }
        public DbLogFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        {
        }

        public override void LogCommand<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {

        }

        public override void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {

        }

        public override void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {

        }

        public override void Closing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {

        }

        public override void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {

        }

        public override void LogResult<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (interceptionContext.Exception == null)
            {
                //var parasBuilder = new System.Text.StringBuilder();
                //for (var i = 0; i < command.Parameters.Count; i++)
                //{
                //    parasBuilder.AppendFormat("'{0}'='{1}'({2});", command.Parameters[i].ParameterName,
                //        command.Parameters[i].Value, command.Parameters[i].DbType);
                //}

                //Logger.Debug(string.Format(
                //    "Executed command: {0}, Parementers: {1} Time:[{2}ms]",
                //    command.CommandText.Replace(Environment.NewLine, ""),
                //    parasBuilder,
                //    Stopwatch.ElapsedMilliseconds));
                var commandText = command.CommandText.Replace(Environment.NewLine, "");
                for (var i = 0; i < command.Parameters.Count; i++)
                {
                    commandText = commandText.Replace("@" + command.Parameters[i].ParameterName, "'" + command.Parameters[i].Value + "'");
                }
                Logger.Debug(string.Format(
                    "Executed command: {0}, Time:[{1}ms]",
                    commandText,
                    Stopwatch.ElapsedMilliseconds));
            }
            else if (!interceptionContext.TaskStatus.HasFlag(TaskStatus.Canceled))
            {
                var result = interceptionContext.Result;
                Logger.Error(string.Format("Executed command failure:{0}", (object)result == null ? "null" : ((object)result is DbDataReader ? result.GetType().Name : result.ToString())));
            }

        }
    }
}
