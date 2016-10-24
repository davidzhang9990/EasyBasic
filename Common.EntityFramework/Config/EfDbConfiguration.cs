using System.Data.Entity;

namespace Common.EntityFramework.Config
{
    public class EfDbConfiguration : DbConfiguration
    {
        public EfDbConfiguration()
        {
            //AddInterceptor(new LogCommandInterceptor());
            SetDatabaseLogFormatter((context, writeAction) =>
                new DbLogFormatter(context, writeAction));
        }
    }
}
