using Facilitate.Libraries.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facilitate.Libraries.Services
{
    public class DBService
    {
        public readonly IMongoClient MongoClient;
        public readonly string ConnectionString;
        public readonly string DatabaseName;

        private readonly IMongoDatabase _mongoDatabase;

        public DBService(IOptions<DBSettings> dbSettings)
        {
            ConnectionString = dbSettings.Value.ConnectionString;
            DatabaseName = dbSettings.Value.DatabaseName;
            MongoClient = new MongoClient(ConnectionString);
            _mongoDatabase = MongoClient.GetDatabase(DatabaseName);
        }


        public IMongoDatabase MongoDatabase => _mongoDatabase;



        public IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings? settings = null)
        {
            return _mongoDatabase.GetCollection<TDocument>(name, settings);
        }

        private IMongoCollection<Quote>? quotes = null;
        public IMongoCollection<Quote> Quotes
        {
            get
            {
                quotes ??= _mongoDatabase.GetCollection<Quote>(KnownCollection.Quote.ToString());
                return quotes;
            }
        }

        private IMongoCollection<ReferenceData>? referenceData = null;
        public IMongoCollection<ReferenceData> ReferenceData
        {
            get
            {
                referenceData ??= _mongoDatabase.GetCollection<ReferenceData>(KnownCollection.ReferenceData.ToString());
                return referenceData;
            }
        }

        private IMongoCollection<Role>? roles = null;
        public IMongoCollection<Role> Roles
        {
            get
            {
                roles ??= _mongoDatabase.GetCollection<Role>(KnownCollection.Roles.ToString());
                return roles;
            }
        }

        private IMongoCollection<User>? users = null;
        public IMongoCollection<User> Users
        {
            get
            {
                users ??= _mongoDatabase.GetCollection<User>(KnownCollection.Users.ToString());
                return users;
            }
        }

        public enum KnownCollection
        {
            Quote,
            ReferenceData,
            Roles,
            Users
        }
    }
}