using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

//using Google.GData.Apps;

//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Gmail.v1;
//using Google.Apis.Gmail.v1.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;

//using HtmlAgilityPack;

//using FluentAssertions;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
//using CLPostLibrary;
//using MongoDB.Driver.Builders;
//using CLPostLibrary;

namespace Facilitate.Libraries.Models
{
    public class Utils
    {
        public static int FemaleFirstNameCount = 4275;
        public static int MaleFirstNameCount = 1219;
        public static int LastNameCount = 79536;
        public static int StreetNameCount = 91670;
        public static int CityNameCount = 387;
        public static int StateNameCount = 50;
        public static int ZipCodeCount = 41365;

        public Utils()
        {

        }

        //public void CreateCities()
        //{
        //    var fileType = "C:\\Joes_Data\\Development\\Personal-Sites\\CLPosts\\PostManagement\\Data\\US-Cities.csv";

        //    try
        //    {
        //        ReadCsv("City", fileType);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public string Propercase()
        //{
        //    var retVal = "";

        //    return retVal;
        //}

        //public void CreateZipCodes()
        //{
        //    var fileType = "C:\\Joes_Data\\Development\\Personal-Sites\\CLPosts\\PostManagement\\Data\\zip_code_database_cleaned.csv";

        //    //try
        //    //{
        //    ReadCsv("ZipCodes", fileType);
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //}
        //}

        //private string[,] ReadCsv(string objectType, string filename)
        //{
        //    CreateDbConnection("State", "States");
        //    CreateDbConnection("City", "Cities");
        //    CreateDbConnection("ZipCode", "ZipCodes");

        //    // Get the file's text.
        //    string whole_file = File.ReadAllText(filename);

        //    // Split into lines.
        //    whole_file = whole_file.Replace('\n', '\r');
        //    string[] lines = whole_file.Split(new char[] { '\r' },
        //        StringSplitOptions.RemoveEmptyEntries);

        //    // See how many rows and columns there are.
        //    int num_rows = lines.Length;
        //    int num_cols = lines[0].Split(',').Length;

        //    // Allocate the data array.
        //    string[,] values = new string[num_rows, num_cols];

        //    // Load the array.
        //    for (int r = 0; r < num_rows; r++)
        //    {
        //        string[] line_r = lines[r].Split(',');
        //        //for (int c = 0; c < num_cols; c++)
        //        for (int c = 0; c < 1; c++)
        //        {
        //            values[r, c] = line_r[c];

        //            switch (objectType.ToLower())
        //            {
        //                case "city":
        //                    var cityName = line_r[0].Trim();
        //                    var stateName = line_r[1].Trim();

        //                    City myCity = new City();
        //                    myCity.Name = cityName;
        //                    myCity.StateName = stateName;
        //                    myCity.StateId = GetStateIdByName(stateName);

        //                    _mongoCityCollection.InsertOne(myCity, null);
        //                    break;

        //                case "zipcodes":

        //                    ZipCode myZipCode = new ZipCode();

        //                    var zipCode = line_r[0].Trim().Replace("\"", "");
        //                    var city = line_r[1].Trim().Replace("\"", "");
        //                    var state = line_r[2].Trim().Replace("\"", "");
        //                    var county = line_r[3].Trim().Replace("\"", "");
        //                    var timezone = line_r[4].Trim().Replace("\"", "");
        //                    var areacodes = line_r[5].Trim().Replace("\"", "");
        //                    var latitude = line_r[6].Trim().Replace("\"", "");
        //                    var longitude = line_r[7].Trim().Replace("\"", "");
        //                    var population = line_r[8].Trim().Replace("\"", "");

        //                    myZipCode.Zip = zipCode;

        //                    State myState = GetStateByAbbr(state);
        //                    if (myState.Name != "")
        //                    {
        //                        myZipCode.StateId = myState._id;
        //                        myZipCode.StateName = myState.Name;

        //                        myZipCode.County = county.Replace("\"", "");
        //                        myZipCode.TimeZone = timezone.Replace("\"", "");
        //                        myZipCode.AreaCodes = areacodes.Replace("\"", "");

        //                        myZipCode.CityName = city.Replace("\"", "");
        //                        myZipCode.CityId = GetCityIdByName(myZipCode.CityName);

        //                        myZipCode.Longitude = latitude.Replace("\"", "");
        //                        myZipCode.Latitude = longitude.Replace("\"", "");

        //                        if (!population.Contains("-") && !population.Contains("."))
        //                            myZipCode.EstimatedPopulation = Convert.ToInt32(population);

        //                        _mongoZipCodeCollection.InsertOne(myZipCode, null);
        //                    }
        //                    break;
        //            }
        //        }
        //    }

        //    // Return the values.
        //    return values;
        //}

        //public void CreateFemaleNames()
        //{
        //    var fileType = "C:\\Temp\\Names\\census-dist-female-first.txt";

        //    CreateDbConnection("Female", "NamesFemale");

        //    try
        //    {
        //        // Read in lines from file.
        //        foreach (string line in File.ReadLines(fileType))
        //        {
        //            Female myName = new Female();

        //            var tmpVal = line.Split(' ');
        //            myName.Name = tmpVal[0].ToLower();

        //            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //            myName.Name = textInfo.ToTitleCase(myName.Name);

        //            _mongoFemaleNameCollection.InsertOne(myName, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        //public void CreateMaleNames()
        //{
        //    var fileType = "C:\\Temp\\Names\\census-dist-male-first.txt";

        //    CreateDbConnection("Male", "NamesMale");

        //    try
        //    {
        //        // Read in lines from file.
        //        foreach (string line in File.ReadLines(fileType))
        //        {
        //            Male myName = new Male();

        //            var tmpVal = line.Split(' ');
        //            myName.Name = tmpVal[0].ToLower();

        //            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //            myName.Name = textInfo.ToTitleCase(myName.Name);

        //            _mongoMaleNameCollection.InsertOne(myName, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        //public void CreateLastNames()
        //{
        //    var fileType = "C:\\Temp\\Names\\census-dist-all-last.txt";

        //    CreateDbConnection("Last", "NamesLast");

