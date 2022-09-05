using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.ENUMs.UserENUMs
{
    public enum ForgotPasswordSendSMSEnum
    {
        Sent,
        Error,
        NotAllowed,
        UnconfirmedPhone,
        OneMiniuteError
    }
}
