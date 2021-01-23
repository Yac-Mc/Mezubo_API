using Mezubo_Api.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mezubo_Api.Repository
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        Task<GenericResponseEntity<IEnumerable<TDocument>>> GetAll();
        Task<GenericResponseEntity<TDocument>> GetById(string id);
        Task<GenericResponseEntity<string>> InsertDocument(TDocument document);
        Task<GenericResponseEntity<string>> UpdateDocument(TDocument document);
        Task<GenericResponseEntity<string>> DeleteById(string id);
    }
}
