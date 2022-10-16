using System;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using InterviewTask.Support;
using InterviewTask.Handle;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace InterviewTask.Handle
{
    class ProductComparare : IEqualityComparer<ConsignmentInformation>
    {
        private Func<ConsignmentInformation, object> _funcDistinct;
        public ProductComparare(Func<ConsignmentInformation, object> funcDistinct)
        {
            this._funcDistinct = funcDistinct;
        }

        public bool Equals(ConsignmentInformation x, ConsignmentInformation y)
        {
            return _funcDistinct(x).Equals(_funcDistinct(y));
        }
        public int GetHashCode(ConsignmentInformation obj)
        {
            return this._funcDistinct(obj).GetHashCode();
        }
    }
    public class CallControl
    {
        private IEnumerable<ConsignmentInformation> consignmentInformation;
        private readonly HttpClient httpClient;

        public CallControl(IEnumerable<ConsignmentInformation> consignmentInformation, HttpClient httpClient, string baseUrl)
        {
            this.consignmentInformation = consignmentInformation;
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        internal async Task<string> GetToken(string endpoint)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{httpClient.BaseAddress}{endpoint}");
            string content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(content);
            }
            return content;
        }

        internal async Task<IEnumerable<string>> PostOrder(string token, string endpoint)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var records = consignmentInformation.ToList();
            var apiDtos = new List<API>();
            var orderedItem = new List<OrderedItem>();
            var orderNoChecker = string.Empty;
            
            for (int i = 0; i < records.Count(); i++)
            {
                var purchaseOrderItem = new OrderedItem()
                {
                    stockCode = records[i].ParcelCode,
                    description = records[i].ItemDesciption,
                    orderQty = int.Parse(records[i].ItemQuantity),
                    unitPrice = float.Parse(records[i].ItemValue)
                };
                orderedItem.Add(purchaseOrderItem);
            }

            var responseIds = new List<string>();
            var distinctRecords = consignmentInformation.Distinct(new ProductComparare(a => a.OrderNumber));
            foreach (ConsignmentInformation CI in distinctRecords)
            {
                var groupedOrders = new List<OrderedItem>();
                for (int i = 0; i <= distinctRecords.Count(); i++)
                {
                    if (i == 0)
                    {
                        groupedOrders.Add(orderedItem[i]);
                        continue;
                    }
                    if (records[i].OrderNumber != records[i - 1].OrderNumber)
                        break;
                    else
                        groupedOrders.Add(orderedItem[i]);
                }

                var aPI = new API()
                {
                    orderNumber = CI.OrderNumber,
                    accountRef = CI.ConsignmentNumber,
                    address1 = CI.AddressOne,
                    address2 = CI.AddressTwo,
                    address4 = CI.City,
                    address5 = CI.CountryCode,
                    contactName = CI.ConsigneeName,
                    orderedItems = groupedOrders.ToArray()
                };

                apiDtos.Add(aPI);

                var stringPayload = JsonConvert.SerializeObject(aPI);

                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync($"{httpClient.BaseAddress}{endpoint}", httpContent);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    responseIds.Add(responseContent);
                    Console.WriteLine(responseContent);
                }
            }
            return responseIds;
        }
    }
}
