using Microsoft.AspNetCore.Mvc;

using AdminBlazor.Components.Pages;
using DevExpress.Export.Xl;
using Facilitate.Libraries.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Reactive;
using System.Threading;

using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using AspNetCore.Identity.Mongo.Mongo;
using System.Collections;
using static Facilitate.Libraries.Models.Constants.Messaging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminBlazor.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberService : ControllerBase
    {
        string serviceResult = string.Empty;
        string dbName = "Facilitate";
        string collectionName = "Users";

        string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";

        IMongoClient client;

        IMongoCollection<User> collection;
        private CancellationToken _cancellationToken;

        //public List<ListItem> GetMembers(Tuple<ObjectId, string> memberType)
        public string[] GetMembers(Tuple<ObjectId, string> memberType)
        {
            List<User> members = null;
            List<ListItem> projectManagers = new List<ListItem>();

            ObjectId memberTypeId = memberType.Item1;
            string memberTypeValue = memberType.Item2;

            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<User>(collectionName);

                var builder = Builders<User>.Filter;
                var filter = builder.Ne(f => f.UserName, "facilitate-null");

                members = collection.Find(filter).ToList();

                string[] projectManagersArray = new string[members.Count];
                int index = 0;

                foreach (User member in members)
                {
                    if (member.Roles.Contains(memberTypeId.ToString()))
                    {
                        //members.Append(member);
                        //ListItem listItem = new ListItem();
                        //listItem.Text = member.NormalizedUserName;
                        //listItem.Value = member._id.ToString();
                        //projectManagers.Add(listItem);

                        var memberData = member.NormalizedUserName;
                        memberData += " (" + member.Email + ")";
                        //memberData += " | " + member._id;

                        projectManagersArray[index] = memberData;

                        index++;
                    }
                }

                return projectManagersArray;
            }
            catch (Exception ex)
            {
                serviceResult = ex.Message;
            }
            finally
            {

            }
            return null;
        }

        //public IEnumerable<User> GetMembers(string memberType)
        //{
        //    try
        //    {
        //        client = new MongoClient(mongoUri);
        //        collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

        //        var builder = Builders<Member>.Filter;
        //        var filter = builder.Eq(f => f.Roles, memberType);

        //        //var allMembers = collection.Find(filter).ToList();
        //        var allMembers = collection.Find(filter).ToList();

        //        //var sortedQuotes = collection.Find(filter).SortByDescending(e => e.timestamp).ToList();

        //        return allMembers;
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult = ex.Message;
        //    }
        //    finally
        //    {

        //    }
        //    return null;
        //}

        //// GET: api/<MemberService>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<MemberService>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //// GET api/<MemberService>/projectmanager
        //[HttpGet("{memberType}")]
        //public string Get(string memberType)
        //{
        //    return "value";
        //}

        //// POST api/<MemberService>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //    serviceResult = string.Empty;
        //}

        //// PUT api/<MemberService>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //    serviceResult = string.Empty;
        //}

        //// DELETE api/<MemberService>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    serviceResult = string.Empty;
        //}
    }
}
