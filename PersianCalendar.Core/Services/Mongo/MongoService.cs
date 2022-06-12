namespace PersianCalendar.Core.Services.Mongo
{
    public class MongoService
    {
        private readonly IMongoCollection<UserInfo> collection;

        public MongoService(IOptions<MongoDBConfig> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            collection = database.GetCollection<UserInfo>(options.Value.CollectionName);
        }

        public async Task SaveUserData(UserInfo userInfo)
        {
            if (await GetAsync(userInfo) == null)
                await CreateAsync(userInfo);
        }

        public async Task<List<UserInfo>> GetAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        private async Task<UserInfo?> GetAsync(UserInfo userInfo)
        {
            return await collection.Find(x => x.UserId == userInfo.UserId).FirstOrDefaultAsync();
        }

        private async Task CreateAsync(UserInfo userInfo)
        {
            await collection.InsertOneAsync(userInfo);
        }

    }
}
