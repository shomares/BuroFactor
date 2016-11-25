using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BuroFactor.code.extension.html
{
    public class ContentType : FilterAttribute, IActionFilter
    {
        private string contentType;
        public ContentType(string ct)
        {
            this.contentType = ct;
        }

        public void OnActionExecuted(ActionExecutedContext context) { /* nada */ }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.ContentType = this.contentType;
        }
    }
}