using JsonDb.Adapters;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace JsonDb.Local
{
    internal class LocalJsonDbFactory : IJsonDbFactory
    {
        private readonly LocalJsonDbOptions options;

        public LocalJsonDbFactory( IOptions<LocalJsonDbOptions> optionsAccessor )
        {
            options = optionsAccessor.Value;
        }

        public IJsonDb GetJsonDb()
        {
            return new LocalJsonDb(options.DbPath, options.CollectionAdapter, options.JsonSerializerOptions);
        }

        public IJsonDb GetJsonDb(string dbPath)
        {
            return new LocalJsonDb(dbPath, options.CollectionAdapter, options.JsonSerializerOptions);
        }

        public IJsonDb GetJsonDb(string dbPath, IJsonCollectionAdapter collectionAdapter)
        {
            return new LocalJsonDb(dbPath, collectionAdapter, options.JsonSerializerOptions);
        }

        public IJsonDb GetJsonDb(string dbPath, IJsonCollectionAdapter collectionAdapter, JsonSerializerOptions jsonSerializerOptions)
        {
            return new LocalJsonDb(dbPath, collectionAdapter, jsonSerializerOptions);
        }

        public IJsonDb GetJsonDb(IJsonCollectionAdapter collectionAdapter)
        {
            return new LocalJsonDb(options.DbPath, collectionAdapter, options.JsonSerializerOptions);
        }

        public IJsonDb GetJsonDb(IJsonCollectionAdapter collectionAdapter, JsonSerializerOptions jsonSerializerOptions)
        {
            return new LocalJsonDb(options.DbPath, collectionAdapter, jsonSerializerOptions);
        }

        public IJsonDb GetJsonDb(JsonSerializerOptions jsonSerializerOptions)
        {
            return new LocalJsonDb(options.DbPath, options.CollectionAdapter, jsonSerializerOptions);
        }
    }
}
