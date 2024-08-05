using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

using MongoDB.Bson;
using MongoDB.Driver;
using static Facilitate.Libraries.Models.Constants.Messaging;

namespace Facilitate.Libraries.Models
{
    public class Utils
    {
        IMongoDatabase _mongoDatabase;
        IMongoClient _mongoClient;

        string _mongoDBName = "Facilitate";
        string _mongoDBCollectionName = "ReferenceData";

        IMongoCollection<State> _mongoReferenceDataCollection;

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

        public static int FemaleFirstNameCount = 4275;
        public static int MaleFirstNameCount = 1219;
        public static int LastNameCount = 79536;
        public static int StreetNameCount = 91670;
        public static int CityNameCount = 387;
        public static int StateNameCount = 50;
        public static int ZipCodeCount = 41365;

        string resultMsg = string.Empty;

        //string _mongoDBConnectionString = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";
        string _mongoDBConnectionString = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";

        public TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;

        public Utils()
        {
            _mongoClient = new MongoClient(_mongoDBConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase("Facilitate");
        }

        public DateTime FormateDateTimeToLocal(DateTime dt)
        {
            return dt.ToLocalTime();
        }

        public string TitleCaseString(string inputString)
        {
            return textinfo.ToTitleCase(inputString.ToLower());
        }

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

            try
            {
                switch (sGender.ToLower())
                {
                    case "female":
                        var femaleNames = _mongoClient.GetDatabase(_mongoDBName).GetCollection<NameFemale>(_mongoDBCollectionName);
                        var myRandomFemaleName = femaleNames.Find(s => s._t == "NameFemale").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                        foreach (var currentFemaleName in myRandomFemaleName)
                        {
                            return currentFemaleName.Name;
                        }
                        break;

                    case "male":
                        var maleNames = _mongoClient.GetDatabase(_mongoDBName).GetCollection<NameMale>(_mongoDBCollectionName);
                        var myRandomMaleName = maleNames.Find(s => s._t == "NameMale").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                        foreach (var currentMaleName in myRandomMaleName)
                        {
                            return currentMaleName.Name;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return null;
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

            try
            {
                var lastNames = _mongoClient.GetDatabase(_mongoDBName).GetCollection<NameMale>(_mongoDBCollectionName);
                var myRandomLastName = lastNames.Find(s => s._t == "NameLast").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (var currentLastName in myRandomLastName)
                {
                    return currentLastName.Name;
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
            int randomRecordNumber = rnd.Next(100, 99999);
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
            Random rnd = new Random();
            int randomRecordNumber = rnd.Next(1, StreetNameCount);

            try
            {
                var streetNames = _mongoClient.GetDatabase(_mongoDBName).GetCollection<NameStreet>(_mongoDBCollectionName);
                var randomStreetName = streetNames.Find(s => s._t == "NameStreet").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (var currentStreetName in randomStreetName)
                {
                    return currentStreetName.Name;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return null;
        }

        public string[] GetRandomCity(ObjectId stateId)
        {
            string[] randomCityInfo = new string[] { "", "", "", "" };

            List<City> myCities = new List<City>();

            try
            {
                var cityCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<City>(_mongoDBCollectionName);
                var randomCities = cityCollection.Find(s => s._t == "City").ToListAsync().Result;

                foreach (City randomCity in randomCities)
                {
                    if(randomCity.StateId == stateId)
                    {
                        myCities.Add(randomCity);
                    }
                }
            }
            catch (Exception ex)
            {
                randomCityInfo[0] = ex.ToString();
            }

            Random rnd = new Random();
            int i = 0;
            int randomRecordNumber = rnd.Next(myCities.Count);

            foreach (City randomCity in myCities)
            {
                if(i == randomRecordNumber)
                {
                    randomCityInfo[0] = randomCity._id.ToString();
                    randomCityInfo[1] = randomCity.Name;
                    randomCityInfo[2] = randomCity.CountyId.ToString();
                    randomCityInfo[3] = randomCity.TimeZoneId.ToString();

                    return randomCityInfo;
                }
                i++;
            }   

            return null;
        }

        public string[] GetRandomState()
        {
            string[] randomStateInfo = new string[] { "", "", "" };

            Random rnd = new Random();
            int randomRecordNumber = rnd.Next(1, StateNameCount);

            try
            {
                var stateNames = _mongoClient.GetDatabase(_mongoDBName).GetCollection<State>(_mongoDBCollectionName);
                var randomStateName = stateNames.Find(s => s._t == "State").Limit(-1).Skip(randomRecordNumber).ToListAsync().Result;
                foreach (var currentState in randomStateName)
                {
                    randomStateInfo[0] = currentState.Name;
                    randomStateInfo[1] = currentState.Abbr;
                    randomStateInfo[2] = currentState._id.ToString();

                    return randomStateInfo;
                }
            }
            catch (Exception ex)
            {
                randomStateInfo[0] = "Exception";
                randomStateInfo[1] = ex.ToString();
            }

            return randomStateInfo;
        }

        public ZipCode GetRandomZipCode(ObjectId cityId)
        {
            List<ZipCode> randomZipCodes = null;
            ZipCode randomZipcode = null;

            try 
            {
                //var zipCodeCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<ZipCode>("ReferenceData");
                //var selectedZipCodes = zipCodeCollection.Find(s => s.CityId == cityId).ToListAsync().Result;

                var zipCodeCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<ZipCode>(_mongoDBCollectionName);
                randomZipCodes = zipCodeCollection.Find(s => s._t == "ZipCode").ToListAsync().Result;

                foreach (ZipCode zipCode in randomZipCodes)
                {
                    if (zipCode.CityId == cityId)
                    {
                        return zipCode;
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return null;
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

        public ZipCode GetGeoLocationInfoByZipCode(int zipCode)
        {
            try
            {
                var zipCodeCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<ZipCode>("ReferenceData");
                var selectedZipCodes = zipCodeCollection.Find(s => s.Zip == zipCode).ToListAsync().Result;

                foreach (ZipCode currentZipCode in selectedZipCodes)
                {
                    return currentZipCode;
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return null;
        }

        public ObjectId GetStateIdByName(string stateName)
        {
            stateName = textinfo.ToTitleCase(stateName.ToLower());

            var stateId = ObjectId.Empty;

            try
            {
                var stateCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<State>("ReferenceData");
                var selectedState = stateCollection.Find(s => s.Name == stateName).ToListAsync().Result;

                foreach (State currentState in selectedState)
                {
                    if(currentState._t == "State")
                    {
                        stateId = currentState._id;
                        return stateId;
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return stateId;
        }

        public ObjectId GetStateByAbbr(string stateAbbr)
        {
            stateAbbr = stateAbbr.ToUpper();

            var stateId = ObjectId.Empty;

            try
            {
                var stateCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<State>("ReferenceData");
                var selectedState = stateCollection.Find(s => s.Abbr == stateAbbr).ToListAsync().Result;

                foreach (State currentState in selectedState)
                {
                    if (currentState._t == "State")
                    {
                        stateId = currentState._id;
                        return stateId;
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return stateId;
        }

        public ObjectId GetCityIdByName(string cityName)
        {
            ObjectId myCityId = ObjectId.Empty;

            try
            {
                CreateDbConnection("City", "Cities");

                var cityCollection = _mongoCityCollection.Find(s => s.Name == cityName).ToListAsync().Result;
                foreach (City currentCity in cityCollection)
                {
                    myCityId = currentCity._id;
                    return myCityId;
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return myCityId;
        }

        public ObjectId GetCityIdByNameAndStateId(string cityName, ObjectId StateId)
        {
            cityName = textinfo.ToTitleCase(cityName.ToLower());

            var cityId = ObjectId.Empty;

            try
            {
                var cityCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<City>("ReferenceData");
                var selectedCities = cityCollection.Find(s => s.Name == cityName).ToListAsync().Result;

                foreach (City currentCity in selectedCities)
                {
                    if (currentCity.StateId == StateId)
                    {
                        if (currentCity._t == "City")
                        {
                            cityId = currentCity._id;
                            return cityId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return cityId;
        }

        public City GetCityInfoByNameAndStateId(string cityName, ObjectId StateId)
        {
            cityName = textinfo.ToTitleCase(cityName.ToLower());

            var cityId = ObjectId.Empty;

            try
            {
                var cityCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<City>("ReferenceData");
                var selectedCities = cityCollection.Find(s => s.Name == cityName).ToListAsync().Result;

                foreach (City currentCity in selectedCities)
                {
                    if (currentCity.StateId == StateId)
                    {
                        if (currentCity._t == "City")
                        {
                            return currentCity;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return null;
        }

        public County GetCountyByCityTimeZoneId(ObjectId timezoneId)
        {
            timezoneId = ObjectId.Parse("57a01b573d33e01394bc8afe");

            try
            {
                var countyCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<County>("ReferenceData");
                var selectedCounties = countyCollection.Find(s => s.TimeZoneId == timezoneId).ToListAsync().Result;

                foreach (County currentCounty in selectedCounties)
                {
                    if (currentCounty._t == "County")
                    {
                        return currentCounty;
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return null;
        }

        public string GetStateAbbr(ObjectId stateId)
        {
            var stateAbbr = "";

            try
            {
                CreateDbConnection("State", "States");

                var stateCollection = _mongoStateCollection.Find(s => s._id == stateId).ToListAsync().Result;
                foreach (State currentState in stateCollection)
                {
                    stateAbbr = currentState.Abbr;
                    return stateAbbr;
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return stateAbbr;
        }

        public string GetStateAbbrByName(string stateName)
        {
            var stateAbbr = "";

            try
            {
                CreateDbConnection("State", "States");

                var stateCollection = _mongoStateCollection.Find(s => s.Name == stateName).ToListAsync().Result;
                foreach (State currentState in stateCollection)
                {
                    stateAbbr = currentState.Abbr;
                    return stateAbbr;
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            //return stateAbbr;

            stateName = textinfo.ToTitleCase(stateName.ToLower());

            try
            {
                //var randomCities = cityCollection.Find(s => s._t == "City").ToListAsync().Result;

                var stateCollection = _mongoClient.GetDatabase(_mongoDBName).GetCollection<State>("ReferenceData");
                var selectedState = stateCollection.Find(s => s._t == "State").ToListAsync().Result;

                foreach (State currentState in selectedState)
                {
                    if (currentState.Name == stateName)
                    {
                        stateAbbr = currentState.Abbr;
                        return stateAbbr;
                    }
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.ToString();
            }

            return stateAbbr;
        }

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

        public Event GetEvent(ObjectId eventId)
        {
            var myEventList = new List<Event>();
            var myEvent = new Event();

            try
            {
                CreateDbConnection("Event", "Events");

                myEventList = _mongoEventCollection.Find(s => s._id == eventId.ToString()).ToListAsync().Result;
                foreach (var currentAd in myEventList)
                {
                    myEvent = currentAd;
                    //return currentAd;
                }
            }
            catch (Exception ex)
            {
                serviceResponse = ex.ToString();
            }

            return myEvent;
        }

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

        public bool SendEmail(string ownerId, string ownerType, string fromAddress, string toAddress, string subject, string body, bool isBodyHtml)
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

        #region MongoDB Methods

        private void CreateDbConnection(string objectType, string _collectionName)
        {
            switch (objectType.ToLower())
            {
                case "advertisement":
                    _mongoAdvertisementCollection = _mongoDatabase.GetCollection<Advertisement>(_collectionName);
                    break;

                case "city":
                    _mongoCityCollection = _mongoDatabase.GetCollection<City>(_collectionName);
                    break;

                case "event":
                    _mongoEventCollection = _mongoDatabase.GetCollection<Event>(_collectionName);
                    break;

                case "market":
                    _mongoMarketCollection = _mongoDatabase.GetCollection<Market>(_collectionName);
                    break;

                case "name":
                    _mongoNameCollection = _mongoDatabase.GetCollection<Name>(_collectionName);
                    break;

                case "female":
                    _mongoFemaleNameCollection = _mongoDatabase.GetCollection<Female>(_collectionName);
                    break;

                case "male":
                    _mongoMaleNameCollection = _mongoDatabase.GetCollection<Male>(_collectionName);
                    break;

                case "last":
                    _mongoLastNameCollection = _mongoDatabase.GetCollection<Last>(_collectionName);
                    break;

                case "street":
                    _mongoStreetNameCollection = _mongoDatabase.GetCollection<Street>(_collectionName);
                    break;

                case "person":
                    _mongoPersonCollection = _mongoDatabase.GetCollection<Person>(_collectionName);
                    break;

                case "state":
                    _mongoStateCollection = _mongoDatabase.GetCollection<State>(_collectionName);
                    break;

                case "stats":
                    _mongoStatsCollection = _mongoDatabase.GetCollection<Stats>(_collectionName);
                    break;

                case "zipcode":
                    _mongoZipCodeCollection = _mongoDatabase.GetCollection<ZipCode>(_collectionName);
                    break;

                default:
                    _mongoStateCollection = _mongoDatabase.GetCollection<State>(_collectionName);
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