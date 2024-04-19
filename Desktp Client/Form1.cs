using Facilitate.Desktop.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Client
{
    public partial class Form1 : Form
    {
        static HttpClient client = new HttpClient();

        public List<Quote> quoteList = new List<Quote>();

        public string apiResponse;

        //public string apiUrl = "http://localhost:8080/api/Quote";
        public string apiUrl = "https://api.facilitate.org/api/Quote";

        public Form1()
        {
            InitializeComponent();

            GetQuoteList();
        }

        private async void GetQuoteList()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Add("RequestType", "GetQuotes");

                    using (HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "?status=new"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            quoteList = JsonConvert.DeserializeObject<List<Quote>>(apiResponse);

                            var tmpVal = "";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create lead
            PostQuote();
            var tmpVal = "";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            GetQuoteList();
        }

        public async Task<string> PostQuote()
        {
            QuoteRoofleSubmission quoteRoofleSubmission = new QuoteRoofleSubmission();
            
            string returnUri = "";
            //apiUrl += "?status=new";

            try
            {
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(apiUrl)
                };

                HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrl, quoteRoofleSubmission);

                response.EnsureSuccessStatusCode();

                returnUri = response.Headers.Location.ToString();

                apiResponse = "Success";
            }
            catch (Exception ex)
            {
                apiResponse = ex.Message;
            }

            return apiResponse;
        }

        //public static byte[] SerializeToUtf8Bytes(object? value, System.Text.Json.Serialization.Metadata.JsonTypeInfo jsonTypeInfo);
    }
}
