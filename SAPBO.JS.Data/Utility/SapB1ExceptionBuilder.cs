using SAPBO.JS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Data.Utility
{
    public static class SapB1ExceptionBuilder
    {
        public static Exception BuildException(int errorCode, string errorMessage)
        {
            switch (errorCode)
            {
                case -2035:
                    var ex = new Exception(AppMessages.NameAlreadyExists);
                    ex.HResult = errorCode;
                    return ex;
                default:
                    break;
            }
            var exg = new Exception(errorMessage);
            exg.HResult = errorCode;
            return exg;
        }
    }
}
