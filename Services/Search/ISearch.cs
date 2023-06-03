

using WareHouseWPF.Models;

using System.Collections.Generic;


namespace WareHouseWPF.Services.Search
{
    internal interface ISearch
    {
        void CreateList<T>(string tableName, List<T> list) where T : BaseModel;
        List<Product> SearchProduct(string searchInput);
        List<Employee> SearchEmployee(string searchInput);
        List<ClientModel> SearchClient(string searchInput);
        List<Shipper> SearchShipper(string searchInput);
        List<Categories> SearchCategory(string searchInput);
        List<ProductType> SearchProductType(string searchInput);
    }
}
