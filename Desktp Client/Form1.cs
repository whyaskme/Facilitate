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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Client
{
    public partial class Form1 : Form
    {

        //public List<Quote> quoteList = new List<Quote>();

        public string apiResponse;
        //public string apiUrl = "http://localhost:8080/api/Quote?status=new";
        public string apiUrl = "https://api.facilitate.org/api/Quote?status=new";

        public Form1()
        {
            InitializeComponent();

            GetQuoteList();
        }

        private async void GetQuoteList()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(apiUrl))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var fileJsonString = await response.Content.ReadAsStringAsync();

                            var tmp2 = "";
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
            var tmpVal = "";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            GetQuoteList();
        }

        public string PostQuote()
        {
            QuoteRoofleSubmission quoteRoofleSubmission = new QuoteRoofleSubmission();

            string responseFromServer = "";

            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = quoteRoofleSubmission.ToString().Length;

            using (var stream = request.GetRequestStream())
            {
                //stream.Write(quoteRoofleSubmission.ToString().ToUtf8Bytes(), 0, quoteRoofleSubmission.ToString().Length);
                //stream.Write(quoteRoofleSubmission.ToString(), 0, quoteRoofleSubmission.ToString().Length);
            }

            WebResponse response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseFromServer = reader.ReadToEnd();
            }

            return responseFromServer;
        }

        //public static byte[] SerializeToUtf8Bytes(object? value, System.Text.Json.Serialization.Metadata.JsonTypeInfo jsonTypeInfo);
    }
}
