using Mezubo_Api.Core.Entities;
using System.Threading.Tasks;

namespace Mezubo_Api.Business
{
    public interface IBetsBL
    {
        Task<GenericResponseEntity<string>> OpenBet(string id);
        Task<GenericResponseEntity<string>> StartBet(BetEntity rouletteBet);
        Task<GenericResponseEntity<BetResultEntity>> ClosedBet(string id);
    }
}