        //    try
        //    {
        //        // Read in lines from file.
        //        foreach (string line in File.ReadLines(fileType))
        //        {
        //            Last myName = new Last();

        //            var tmpVal = line.Split(' ');
        //            myName.Name = tmpVal[0].ToLower();

        //            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //            myName.Name = textInfo.ToTitleCase(myName.Name);

        //            _mongoLastNameCollection.InsertOne(myName, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        //public void CreateStreetNames()
        //{
        //    var fileType = "C:\\Temp\\Names\\allstreets.txt";

        //    CreateDbConnection("Street", "NamesStreet");

        //    try
        //    {
        //        // Read in lines from file.
        //        foreach (string line in File.ReadLines(fileType))
        //        {
        //            Street myName = new Street();

        //            myName.Name = line.ToLower();

        //            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //            myName.Name = textInfo.ToTitleCase(myName.Name);

        //            _mongoStreetNameCollection.InsertOne(myName, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        //public void CreateNames(string nameType, string nameGender)
        //{
        //    var fileType = "";

        //    CreateDbConnection("Name", "Names");

        //    try
        //    {
        //        switch (nameType.ToLower())
        //        {
        //            case "first":
        //                if (nameGender.ToLower() == "female")
        //                    fileType = "C:\\Temp\\Names\\census-dist-female-first.txt";
        //                else
        //                    fileType = "C:\\Temp\\Names\\census-dist-male-first.txt";
        //                break;

        //            case "last":
        //                fileType = "C:\\Temp\\Names\\census-dist-all-last.txt";
        //                break;

        //            case "street":
        //                fileType = "C:\\Temp\\Names\\allstreets.txt";
        //                break;
        //        }

        //        // Read in lines from file.
        //        foreach (string line in File.ReadLines(fileType))
        //        {
        //            Name myName = new Name();

        //            switch (nameType.ToLower())
        //            {
        //                case "first":
        //                    var tmpVal = line.Split(' ');

        //                    myName._t = "First";
        //                    myName.Value = tmpVal[0].ToLower();

        //                    if (nameGender.ToLower() == "female")
        //                        myName.Gender = 1; // 4,275 names
        //                    else
        //                        myName.Gender = 2; // 1,219 names
        //                    break;

        //                case "last":
        //                    var tmpVal2 = line.Split(' ');

        //                    myName._t = "Last";
        //                    myName.Value = tmpVal2[0].ToLower();
        //                    myName.Gender = 3;  // 88,799 names
        //                    break;

        //                case "street":
        //                    myName._t = "Street";
        //                    myName.Value = line.ToLower();
        //                    myName.Gender = 4;  // 91,670 names
        //                    break;
        //            }

        //            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //            myName.Value = textInfo.ToTitleCase(myName.Value);

        //            _mongoNameCollection.InsertOne(myName, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        //public void CreatePeople(int personCount, string personGender)
        //{
        //    CreateDbConnection("Person", "Persons");

        //    try
        //    {
        //        // All values should be random and unique
        //        Person newPerson = new Person();

        //        newPerson.Gender = personGender;

        //        newPerson.FirstName = GetRandomFirstName(personGender);
        //        newPerson.MiddleInitial = GetRandomMiddleInitial();
        //        newPerson.LastName = GetRandomLastName();

        //        newPerson.DoB = GetRandomDoB();

        //        ZipCode randomZipCode = GetRandomZipCode();

        //        Address newAddress = new Address();
        //        newAddress.Address1 = GetRandomStreetNumber() + " " + GetRandomStreetName();
        //        newAddress.Address2 = ""; // GetRandomUnitNumber();

        //        newAddress.City = randomZipCode.CityName;
        //        newAddress.State = randomZipCode.StateName;
        //        newAddress.Zip = randomZipCode.Zip;

        //        newPerson.Address = newAddress;

        //        ContactInfo newContactInfo = new ContactInfo();

        //        newContactInfo.Email = newPerson.FirstName.ToLower() + "." + newPerson.LastName.ToLower() + "@gmail.com";

        //        var areaCode = randomZipCode.AreaCodes;
        //        if (areaCode == "")
        //            areaCode = GetRandomAreaCode();

        //        newContactInfo.HomePhone = areaCode + "-" + GetRandomHomePhoneNumber();

        //        newContactInfo.MobilePhone = areaCode + "-" + GetAvailableMobilePhoneNumber();

        //        newPerson.Contact = newContactInfo;

        //        var randomPassword = "!1myPassWord#";

        //        // Add accounts
        //        AccountGmail newAccountGmail = new AccountGmail();
        //        newAccountGmail.UserName = newContactInfo.Email;
        //        newAccountGmail.Password = randomPassword;

        //        newPerson.AccountGmail = newAccountGmail;

        //        AccountCL newClAccount = new AccountCL();
        //        newClAccount.UserName = newContactInfo.Email;
        //        newClAccount.Password = randomPassword;
        //        newPerson.AccountCL = newClAccount;

        //        AccountEBay EbayAccount = new AccountEBay();
        //        EbayAccount.UserName = newContactInfo.Email;
        //        EbayAccount.Password = randomPassword;
        //        newPerson.AccountEBay = EbayAccount;

        //        _mongoPersonCollection.InsertOne(newPerson, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        public void UpdatePerson(Person myPerson)
        {
            try
            {
                CreateDbConnection("Person", "Persons");

                var replaceOneResult = _mongoPersonCollection.ReplaceOneAsync(s => s._id == myPerson._id, myPerson);
            }
            catch (Exception ex)
            {

            }
        }

        public string GetRandomAreaCode()
        {
            var randomAreaCode = "";

            Random rnd = new Random();

            randomAreaCode = rnd.Next(200, 999).ToString();

            return randomAreaCode;
        }

        public string GetRandomHomePhoneNumber()
        {
            var availablePhoneNumber = "";

            Random rnd = new Random();

            int randomExchangePrefix = rnd.Next(200, 999);
            int randomExchangeNumber = rnd.Next(1000, 9999);

            availablePhoneNumber = randomExchangePrefix + "-" + randomExchangeNumber;

            return availablePhoneNumber;
        }

