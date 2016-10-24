using System;
using System.Data;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Common.EntityFramework.DataAccess;

namespace Common.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TransactionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            var unitOfWork = (IUnitOfWork)httpActionContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWork));
            unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public override void OnActionExecuted(HttpActionExecutedContext httpActionExecutedContext)
        {
            var unitOfWork = (IUnitOfWork)httpActionExecutedContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWork));

            try
            {
                if (!unitOfWork.IsInTransaction)
                {
                    return;
                }

                if (httpActionExecutedContext.Exception != null)
                {
                    unitOfWork.RollBackTransaction();
                }
                else
                {
                    unitOfWork.CommitTransaction();
                }
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
    }
}
