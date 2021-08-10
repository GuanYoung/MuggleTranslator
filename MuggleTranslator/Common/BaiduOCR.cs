using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MuggleTranslator.Common
{
    public class BaiduOCR
    {
        private string _clientId;                // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private string _clientSecret;            // 百度云中开通对应服务应用的 Secret Key

        public BaiduOCR(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public string GeneralBasic(string imageBase64String)
        {
            var response = RequestServerToOCR(imageBase64String, "general_basic");
            var sb = new StringBuilder();
            var words_result = response["words_result"] as JArray;
            if (words_result != null)
            {
                foreach (var word in words_result)
                    sb.Append(word["words"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string AccurateBasic(string fileName)
        {
            var response = RequestServerToOCR(fileName, "accurate_basic");
            var sb = new StringBuilder();
            var words_result = response["words_result"] as JArray;
            if (words_result != null)
            {
                foreach (var word in words_result)
                {
                    sb.Append(word["words"].ToString() + "\r\n");
                }
            }
            return sb.ToString();
        }

        public string VatInvoice(string fileName)
        {
            var response = RequestServerToOCR(fileName, "vat_invoice");
            var sb = new StringBuilder();
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("发票种类：" + words_result["InvoiceType"].ToString() + "\r\n");
                sb.Append("发票名称：" + words_result["InvoiceTypeOrg"].ToString() + "\r\n");
                sb.Append("发票代码：" + words_result["InvoiceCode"].ToString() + "\r\n");
                sb.Append("发票号码：" + words_result["InvoiceNum"].ToString() + "\r\n");
                sb.Append("校验码：" + words_result["CheckCode"].ToString() + "\r\n");

                sb.Append("开票日期：" + words_result["InvoiceDate"].ToString() + "\r\n");
                sb.Append("购方名称：" + words_result["PurchaserName"].ToString() + "\r\n");
                sb.Append("购方纳税人识别号：" + words_result["PurchaserRegisterNum"].ToString() + "\r\n");
                sb.Append("购方地址及电话：" + words_result["PurchaserAddress"].ToString() + "\r\n");
                sb.Append("购方开户行及账号：" + words_result["PurchaserBank"].ToString() + "\r\n");

                sb.Append("密码区：" + words_result["Password"].ToString() + "\r\n");
                sb.Append("销售方名称：" + words_result["SellerName"].ToString() + "\r\n");
                sb.Append("销售方纳税人识别号：" + words_result["SellerRegisterNum"].ToString() + "\r\n");
                sb.Append("销售方地址及电话：" + words_result["SellerAddress"].ToString() + "\r\n");
                sb.Append("销售方开户行及账号：" + words_result["SellerBank"].ToString() + "\r\n");

                sb.Append("合计金额：" + words_result["TotalAmount"].ToString() + "\r\n");
                sb.Append("合计税额：" + words_result["TotalTax"].ToString() + "\r\n");
                sb.Append("价税合计（大写）：" + words_result["AmountInWords"].ToString() + "\r\n");
                sb.Append("价税合计（小写）：" + words_result["AmountInFiguers"].ToString() + "\r\n");
                sb.Append("备注：" + words_result["Remarks"].ToString() + "\r\n");

                sb.Append("收款人：" + words_result["Payee"].ToString() + "\r\n");
                sb.Append("复核：" + words_result["Checker"].ToString() + "\r\n");
                sb.Append("开票人：" + words_result["NoteDrawer"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string QuotaInvoice(string fileName)
        {
            var response = RequestServerToOCR(fileName, "quota_invoice");
            var sb = new StringBuilder();
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("发票代码：" + words_result["invoice_code"].ToString() + "\r\n");
                sb.Append("发票号码：" + words_result["invoice_number"].ToString() + "\r\n");
                sb.Append("金额：" + words_result["invoice_rate"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string Idcard(string fileName)
        {
            var response = RequestServerToOCR(fileName, "idcard", param: "id_card_side=front");
            var sb = new StringBuilder();
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("姓名：" + words_result["姓名"]["words"].ToString() + "\r\n");
                sb.Append("性别：" + words_result["性别"]["words"].ToString() + "\r\n");
                sb.Append("民族：" + words_result["民族"]["words"].ToString() + "\r\n");
                sb.Append("出生：" + words_result["出生"]["words"].ToString() + "\r\n");
                sb.Append("住址：" + words_result["住址"]["words"].ToString() + "\r\n");
                sb.Append("身份号码：" + words_result["公民身份号码"]["words"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string Bankcard(string fileName)
        {
            var response = RequestServerToOCR(fileName, "bankcard");
            var sb = new StringBuilder();
            var words_result = response["result"];
            if (words_result != null)
            {
                sb.Append("银行卡卡号：" + words_result["bank_card_number"].ToString() + "\r\n");
                sb.Append("银行名：" + words_result["bank_name"].ToString() + "\r\n");
                var bank_card_type = Convert.ToInt32(words_result["bank_card_type"].ToString());
                var type = "不能识别";
                if (bank_card_type == 1)
                    type = "借记卡";
                else if (bank_card_type == 2)
                    type = "贷记卡";
                else if (bank_card_type == 3)
                    type = "准贷记卡";
                else if (bank_card_type == 4)
                    type = "预付费卡";
                sb.Append("银行卡类型：" + type + "\r\n");
                sb.Append("有效期：" + words_result["valid_date"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string LicensePlate(string fileName)
        {
            var response = RequestServerToOCR(fileName, "license_plate");
            var sb = new StringBuilder();
            //var words_result = response["data"]["words_result"];
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("车牌号码：" + words_result["number"].ToString() + "\r\n");
                sb.Append("车牌颜色：" + words_result["color"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string DrivingLicense(string fileName)
        {
            var response = RequestServerToOCR(fileName, "driving_license");
            var sb = new StringBuilder();
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("证号：" + words_result["证号"]["words"].ToString() + "\r\n");
                sb.Append("姓名：" + words_result["姓名"]["words"].ToString() + "\r\n");
                sb.Append("性别：" + words_result["性别"]["words"].ToString() + "\r\n");
                sb.Append("国籍：" + words_result["国籍"]["words"].ToString() + "\r\n");
                sb.Append("住址：" + words_result["住址"]["words"].ToString() + "\r\n");

                sb.Append("出生日期：" + words_result["出生日期"]["words"].ToString() + "\r\n");
                sb.Append("初次领证日期：" + words_result["初次领证日期"]["words"].ToString() + "\r\n");
                sb.Append("准驾车型：" + words_result["准驾车型"]["words"].ToString() + "\r\n");
                sb.Append("有效期限：" + words_result["有效期限"]["words"].ToString() + "\r\n");
                sb.Append("至：" + words_result["至"]["words"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string TrainTicket(string fileName)
        {
            var response = RequestServerToOCR(fileName, "train_ticket");
            var sb = new StringBuilder();
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("车票号：" + words_result["ticket_num"].ToString() + "\r\n");
                sb.Append("车次号：" + words_result["ticket_num"].ToString() + "\r\n");

                sb.Append("始发站：" + words_result["starting_station"].ToString() + "\r\n");
                sb.Append("到达站：" + words_result["destination_station"].ToString() + "\r\n");
                sb.Append("出发日期：" + words_result["date"].ToString() + "\r\n");
                sb.Append("时间：" + words_result["time"].ToString() + "\r\n");
                sb.Append("座位号：" + words_result["train_num"].ToString() + "\r\n");

                sb.Append("车票金额：" + words_result["ticket_rates"].ToString() + "\r\n");
                sb.Append("席别：" + words_result["seat_category"].ToString() + "\r\n");
                sb.Append("乘客姓名：" + words_result["name"].ToString() + "\r\n");
                sb.Append("身份证号：" + words_result["id_num"].ToString() + "\r\n");

                sb.Append("序列号：" + words_result["serial_number"].ToString() + "\r\n");
                sb.Append("售站：" + words_result["sales_station"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public string TaxiReceipt(string fileName)
        {
            var response = RequestServerToOCR(fileName, "taxi_receipt");
            var sb = new StringBuilder();
            var words_result = response["words_result"];
            if (words_result != null)
            {
                sb.Append("发票代号：" + words_result["InvoiceCode"].ToString() + "\r\n");
                sb.Append("发票号码：" + words_result["InvoiceNum"].ToString() + "\r\n");
                sb.Append("车牌号：" + words_result["TaxiNum"].ToString() + "\r\n");
                sb.Append("日期：" + words_result["Date"].ToString() + "\r\n");
                sb.Append("上下车时间：" + words_result["Time"].ToString() + "\r\n");

                sb.Append("总金额：" + words_result["Fare"].ToString() + "\r\n");
                sb.Append("燃油附加费：" + words_result["FuelOilSurcharge"].ToString() + "\r\n");
                sb.Append("叫车服务费：" + words_result["CallServiceSurcharge"].ToString() + "\r\n");
                sb.Append("省：" + words_result["Province"].ToString() + "\r\n");
                sb.Append("市：" + words_result["City"].ToString() + "\r\n");

                sb.Append("单价：" + words_result["PricePerkm"].ToString() + "\r\n");
                sb.Append("里程：" + words_result["Distance"].ToString() + "\r\n");
            }
            return sb.ToString();
        }

        public bool Test()
        {
            return true;
        }

        #region 内部方法
        private JObject RequestServerToOCR(string imageBase64, string action, string param = "")
        {
            string token = getAccessToken();
            string host = $"https://aip.baidubce.com/rest/2.0/ocr/v1/{action}?access_token=" + token;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            request.Proxy = null;

            // 图片的base64编码
            String str = "image=" + HttpUtility.UrlEncode(imageBase64) + "&" + param;
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            return JObject.Parse(result);
        }

        private String getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", _clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", _clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(result)["access_token"].ToString();
        }
        #endregion
    }
}
