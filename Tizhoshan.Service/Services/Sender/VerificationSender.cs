using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tizhoshan.DataLayer.Context;

namespace Tizhoshan.ServiceLayer.Services.Sender
{


    public interface iVerificationSender
    {
        bool Send(string toNumber, string userName, string verificationCode);
    }

    public class VerificationSender : iVerificationSender
    {
        private readonly ApplicationDbContext _context;
        public VerificationSender(ApplicationDbContext cnt)
        {
            _context = cnt;
        }

        public bool Send(string toNumber, string userName, string verificationCode)
        {



            if (toNumber != null)
            {
                //https://farazsms.com/

                string user = userName.Replace(" ", "_");
                string apiKey = "v0XM89Z6bx_xls61OT0tscu6-6Om_02x2OEdoI4J7zk=";
                string patternid = "75r5fo7l7u";
                string fromNumber = "0983000505";

                string url = $"http://ippanel.com:8080/?apikey={apiKey}&pid={patternid}&fnum={fromNumber}&tnum={toNumber}&p1=name&p2=verification-code&v1={user}&v2={verificationCode}";


                HttpClient httpClient = new HttpClient();
                var httpResponse = httpClient.GetAsync(url);
                if (httpResponse.IsCompleted == true)
                {
                    return true;
                }
            }
            return false;
        }

    
    }
}
