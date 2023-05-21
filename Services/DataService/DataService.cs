

using WareHouseWPF.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WareHouseWPF.Services.DataService
{
    internal class DataService : IDataService
    {


        private readonly IRepository _repository;


        public DataService(IRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<T>> GetDataAsync<T>() where T : class, new()
        {
            return await _repository.GetDataAsync<T>();
        }

        public async Task<List<T>> GetDataAsync<T>(int id) where T : class, new()
        {
            return await _repository.GetDataAsync<T>();
        }
    }
}
