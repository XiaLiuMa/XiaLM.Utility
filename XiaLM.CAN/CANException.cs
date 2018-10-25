using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Can
{
    public class CanException : Exception
    {
        string _errMsg;
        public CanException(string msg)
        {
            _errMsg = msg;
        }

        public override string Message
        {
            get
            {
                return _errMsg;
            }
        }
    }
}
