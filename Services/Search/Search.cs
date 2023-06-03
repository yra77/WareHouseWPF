

using WareHouseWPF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WareHouseWPF.Services.Search
{
    internal class Search : ISearch
    {

        private List<Product> _productList;
        private List<Employee> _employeeList;
        private List<ClientModel> _clientList;
        private List<Shipper> _shipperList;
        private List<Categories> _categoryList;
        private List<ProductType> _productTypeList;


        public void CreateList<T>(string tableName, List<T> list) where T :  BaseModel
        {
            switch (tableName)
            {
                case "Product":
                    _productList = new List<Product>(list as List<Product>);
                    break;

                case "Employee":
                    _employeeList = new List<Employee>(list as List<Employee>);
                    break;

                case "Client":
                    _clientList = new List<ClientModel>(list as List<ClientModel>);
                    break;

                case "Shipper":
                    _shipperList = new List<Shipper>(list as List<Shipper>);
                    break;

                case "Category":
                    _categoryList = new List<Categories>(list as List<Categories>);
                    break;

                case "ProductType":
                    _productTypeList = new List<ProductType>(list as List<ProductType>);
                    break;
            }
        }

        public List<Product> SearchProduct(string searchInput)
        {
            if (searchInput.Length > 0)
            {
                return _productList.AsParallel().Where(item => item.Name.ToLower().Contains(searchInput.ToLower())
                                || item.Id.ToString().Contains(searchInput)
                                || item.Code.Contains(searchInput)).ToList();
            }
            else
            {
                return _productList;
            }
        }

        public List<Employee> SearchEmployee(string searchInput)
        {
            if (searchInput.Length > 0)
            {
                return _employeeList.AsParallel().Where(item => item.Name.ToLower().Contains(searchInput.ToLower())
                                || item.PhoneNumber.Contains(searchInput)
                                || item.SecondName.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            else
            {
                return _employeeList;
            }
        }

        public List<ClientModel> SearchClient(string searchInput)
        {
            if (searchInput.Length > 0)
            {
                return _clientList.AsParallel().Where(item => item.Name.ToLower().Contains(searchInput.ToLower())
                                || item.Organization.ToLower().Contains(searchInput.ToLower())
                                || item.SecondName.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            else
            {
                return _clientList;
            }
        }

        public List<Shipper> SearchShipper(string searchInput)
        {
            if (searchInput.Length > 0)
            {
                return _shipperList.AsParallel().Where(item => item.Name.ToLower().Contains(searchInput.ToLower())
                                || item.Id.ToString().Contains(searchInput)
                                || item.Phone.Contains(searchInput)).ToList();
            }
            else
            {
                return _shipperList;
            }
        }

        public List<Categories> SearchCategory(string searchInput)
        {
            if (searchInput.Length > 0)
            {
                return _categoryList.AsParallel().Where(item => item.Name.ToLower().Contains(searchInput.ToLower())
                                || item.Id.ToString().Contains(searchInput)).ToList();
            }
            else
            {
                return _categoryList;
            }
        }

        public List<ProductType> SearchProductType(string searchInput)
        {
            if (searchInput.Length > 0)
            {
                return _productTypeList.AsParallel().Where(item => item.Name.ToLower().Contains(searchInput.ToLower())
                                || item.Id.ToString().Contains(searchInput)).ToList();
            }
            else 
            {
                return _productTypeList;
            }
        }
    }
}
