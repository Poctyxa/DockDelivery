using DockDelivery.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestDataLibrary
{
    public class MongoDbFixture : IDisposable
    {
        private readonly IMongoClient mClient;
        public readonly IMongoDatabase mDatabase;
        private const string DatabaseName = "DockDeliveryTestDb";
        bool isDisposed = false;

        public MongoDbFixture()
        {
            mClient = new MongoClient();
            mDatabase = mClient.GetDatabase(DatabaseName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
                mClient.DropDatabase(DatabaseName);

            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
