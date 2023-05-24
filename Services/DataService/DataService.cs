

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

        public async Task<bool> InsertUpdateAsync<T>(int id, T item) where T : class, new()
        {
            try
            { 
                if(id > 0)
                {
                    return await _repository.UpdateDataAsync<T>(id, item);
                }

               return await _repository.InsertAsync<T>(item);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error InsertUpdate data service - " + e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync<T>(int id) where T : class, new()
        {
            try
            {
                return await _repository.DeleteAsync<T>(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Delete data service - " + e.Message);
                return false;
            }
        }

    }
}
