using Mezubo_Api.Core;
using Mezubo_Api.Core.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mezubo_Api.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IOptions<MongoSettings> opt)
        {
            var client = new MongoClient(opt.Value.ConnectionStrings);
            var db = client.GetDatabase(opt.Value.Database);
            _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }

        public async Task<GenericResponseEntity<IEnumerable<TDocument>>> GetAll()
        {
            var result = await _collection.Find(p => true).ToListAsync();
            GenericResponseEntity<IEnumerable<TDocument>> response = new GenericResponseEntity<IEnumerable<TDocument>>()
            {
                Message = result.Count() > 0 ? "Consulta realizada con éxito" : "En el momento no existen ruletas creadas. Por favor cree una rulta.",
                Result = result
            };

            return response;
        }

        public async Task<GenericResponseEntity<TDocument>> GetById(string id)
        {
            var result = await _collection.Find(Filter(id)).SingleOrDefaultAsync();
            GenericResponseEntity<TDocument> response = new GenericResponseEntity<TDocument>()
            {
                Message = result != null ? "Consulta realizada con éxito" : "El Id de la ruleta ha consultar no existe. Por favor verifique el Id",
                Result = result
            };

            return response;
        }

        public async Task<GenericResponseEntity<string>> InsertDocument(TDocument document)
        {
            await _collection.InsertOneAsync(document);
            GenericResponseEntity<string> response = new GenericResponseEntity<string>()
            {
                Message = "Ruleta creada con éxito",
                Result = document.Id
            };

            return response;
        }

        public async Task<GenericResponseEntity<string>> UpdateDocument(TDocument document)
        {
            var result = await _collection.FindOneAndReplaceAsync(Filter(document.Id), document);
            GenericResponseEntity<string> response = new GenericResponseEntity<string>()
            {
                Result = result != null ? "Ruleta actualizada con éxito" : "El Id de la ruleta ha actualizar no existe. Por favor verifique el Id"
            };

            return response;
        }

        public async Task<GenericResponseEntity<string>> DeleteById(string id)
        {
            var result = await _collection.FindOneAndDeleteAsync(Filter(id));
            GenericResponseEntity<string> response = new GenericResponseEntity<string>()
            {
                Result = result != null ? "Ruleta eliminada con éxito" : "El Id de la ruleta ha eliminar no existe. Por favor verifique el Id"
            };

            return response;
        }

        private static FilterDefinition<TDocument> Filter(string id)
        {
            return Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        }
    }
}
