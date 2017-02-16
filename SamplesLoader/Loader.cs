using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using learn_xamarin.Model;
using MongoDB.Driver;

namespace SamplesLoader
{
    public class Loader
    {
        private readonly Dictionary<string, Category> _categoriesByName;

        public Loader()
        {
            _categoriesByName = new Dictionary<string, Category>();
            Console.WriteLine("Load finished");
            Console.ReadKey();
        }

        public void Load(string path)
        {
            var expenditures = ParseFile(path).ToArray();

            var mongo = GetMongoDb();
            mongo.DropCollection("expenditures");
            mongo.DropCollection("categories");
            mongo.CreateCollection("expenditures");
            mongo.CreateCollection("categories");

            foreach (var category in _categoriesByName.Values)
            {
                mongo.GetCollection<Category>("categories").InsertOne(category);
            }
            foreach (var expenditure in expenditures)
            {
                mongo.GetCollection<Expenditure>("expenditures").InsertOne(expenditure);
            }
        }

        private IMongoDatabase GetMongoDb()
        {
            var connectionString = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase("expenditures");
            return db;
        }

        private IEnumerable<Expenditure> ParseFile(string path)
        {
            DateTime? lastDate = null;

            foreach (var line in System.IO.File.ReadAllLines(path))
            {
                var splitProds = line.Split(',');

                var dt = splitProds[0] != string.Empty
                    ? DateTime.ParseExact(splitProds[0], "dd.MM.yyyy", CultureInfo.InvariantCulture)
                    : lastDate.Value;

                lastDate = dt;

                var sum = decimal.Parse(splitProds[1]);
                var category = splitProds[2];

                AddCategoryIfNeeded(category);

                var exp = new Expenditure
                {
                    CategoryId = _categoriesByName[category].Id,
                    Id = Guid.NewGuid(),
                    Sum = sum,
                    Timestamp = dt
                };
                yield return exp;
            }
        }

        private void AddCategoryIfNeeded(string category)
        {
            if (_categoriesByName.ContainsKey(category)) return;
            _categoriesByName.Add(category, new Category {Id = Guid.NewGuid(), Name = category});
        }
    }
}