using Mezubo_Api.Core.Entities;
using Mezubo_Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mezubo_Api.Business
{
    public class BetsBL : IBetsBL
    {
        private readonly IMongoRepository<RouletteEntity> _rouletteRepository;
        private readonly IMongoRepository<BetEntity> _betRepository;
        private readonly IMongoRepository<BetResultEntity> _betResultRepository;

        public BetsBL(IMongoRepository<RouletteEntity> rouletteRepository, IMongoRepository<BetEntity> betRepository, IMongoRepository<BetResultEntity> betResultRepository)
        {
            _rouletteRepository = rouletteRepository;
            _betRepository = betRepository;
            _betResultRepository = betResultRepository;
        }

        public async Task<GenericResponseEntity<string>> OpenBet(string id)
        {
            RouletteEntity rouletteUpd = _rouletteRepository.GetById(id).Result.Result;
            if (rouletteUpd != null)
            {
                rouletteUpd.Open = true;
                rouletteUpd.OpenDate = DateTime.Now;
                return await _rouletteRepository.UpdateDocument(rouletteUpd);
            }
            GenericResponseEntity<string> response = new GenericResponseEntity<string>()
            {
                Result = "El Id de la ruleta ha actualizar no existe. Por favor verifique el Id"
            };
            return response;
        }

        public async Task<GenericResponseEntity<string>> StartBet(BetEntity rouletteBet)
        {
            GenericResponseEntity<string> response = new GenericResponseEntity<string>();
            Tuple<bool, string> validations = BetValidation(rouletteBet);
            if (validations.Item1)
            {
                rouletteBet.Id = "";
                await _betRepository.InsertDocument(rouletteBet);
                response.Result = "Apuesta realizada con éxito";
            }
            else
            {
                response.Result = validations.Item2;
            }

            return response;
        }

        public async Task<GenericResponseEntity<BetResultEntity>> ClosedBet(string id)
        {
            GenericResponseEntity<BetResultEntity> response = new GenericResponseEntity<BetResultEntity>();
            if (RouletteExistsOrOpen(id))
            {
                var result = await _betRepository.GetAll();
                if (result.Result != null)
                {
                    response.Message = "Tenermos ganadores";
                    response.Result = GenerateBetResult(result.Result.Where(x => x.RouletteId.Equals(id)).ToList());
                    await _betResultRepository.InsertDocument(response.Result);
                }
                else
                    response.Message = "No hay apuestas realizadas a la ruleta";
            }
            else
                response.Message = "El Id de la ruleta ha cerrar la apuesta no existe o la ruleta esta cerrada.";

            return response;
        }

        private Tuple<bool, string> BetValidation(BetEntity rouletteBet)
        {
            bool validations = false;
            string msg = "";
            if (RouletteExistsOrOpen(rouletteBet.RouletteId))
            {
                if (rouletteBet.Color.Trim().ToLower().Equals("rojo") || rouletteBet.Color.Trim().ToLower().Equals("negro"))
                {
                    if (rouletteBet.Number < 0 || rouletteBet.Number > 36)
                        msg = "Debes apostar del 0 al 36";
                    else if (rouletteBet.Value <= 0 || rouletteBet.Value > 10000)
                        msg = "El valor mínimo de la apuesta es de US$1 y el valor máximo de la apuesta es de US$10.000";
                    else
                        validations = true;
                }
                else
                    msg = "Debe agregar color rojo o negro a la apuesta";
            }
            else
                msg = "La ruleta ha apostar no existe o la ruleta no esta abierta para apuestas";

            return Tuple.Create(validations, msg);
        }

        private bool RouletteExistsOrOpen(string id)
        {
            bool result = false;
            RouletteEntity roulette = _rouletteRepository.GetById(id).Result.Result;
            if (roulette != null && roulette.Open)
            {
                result = true;
            }
            return result;
        }

        private BetResultEntity GenerateBetResult(List<BetEntity> listBets)
        {
            BetResultEntity result = new BetResultEntity();
            Random random = new Random();
            result.WinningNumber = random.Next(0, 36);
            result.WinningColor = result.WinningNumber % 2 == 0 ? "rojo" : "negro";
            List<BetEntity> lstBetW = listBets.Where(x => x.Color.Trim().ToLower().Equals(result.WinningColor)).ToList();
            if (lstBetW.Count > 0)
            {
                result.ListWinners.AddRange(CalculateWinningValues("color", lstBetW));
            }
            lstBetW = listBets.Where(x => x.Color.Trim().ToLower().Equals(result.WinningNumber)).ToList();
            if (lstBetW.Count > 0)
            {
                result.ListWinners.AddRange(CalculateWinningValues("numero", lstBetW));
            }
            result.ListBets = listBets;
            MovementsInDatabase(listBets);

            return result;
        }

        private IEnumerable<WinnerEntity> CalculateWinningValues(string type, List<BetEntity> lstBetW)
        {
            List<WinnerEntity> lstBetWinners = new List<WinnerEntity>();
            double valueToCalculate = type.Equals("color") ? 1.8 : 5;
            foreach (BetEntity item in lstBetW)
            {
                WinnerEntity betWinners = new WinnerEntity()
                {
                    EarnedValue = item.Value * Convert.ToDecimal(valueToCalculate),
                    IsWinningColor = type.Equals("color"),
                    IsWinningNumber = type.Equals("numero")
                };
                lstBetWinners.Add(betWinners);
            }

            return lstBetWinners;
        }

        private async void MovementsInDatabase(List<BetEntity> listBets)
        {
            foreach (var item in listBets)
            {
                await _betRepository.DeleteById(item.Id);
            }
            var result = await _rouletteRepository.GetById(listBets.Select(x => x.RouletteId).FirstOrDefault());
            result.Result.Open = false;
            result.Result.ClosedDate = DateTime.Now;
            await _rouletteRepository.UpdateDocument(result.Result);
        }
    }
}
