﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create lead

        }
    }
}