        public string GetAvailableMobilePhoneNumber()
        {
            // Call MessageBroadcast service  API
            var availablePhoneNumber = "555-555-1212";

            Random rnd = new Random();

            int randomExchangePrefix = rnd.Next(200, 999);
            int randomExchangeNumber = rnd.Next(1000, 9999);

            availablePhoneNumber = randomExchangePrefix + "-" + randomExchangeNumber;

            return availablePhoneNumber;
        }

        public string GetRandomGender()
        {
            var randomGender = "Female";

            return randomGender;
        }

        public string GetRandomDoB()
        {
            var randomDoB = "07/01/1970";

            Random rnd = new Random();

            int randomMonth = rnd.Next(1, 12);
            int randomDay = rnd.Next(1, 30);
            int randomYear = rnd.Next(1946, 1998);

            var sMonth = randomMonth.ToString();
            if (sMonth.Length == 1)
                sMonth = "0" + sMonth;

            var sDay = randomDay.ToString();
            if (sDay.Length == 1)
                sDay = "0" + sDay;

            var sYear = randomYear.ToString();

            randomDoB = sMonth + "/" + sDay + "/" + sYear;

            return randomDoB;
        }

        public string GetRandomFirstName(string sGender)
        {
            var randomFirstName = "";

            Random rnd = new Random();
            int randomRecordNumber = 0;  // rnd.Next(1, 13); 

            if (sGender == "Female") // Female
                randomRecordNumber = rnd.Next(1, FemaleFirstNameCount);
            else
                randomRecordNumber = rnd.Next(1, MaleFirstNameCount);

            //List<Name> myRandomName = new List<Name>();

            try
            {
                switch (sGender)
                {
                    case "Female":
                        CreateDbConnection("Female", "NamesFemale");

                        List<Female> myRandomName = _mongoFemaleNameCollection.Find(s => s._t == sGender).Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                        foreach (Female currentFemaleName in myRandomName)
                        {
                            randomFirstName = currentFemaleName.Name;
                        }
                        break;

                    case "Male":
                        CreateDbConnection("Male", "NamesMale");

                        List<Male> myRandomMaleName = _mongoMaleNameCollection.Find(s => s._t == sGender).Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                        foreach (Male currentName in myRandomMaleName)
                        {
                            randomFirstName = currentName.Name;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                randomFirstName = ex.ToString();
            }

            return randomFirstName;
        }

        public string GetRandomMiddleInitial()
        {
            var randomMiddleInitial = "";

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Random rnd = new Random();

            int index = rnd.Next(chars.Length);
            randomMiddleInitial = chars[index].ToString();

            return randomMiddleInitial;
        }

        public string GetRandomLastName()
        {
            var randomLastName = "";

            Random rnd = new Random();
            int randomRecordNumber = rnd.Next(1, LastNameCount);

            List<Last> myRandomName = new List<Last>();

            try
            {
                CreateDbConnection("Last", "NamesLast");

                // Query random on iGender
                myRandomName = _mongoLastNameCollection.Find(s => s._t == "Last").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (Last currentName in myRandomName)
                {
                    randomLastName = currentName.Name;
                }
            }
            catch (Exception ex)
            {
                randomLastName = ex.ToString();
            }

            return randomLastName;
        }

        public string GetRandomStreetNumber()
        {
            var randomStreetNumber = "";

            Random rnd = new Random();
            int randomRecordNumber = rnd.Next(1, 999);
            randomStreetNumber = randomRecordNumber.ToString();

            return randomStreetNumber;
        }

        public string GetRandomUnitNumber()
        {
            var randomUnitNumber = "Apt #A-1";

            return randomUnitNumber;
        }

        public string GetRandomStreetName()
        {
            var randomStreetName = "";

            Random rnd = new Random();
            int randomRecordNumber = rnd.Next(1, StreetNameCount);

            List<Street> myRandomName = new List<Street>();

            try
            {
                CreateDbConnection("Street", "NamesStreet");

                // Query random on iGender
                myRandomName = _mongoStreetNameCollection.Find(s => s._t == "Street").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (Street currentName in myRandomName)
                {
                    randomStreetName = currentName.Name;
                }
            }
            catch (Exception ex)
            {
                randomStreetName = ex.ToString();
            }

            return randomStreetName;
        }

        public string GetRandomCity(ObjectId stateId)
        {
            var randomCity = "";

            try
            {
                CreateDbConnection("City", "StatesAndCities");

                // This needs to be queried by state
                CityNameCount = _mongoCityCollection.Find(s => s.StateId == stateId).ToListAsync().Result.Count();

                Random rnd = new Random();
                int randomRecordNumber = rnd.Next(1, CityNameCount);

                List<City> myRandomName = new List<City>();

                // Query random on iGender
                myRandomName = _mongoCityCollection.Find(s => s.StateId == stateId).Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (City currentName in myRandomName)
                {
                    randomCity = currentName.Name;
                }
            }
            catch (Exception ex)
            {
                randomCity = ex.ToString();
            }

            return randomCity;
        }

        public string[] GetRandomState()
        {
            string[] randomStateInfo = new string[] { "", "" };

            Random rnd = new Random();
            int randomRecordNumber = rnd.Next(1, StateNameCount);

            List<State> myRandomName = new List<State>();

            try
            {
                CreateDbConnection("State", "States");

                // Query random on iGender
                myRandomName = _mongoStateCollection.Find(s => s._t == "State").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (State currentName in myRandomName)
                {
                    randomStateInfo[0] = currentName.Name;
                    randomStateInfo[1] = currentName._id.ToString();
                }
            }
            catch (Exception ex)
            {
                randomStateInfo[0] = "Exception";
                randomStateInfo[1] = ex.ToString();
            }

            return randomStateInfo;
        }

        public ZipCode GetRandomZipCode()
        {
            ZipCode randomZipCode = new ZipCode();

            try
            {
                CreateDbConnection("ZipCode", "ZipCodes");

                Random rnd = new Random();
                int randomRecordNumber = rnd.Next(1, ZipCodeCount);

                List<ZipCode> myRandomZipCodes = new List<ZipCode>();

                myRandomZipCodes = _mongoZipCodeCollection.Find(s => s._t == "ZipCode").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (ZipCode currentZipCode in myRandomZipCodes)
                {
                    randomZipCode = currentZipCode;
                }
            }
            catch (Exception ex)
            {
                randomZipCode = null;
            }

            return randomZipCode;
        }

        public List<State> GetStates()
        {
            var stateList = new List<State>();

            try
            {
                CreateDbConnection("State", "States");

                var stateCollection = _mongoStateCollection.Find(s => s._t == "State").ToListAsync().Result;
                foreach (State currentState in stateCollection)
                {
                    var _state = new State();
                    _state._id = currentState._id;
                    _state.Name = currentState.Name;
                    //_state.Abbr = currentState.Abbr;

                    stateList.Add(_state);
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return stateList;
        }

        //public ObjectId GetStateIdByName(string stateName)
        //{
        //    var stateId = ObjectId.Empty;

        //    try
        //    {
        //        CreateDbConnection("State", "States");

        //        var stateCollection = _mongoStateCollection.Find(s => s.Name == stateName).ToListAsync().Result;
        //        foreach (State currentState in stateCollection)
        //        {
        //            stateId = currentState._id;
        //            return stateId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }

        //    return stateId;
        //}

        //public ObjectId GetCityIdByName(string cityName)
        //{
        //    ObjectId myCityId = ObjectId.Empty;

        //    try
        //    {
        //        CreateDbConnection("City", "Cities");

        //        var cityCollection = _mongoCityCollection.Find(s => s.Name == cityName).ToListAsync().Result;
        //        foreach (City currentCity in cityCollection)
        //        {
        //            myCityId = currentCity._id;
        //            return myCityId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }

        //    return myCityId;
        //}

        //public State GetStateByAbbr(string stateAbbr)
        //{
        //    State myState = new State();

        //    try
        //    {
        //        CreateDbConnection("State", "States");

        //        var stateCollection = _mongoStateCollection.Find(s => s.Abbr == stateAbbr).ToListAsync().Result;
        //        foreach (State currentState in stateCollection)
        //        {
        //            myState = currentState;
        //            return myState;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }

        //    return myState;
        //}

        //public string GetStateAbbr(ObjectId stateId)
        //{
        //    var stateAbbr = "";

        //    try
        //    {
        //        CreateDbConnection("State", "States");

        //        var stateCollection = _mongoStateCollection.Find(s => s._id == stateId).ToListAsync().Result;
        //        foreach (State currentState in stateCollection)
        //        {
        //            stateAbbr = currentState.Abbr;
        //            return stateAbbr;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }

        //    return stateAbbr;
        //}

        public string SaveEvent(Event myEvent)
        {
            CreateDbConnection("Event", "Events");

            try
            {
                _mongoEventCollection.InsertOne(myEvent, null);
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return serviceResponse;
        }

        private string serviceResponse = "Successful";

        //public void CreateEvents()
        //{
        //    CreateDbConnection("Event", "TypeDefinitions");

        //    try
        //    {
        //        // Read in lines from file.
        //        foreach (string line in File.ReadLines("C:\\Joes_Data\\Development\\Personal-Sites\\CLPosts\\PostManagement\\Data\\Events.txt"))
        //        {
        //            var tmpVal = line.Split('|');

        //            Event myEvent = new Event();
        //            myEvent.TypeId = Convert.ToInt16(tmpVal[1]);
        //            myEvent.TypeName = tmpVal[0];
        //            myEvent.Details = "";
        //            myEvent.Server = "";

        //            _mongoEventCollection.InsertOne(myEvent, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errMsg = ex.ToString();
        //    }
        //}

        //public void CreateMarkets()
        //{
        //    CreateDbConnection("State", "States");

        //    CreateDbConnection("Market", "Markets");

        //    List<HtmlNode> listResults = new List<HtmlNode>();

        //    string URI = "http://www.craigslist.org/about/sites";

        //    var itemCount = 0;

        //    using (WebClient wc = new WebClient())
        //    {
        //        try
        //        {
        //            HtmlWeb w = new HtmlWeb();
        //            HtmlDocument responseDoc = w.Load(URI);
        //            foreach (HtmlNode node in responseDoc.DocumentNode.SelectNodes("//div[@class='colmask']"))
        //            {
        //                if (itemCount == 0)
        //                {
        //                    foreach (var childItem in node.ChildNodes)
        //                    {
        //                        // This is an item we need to parse
        //                        if (childItem.Name.ToLower() == "div")
        //                        {
        //                            listResults.Add(childItem);
        //                        }
        //                    }
        //                    itemCount++;
        //                }
        //            }

        //            // Parse the list
        //            foreach (var currentItem in listResults)
        //            {
        //                var currentStateName = "";
        //                var currentStateId = ObjectId.Empty;

        //                foreach (HtmlNode node in currentItem.ChildNodes)
        //                {
        //                    switch (node.Name.ToLower())
        //                    {
        //                        case "h4": // This is the State, get StateId
        //                            currentStateName = node.InnerText;

        //                            var stateCollection = _mongoStateCollection.Find(s => s.Name == currentStateName).ToListAsync().Result;
        //                            foreach (State currentState in stateCollection)
        //                            {
        //                                currentStateId = currentState._id;
        //                            }
        //                            break;

        //                        case "ul": // City list
        //                            foreach (var listItem in node.ChildNodes)
        //                            {
        //                                switch (listItem.Name.ToLower())
        //                                {
        //                                    case "li":
        //                                        // Current city
        //                                        var currCity = listItem.InnerHtml;

        //                                        // Parse city name and url
        //                                        var tmpCityInfo = currCity.Split('>');

        //                                        //<a href="http://auburn.craigslist.org"
        //                                        var tmpVal2 = tmpCityInfo[0].Split('"');

        //                                        var tmpCityUrl = tmpVal2[1].Replace("\"", "");
        //                                        var tmpCityName = tmpCityInfo[1].Replace("</a", "");

        //                                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //                                        tmpCityName = textInfo.ToTitleCase(tmpCityName);

        //                                        Market marketItem = new Market();
        //                                        marketItem.StateId = currentStateId;
        //                                        marketItem.Name = tmpCityName;
        //                                        marketItem.Url = tmpCityUrl.Replace("//", "http://");

        //                                        // Save
        //                                        _mongoMarketCollection.InsertOne(marketItem, null);
        //                                        break;
        //                                }
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var errMsg = ex.ToString();
        //        }
        //    }
        //}

        public List<City> GetCities(ObjectId stateId)
        {
            var cityList = new List<City>();

            CreateDbConnection("City", "StatesAndCities");

            var sort = Builders<BsonDocument>.Sort.Ascending("Name");

            var cityCollection = _mongoCityCollection.Find(s => s.StateId == stateId).ToListAsync().Result;

            foreach (City currentCity in cityCollection)
            {
                var _city = new City();
                _city.Name = currentCity.Name;
                //_city.Url = currentCity.Url;

                cityList.Add(_city);
            }

            return cityList;
        }

        public List<Market> GetMarkets(ObjectId stateId)
        {
            var marketList = new List<Market>();

            CreateDbConnection("Market", "Markets");

            var sort = Builders<BsonDocument>.Sort.Ascending("Name");

            var marketCollection = _mongoMarketCollection.Find(s => s.StateId == stateId).ToListAsync().Result;

            foreach (Market currentMarket in marketCollection)
            {
                var _market = new Market();
                _market.Name = currentMarket.Name;
                _market.Url = currentMarket.Url;

                marketList.Add(_market);
            }

            return marketList;
        }

        //public string SaveAdvertisement(Advertisement newAdvertisement)
        //{
        //    try
        //    {
        //        CreateDbConnection("Advertisement", "Advertisements");

        //        // Save
        //        _mongoAdvertisementCollection.InsertOne(newAdvertisement, null);

        //        Event myEvent = new Event();

        //        myEvent.TypeId = Constants.Event.Advertisement.Created.Item1;
        //        myEvent.TypeName = Constants.Event.Advertisement.Created.Item2;
        //        myEvent.ReferenceId = newAdvertisement._id;
        //        myEvent.Details = newAdvertisement.AdTitle; // Constants.Event.Advertisement.Created.Item3;
        //        myEvent.Server = "";

        //        myEvent.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return serviceResponse;
        //}

        //public Advertisement GetAdvertisement(ObjectId adertisementId)
        //{
        //    var myAdList = new List<Advertisement>();
        //    var myAd = new Advertisement();

        //    try
        //    {
        //        CreateDbConnection("Advertisement", "Advertisements");

        //        myAdList = _mongoAdvertisementCollection.Find(s => s._id == adertisementId).ToListAsync().Result;
        //        foreach (var currentAd in myAdList)
        //        {
        //            myAd = currentAd;
        //            //return currentAd;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return myAd;
        //}

        //public List<Advertisement> GetSampleAdvertisements()
        //{
        //    var myAdList = new List<Advertisement>();
        //    var myAd = new Advertisement();

        //    try
        //    {
        //        CreateDbConnection("Advertisement", "TypeDefinitions");

        //        myAdList = _mongoAdvertisementCollection.Find(s => s._t == "Advertisement").ToListAsync().Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return myAdList;
        //}

        //public Advertisement GetSampleAdvertisementData(ObjectId adId)
        //{
        //    var myAdList = new List<Advertisement>();
        //    var myAd = new Advertisement();

        //    try
        //    {
        //        CreateDbConnection("Advertisement", "TypeDefinitions");

        //        myAdList = _mongoAdvertisementCollection.Find(s => s._id == adId).ToListAsync().Result;
        //        foreach (var currAd in myAdList)
        //        {
        //            myAd = currAd;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return myAd;
        //}

        //public string DeleteAdvertisement(ObjectId adertisementId)
        //{
        //    var serviceResponse = "";

        //    var myAdList = new List<Advertisement>();
        //    var myAd = new Advertisement();

        //    try
        //    {
        //        CreateDbConnection("Advertisement", "Advertisements");

        //        myAdList = _mongoAdvertisementCollection.Find(s => s._id == adertisementId).ToListAsync().Result;
        //        foreach (var currentAd in myAdList)
        //        {
        //            myAd = currentAd;

        //            currentAd.IsDeleted = true;
        //            _mongoAdvertisementCollection.ReplaceOne(s => s._id == adertisementId, currentAd);
        //        }
        //        serviceResponse = "Successful";
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return serviceResponse;
        //}

        //public string GetAdCount()
        //{
        //    var countResponse = "";

        //    List<Advertisement> myAds = new List<Advertisement>();

        //    short adCount = 0;

        //    try
        //    {
        //        CreateDbConnection("Advertisement", "Advertisements");

        //        var sort = Builders<Advertisement>.Sort.Descending("DateCreated");

        //        myAds = _mongoAdvertisementCollection.Find(s => s._t == "Advertisement").ToListAsync().Result;
        //        adCount = Convert.ToInt16(myAds.Count);

        //        foreach (var currAd in myAds)
        //        {
        //            foreach (var currState in currAd.ItemStates)
        //            {
        //                adCount += Convert.ToInt16(currState.Markets.Count);
        //            }

        //        }

        //        countResponse = myAds.Count + "|" + adCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return countResponse;
        //}

        public short GetEventCount(int eventType)
        {
            List<Event> myEvents = new List<Event>();

            short eventCount = 0;

            try
            {
                CreateDbConnection("Event", "Events");

                var sort = Builders<Advertisement>.Sort.Descending("DateCreated");

                //myEvents = _mongoEventCollection.Find<Event>(s => s._t == "Event").ToListAsync<Event>().Result;

                if (eventType > 0)
                {
                    myEvents = _mongoEventCollection.Find(s => s.TypeId == eventType).ToListAsync().Result;
                }
                else
                {
                    myEvents = _mongoEventCollection.Find(s => s._t == "Event").ToListAsync().Result;
                }

                eventCount = Convert.ToInt16(myEvents.Count);
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return eventCount;
        }

        //public Event GetEvent(ObjectId eventId)
        //{
        //    var myEventList = new List<Event>();
        //    var myEvent = new Event();

        //    try
        //    {
        //        CreateDbConnection("Event", "Events");

        //        myEventList = _mongoEventCollection.Find(s => s._id == eventId).ToListAsync().Result;
        //        foreach (var currentAd in myEventList)
        //        {
        //            myEvent = currentAd;
        //            //return currentAd;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return myEvent;
        //}

        //public string GetRandomText(int sentanceCount)
        //{
        //    var randomText = "";
        //    int[] randomSenLen = { 1, 3, 5, 10 };

        //    RandomText mtText = new RandomText();
        //    randomText = mtText.Paragraph(sentanceCount, randomSenLen);
        //    randomText += Environment.NewLine;

        //    return randomText;
        //}

        //public List<Advertisement> GetAllAdvertisements(int recLimit, int pageNumber)
        //{
        //    List<Advertisement> myAds = new List<Advertisement>();

        //    try
        //    {
        //        CreateDbConnection("Advertisement", "Advertisements");

        //        var sort = Builders<Advertisement>.Sort.Descending("DateCreated");

        //        myAds = _mongoAdvertisementCollection.Find(s => s._t == "Advertisement").Limit(recLimit).Skip(pageNumber).Sort(sort).ToListAsync().Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse = ex.ToString();
        //    }

        //    return myAds;
        //}

        public List<Person> GetAllUsers(int recLimit, int pageNumber, string sortBy, string orderBy)
        {
            List<Person> myUsers = new List<Person>();

            try
            {
                CreateDbConnection("Person", "Persons");

                switch (orderBy)
                {
                    case "Ascending":
                        var sortAsc = Builders<Person>.Sort.Ascending(sortBy);

                        myUsers = _mongoPersonCollection.Find(s => s._t == "Person").Limit(recLimit).Skip(pageNumber).Sort(sortAsc).ToListAsync().Result;
                        break;

                    case "Descending":
                        var sortDesc = Builders<Person>.Sort.Descending(sortBy);

                        myUsers = _mongoPersonCollection.Find(s => s._t == "Person").Limit(recLimit).Skip(pageNumber).Sort(sortDesc).ToListAsync().Result;
                        break;
                }
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return myUsers;
        }

        public List<Stats> GetStats()
        {
            List<Stats> myStats = new List<Stats>();

            try
            {
                CreateDbConnection("Stats", "Stats");

                myStats = _mongoStatsCollection.Find(s => s._t == "Stats").ToListAsync().Result;
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return myStats;
        }

        public void CreateStats()
        {
            Stats myStats = new Stats();

            try
            {
                CreateDbConnection("Stats", "Stats");

                _mongoStatsCollection.InsertOne(myStats, null);
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }
        }

        public void UpdateStats(string statType, long statValue)
        {
            string serviceResponse = "";

            try
            {
                CreateDbConnection("Stats", "Stats");

                List<Stats> myStats = _mongoStatsCollection.Find(s => s._t == "Stats").ToListAsync().Result;

                switch (statType.ToLower())
                {
                    case "users":
                        myStats[0].Users += statValue;
                        break;

                    case "marketsassigned":
                        myStats[0].MarketsAssigned += statValue;
                        break;

                    case "marketsunassigned":
                        myStats[0].MarketsUnAssigned -= statValue;
                        break;

                    case "gmailregistered":
                        myStats[0].GmailRegistered += statValue;
                        break;

                    case "gmailunregistered":
                        myStats[0].GmailUnRegistered -= statValue;
                        break;

                    case "craigslistregistered":
                        myStats[0].CraigslistRegistered += statValue;
                        break;

                    case "craigslistunregistered":
                        myStats[0].CraigslistUnRegistered -= statValue;
                        break;
                }

                var replaceOneResult = _mongoStatsCollection.ReplaceOneAsync(s => s._t == "Stats", myStats[0]);
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }
        }

        //public string RegisterGmailUserAccount(string userName, string firstName, string lastName, string password)
        //{
        //    var registrationResponse = "";

        //    Service appService = new AppsService("clposts.com", "clposts81@gmail.com", "13324BossWood");

        //    appService.ApplicationName = "CLPosts";
        //    //appService.Domain = "clposts.com";

        //    try
        //    {
        //        //var a = appService.CreateUser(UserName, GivenName, FamilyName, Password);
        //        var a = appService.CreateUser(userName, firstName, lastName, password);
        //    }
        //    catch (AppsException ex)
        //    {
        //        var errMsg = ex.Reason.ToString() + " " + ex.ErrorCode.ToString();
        //    }

        //    try
        //    {
        //        string[] Scopes = { GmailService.Scope.GmailReadonly };
        //        string ApplicationName = "CLPosts";

        //        UserCredential credential;

        //        var credentialFilePath = AppDomain.CurrentDomain.BaseDirectory + "PostManagement\\" + "client_secret.json";

        //        //using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
        //        using (var stream = new FileStream(credentialFilePath, FileMode.Open, FileAccess.Read))
        //        {
        //            string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //            credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart");

        //            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //                GoogleClientSecrets.Load(stream).Secrets,
        //                Scopes,
        //                "user",
        //                CancellationToken.None,
        //                new FileDataStore(credPath, true)).Result;

        //            var resp1 = "Credential file saved to: " + credPath;

        //        }

        //        // Create Gmail API service.
        //        var service = new GmailService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = ApplicationName,
        //        });

        //        // Define parameters of request.
        //        UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

        //        // List labels.
        //        IList<Label> labels = request.Execute().Labels;
        //        //Console.WriteLine("Labels:");

        //        var resp2 = "";

        //        if (labels != null && labels.Count > 0)
        //        {
        //            foreach (var labelItem in labels)
        //            {
        //                resp2 = "{0}" + labelItem.Name;
        //            }
        //        }
        //        else
        //        {
        //            resp2 = "No labels found.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        registrationResponse = ex.ToString();
        //    }

        //    return registrationResponse;
        //}

        public List<Person> GetUsersWithFilter(string queryType)
        {
            List<Person> myUsers = new List<Person>();

            CreateDbConnection("Person", "Persons");

            try
            {
                switch (queryType.ToLower())
                {
                    case "marketassigned":
                        myUsers = _mongoPersonCollection.Find(s => s.Markets[0].Name != "").ToListAsync().Result;
                        break;

                    case "marketnotassigned":
                        myUsers = _mongoPersonCollection.Find(s => s.Markets[0].Name == "").ToListAsync().Result;
                        //myUsers = _mongoPersonCollection.Find<Person>(s => s._t == "Person").ToListAsync<Person>().Result;
                        break;

                    case "craigslistregistered":
                        myUsers = _mongoPersonCollection.Find(s => s.AccountCL.IsRegistered == true).ToListAsync().Result;
                        break;

                    case "craigslistnotregistered":
                        myUsers = _mongoPersonCollection.Find(s => s.AccountCL.IsRegistered == false).ToListAsync().Result;
                        break;

                    case "gmailregistered":
                        myUsers = _mongoPersonCollection.Find(s => s.AccountGmail.IsRegistered == true).ToListAsync().Result;
                        break;

                    case "gmailnotregistered":
                        myUsers = _mongoPersonCollection.Find(s => s.AccountGmail.IsRegistered == false).ToListAsync().Result;
                        break;
                }
            }
            catch (Exception ex)
            {
                var errMsg = "";
            }

            return myUsers;
        }

        public long GetUserCount(string queryType)
        {
            long userCount = 0;

            try
            {
                CreateDbConnection("Person", "Persons");

                switch (queryType.ToLower())
                {
                    case "allregistered":
                        userCount = _mongoPersonCollection.Count(s => s._t == "Person", null);
                        break;

                    case "craigslistregistered":
                        userCount = _mongoPersonCollection.Count(s => s.AccountCL.IsRegistered == true, null);
                        break;

                    case "craigslistnotregistered":
                        userCount = _mongoPersonCollection.Count(s => s.AccountCL.IsRegistered == false, null);
                        break;

                    case "gmailregistered":
                        userCount = _mongoPersonCollection.Count(s => s.AccountGmail.IsRegistered == true, null);
                        break;

                    case "gmailnotregistered":
                        userCount = _mongoPersonCollection.Count(s => s.AccountGmail.IsRegistered == false, null);
                        break;

                    case "marketassigned":
                        userCount = _mongoPersonCollection.Count(s => s.Markets[0].Name != "", null);
                        break;

                    case "marketnotassigned":
                        userCount = _mongoPersonCollection.Count(s => s.Markets[0].Name == "", null);
                        break;

                    default:
                        userCount = _mongoPersonCollection.Count(s => s._t == "Person", null);
                        break;
                }
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return userCount;
        }

        public List<Event> GetAllEvents(int recLimit, int pageNumber, int eventType)
        {
            List<Event> myEvents = new List<Event>();

            try
            {
                CreateDbConnection("Event", "Events");

                var sort = Builders<Event>.Sort.Descending("DateCreated");

                if (eventType > 0)
                {
                    myEvents = _mongoEventCollection.Find(s => s.TypeId == eventType).Limit(recLimit).Skip(pageNumber).Sort(sort).ToListAsync().Result;
                }
                else
                {
                    myEvents = _mongoEventCollection.Find(s => s._t == "Event").Limit(recLimit).Skip(pageNumber).Sort(sort).ToListAsync().Result;
                }
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return myEvents;
        }

        public List<Event> GetEventTypes()
        {
            List<Event> myEventTypes = new List<Event>();

            try
            {
                CreateDbConnection("Event", "TypeDefinitions");

                var sort = Builders<Event>.Sort.Ascending("TypeName");

                myEventTypes = _mongoEventCollection.Find(s => s._t == "Event").Sort(sort).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return myEventTypes;
        }

        public string GetCityCount()
        {
            var cityCount = 0;

            CreateDbConnection("City", "StatesAndCities");

            var sort = Builders<BsonDocument>.Sort.Ascending("Name");

            var cityCollection = _mongoCityCollection.Find(s => s._t == "City").ToListAsync().Result;

            foreach (City currentCity in cityCollection)
            {
                cityCount++;
            }

            return cityCount.ToString();
        }

        public string GetMarketCount()
        {
            var marketCount = 0;

            CreateDbConnection("Market", "Markets");

            var sort = Builders<BsonDocument>.Sort.Ascending("Name");

            var marketCollection = _mongoMarketCollection.Find(s => s._t == "Market").ToListAsync().Result;

            foreach (Market currentMarket in marketCollection)
            {
                marketCount++;
            }

            return marketCount.ToString();
        }


        public string GeoLocationByUserIp(string userIp)
        {
            var stateName = "Unknown";
            var cityName = "Unknown";
            var zipCode = "Unknown";

            try
            {
                var request = "http://freegeoip.net/xml/" + userIp;
                var webRequest = WebRequest.Create(request);
                webRequest.Method = "GET";

                var res = webRequest.GetResponse();
                var response = res.GetResponseStream();
                var xmlDoc = new XmlDocument();
                if (response != null)
                {
                    xmlDoc.Load(response);

                    stateName = xmlDoc.ChildNodes[1].ChildNodes[4].InnerText;
                    cityName = xmlDoc.ChildNodes[1].ChildNodes[5].InnerText;
                    zipCode = xmlDoc.ChildNodes[1].ChildNodes[6].InnerText;
                }

                return cityName + ", " + stateName + " " + zipCode;
            }
            catch (Exception ex)
            {
                return "Error: GeoLocationByUserIp(" + userIp + ") " + ex.Message;
            }
        }

        #region String functions

        public bool IsValueObjectId(string queryValue)
        {
            try
            {
                ObjectId.Parse(queryValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool HasLowerCase(string evalString)
        {
            return !string.IsNullOrEmpty(evalString) && Regex.IsMatch(evalString, "[a-z]");
        }

        public bool HasUpperCase(string evalString)
        {
            return !string.IsNullOrEmpty(evalString) && Regex.IsMatch(evalString, "[A-Z]");
        }

        public bool HasNumeric(string evalString)
        {
            return !string.IsNullOrEmpty(evalString) && Regex.IsMatch(evalString, "[0-9]");
        }

        public string FormatNumber(string formatValue)
        {
            int inputNumber = Convert.ToInt32(formatValue);
            var formattedNumber = string.Format("{0:##,####,####}", inputNumber);

            if (formattedNumber == "")
                formattedNumber = "0";

            return formattedNumber;
        }

        public string SanitizeString(string response)
        {
            var cleanedString = response;
            cleanedString = cleanedString.Replace(" & ", " and ");
            cleanedString = cleanedString.Replace("'", "&apos;");

            return cleanedString;
        }

        public string SanitizeXmlString(string response)
        {
            var cleanedString = response;
            cleanedString = cleanedString.Replace("&", "&amp;").Replace(Environment.NewLine, "");
            return cleanedString;
        }

        public bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        #endregion

        public bool SendGenericEmail(string ownerId, string ownerType, string fromAddress, string toAddress, string subject, string body, bool isBodyHtml)
        {
            if (fromAddress == "")
                fromAddress = ""; // ConfigurationManager.AppSettings[cfg.FromAddress];

            const string emailTemplate = ""; // GetEmailTemplate(ownerId, ownerType);

            try
            {
                //Set up message
                var message = new MailMessage { From = new MailAddress(fromAddress) };
                message.To.Add(new MailAddress(toAddress));
                message.Subject = subject;
                message.IsBodyHtml = isBodyHtml;

                var messageBody = emailTemplate; // sbEmailTemplate.Replace("[DocTitle]", Subject);
                messageBody = messageBody.Replace("[DocTitle]", subject);
                messageBody = messageBody.Replace("[Date]", DateTime.UtcNow.ToLocalTime().ToString(CultureInfo.InvariantCulture));
                messageBody = messageBody.Replace("[MessageBody]", body.Replace("|", isBodyHtml ? "<br />" : Environment.NewLine));

                message.Body = messageBody;

                message.Priority = MailPriority.High;

                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                // setup Smtp Client
                //var smtp = new SmtpClient
                //{
                //    Port = Convert.ToInt16(ConfigurationManager.AppSettings[cfg.Port]),
                //    Host = ConfigurationManager.AppSettings[cfg.Host],
                //    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings[cfg.EnableSsl]),
                //    UseDefaultCredentials =
                //        Convert.ToBoolean(ConfigurationManager.AppSettings[cfg.UseDefaultCredentials]),
                //    Credentials =
                //        new NetworkCredential(ConfigurationManager.AppSettings[cfg.LoginUserName],
                //            ConfigurationManager.AppSettings[cfg.LoginPassword]),
                //    DeliveryMethod = SmtpDeliveryMethod.Network
                //};

                //smtp.Send(message);

                return true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {

            }
            return false;
        }

        #region Cookie methods

        //public void CreateCookie(string cookieName, string cookieValue, DateTime cookieExpires)
        //{
        //    var myCookie = new HttpCookie(cookieName) { Value = cookieValue, Expires = cookieExpires };

        //    HttpContext.Current.Response.Cookies.Add(myCookie);
        //}

        //public string ReadCookie(string cookieName)
        //{
        //    var myCookie = HttpContext.Current.Request.Cookies[cookieName];
        //    Debug.Assert(condition: myCookie != null, message: "myCookie != null");
        //    return myCookie.Value;
        //}

        //public void UpdateCookie()
        //{
        //}

        //public void DeleteCookie()
        //{
        //}

        #endregion

        #region MongoDB Methods

        IMongoDatabase _mongoDatabase;
        IMongoClient _mongoClient;
        IMongoCollection<State> _mongoStateCollection;
        IMongoCollection<City> _mongoCityCollection;
        IMongoCollection<ZipCode> _mongoZipCodeCollection;
        IMongoCollection<Market> _mongoMarketCollection;
        IMongoCollection<Advertisement> _mongoAdvertisementCollection;
        IMongoCollection<Event> _mongoEventCollection;
        IMongoCollection<Name> _mongoNameCollection;
        IMongoCollection<Female> _mongoFemaleNameCollection;
        IMongoCollection<Male> _mongoMaleNameCollection;
        IMongoCollection<Last> _mongoLastNameCollection;
        IMongoCollection<Street> _mongoStreetNameCollection;
        IMongoCollection<Person> _mongoPersonCollection;
        IMongoCollection<Stats> _mongoStatsCollection;

        private void CreateDbConnection(string objectType, string collectionName)
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString;

            _mongoClient = new MongoClient(dbConnectionString);

            _mongoDatabase = _mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDbName"]); // this database uses the new API

            switch (objectType.ToLower())
            {
                case "advertisement":
                    _mongoAdvertisementCollection = _mongoDatabase.GetCollection<Advertisement>(collectionName);
                    break;

                case "city":
                    _mongoCityCollection = _mongoDatabase.GetCollection<City>(collectionName);
                    break;

                case "event":
                    _mongoEventCollection = _mongoDatabase.GetCollection<Event>(collectionName);
                    break;

                case "market":
                    _mongoMarketCollection = _mongoDatabase.GetCollection<Market>(collectionName);
                    break;

                case "name":
                    _mongoNameCollection = _mongoDatabase.GetCollection<Name>(collectionName);
                    break;

                case "female":
                    _mongoFemaleNameCollection = _mongoDatabase.GetCollection<Female>(collectionName);
                    break;

                case "male":
                    _mongoMaleNameCollection = _mongoDatabase.GetCollection<Male>(collectionName);
                    break;

                case "last":
                    _mongoLastNameCollection = _mongoDatabase.GetCollection<Last>(collectionName);
                    break;

                case "street":
                    _mongoStreetNameCollection = _mongoDatabase.GetCollection<Street>(collectionName);
                    break;

                case "person":
                    _mongoPersonCollection = _mongoDatabase.GetCollection<Person>(collectionName);
                    break;

                case "state":
                    _mongoStateCollection = _mongoDatabase.GetCollection<State>(collectionName);
                    break;

                case "stats":
                    _mongoStatsCollection = _mongoDatabase.GetCollection<Stats>(collectionName);
                    break;

                case "zipcode":
                    _mongoZipCodeCollection = _mongoDatabase.GetCollection<ZipCode>(collectionName);
                    break;

                default:
                    _mongoStateCollection = _mongoDatabase.GetCollection<State>(collectionName);
                    break;
            }
        }

        public interface IIdentified
        {
            ObjectId _id { get; }
        }

        #endregion
    }
}