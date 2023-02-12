using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Models
{
    public class PaymentSettingsModel
    {
        public string API_LOGIN_ID { get; set; }
        public string TRANSACTION_KEY { get; set; }
        public string PaymentURL { get; set; }
        public string PaymentEnvironment { get; set; }
        public string PaymentSuccessURL { get; set; }
        public string PaymentCancelURL { get; set; }
        public string PaymentCommunicator { get; set; }
    }
}
