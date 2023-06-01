

using WareHouseWPF.Models;
using System.Collections.Generic;


namespace WareHouseWPF.Services.Sort
{
    internal interface ISortService
    {
        List<Product> SortProduct(string columnName, List<Product> list);
        List<Shipper> SortShipper(string columnName, List<Shipper> list);
        List<ClientModel> SortClient(string columnName, List<ClientModel> list);
        List<Employee> SortEmployee(string columnName, List<Employee> list);
        List<Categories> SortCategory(string columnName, List<Categories> list);
        List<ProductType> SortProductType(string columnName, List<ProductType> list);
        List<TransferOfGoods> SortTransfer(string columnName, List<TransferOfGoods> list);
        List<ArrivalOfGoods> SortArrival(string columnName, List<ArrivalOfGoods> list);
    }
}
