using Newtonsoft.Json;
using System.Net;

namespace Mezubo_Api.Core.Entities
{
    public class GenericResponseEntity<T>
    {
        public bool IsSuccessful { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public GenericResponseEntity()
        {
            StatusCode = (int)HttpStatusCode.OK;
            IsSuccessful = true;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
