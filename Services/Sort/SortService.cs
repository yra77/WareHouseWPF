

using WareHouseWPF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WareHouseWPF.Services.Sort
{
    internal class SortService : ISortService
    {

        public List<Product> SortProduct(string columnName, List<Product> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

                case "Type":
                    return list.AsParallel().OrderBy(x => x.TypeId).ToList();

                case "Category":
                    return list.AsParallel().OrderBy(x => x.CategoryId).ToList();

                case "Quantity":
                    return list.AsParallel().OrderBy(x => x.Quantity).ToList();

                case "Price":
                    return list.AsParallel().OrderBy(x => x.Price).ToList();

                case "Shipper":
                    return list.AsParallel().OrderBy(x => x.ShipperId).ToList();

                case "Code":
                    return list.AsParallel().OrderBy(x => x.Code).ToList();

                case "WarehouseNumber":
                    return list.AsParallel().OrderBy(x => x.WarehouseNumber).ToList();

                case "DateArrival":
                    return list.AsParallel().OrderBy(x => x.DateArrival).ToList();
            }
            return list;
        }

        public List<Shipper> SortShipper(string columnName, List<Shipper> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

                case "Phone":
                    return list.AsParallel().OrderBy(x => x.Phone).ToList();

                case "BankName":
                    return list.AsParallel().OrderBy(x => x.BankAccount[0].Name).ToList();

                case "BankAccount":
                    return list.AsParallel().OrderBy(x => x.BankAccount[0].BankAccounts).ToList();

            }
            return list;
        }

        public List<ClientModel> SortClient(string columnName, List<ClientModel> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

                case "SecondName":
                    return list.AsParallel().OrderBy(x => x.SecondName).ToList();

                case "Organization":
                    return list.AsParallel().OrderBy(x => x.Organization).ToList();

                case "City":
                    return list.AsParallel().OrderBy(x => x.City).ToList();

                case "Street":
                    return list.AsParallel().OrderBy(x => x.Street).ToList();

                case "Email":
                    return list.AsParallel().OrderBy(x => x.Email).ToList();

                case "BankName":
                    return list.AsParallel().OrderBy(x => x.BanksAccount[0].Name).ToList();

                case "Phone":
                    return list.AsParallel().OrderBy(x => x.PhoneNumber).ToList();
            }
            return list;
        }

        public List<Employee> SortEmployee(string columnName, List<Employee> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

                case "SecondName":
                    return list.AsParallel().OrderBy(x => x.SecondName).ToList();

                case "Age":
                    return list.AsParallel().OrderBy(x => x.Age).ToList();

                case "Email":
                    return list.AsParallel().OrderBy(x => x.Email).ToList();

                case "Phone":
                    return list.AsParallel().OrderBy(x => x.PhoneNumber).ToList();

                case "Role":
                    return list.AsParallel().OrderBy(x => x.Role).ToList();

                case "JobTitle":
                    return list.AsParallel().OrderBy(x => x.JobTitle).ToList();

                case "BankName":
                    return list.AsParallel().OrderBy(x => x.BankAccount[0].Name).ToList();

                case "BankAccount":
                    return list.AsParallel().OrderBy(x => x.BankAccount[0].BankAccounts).ToList();
            }
            return list;
        }

        public List<Categories> SortCategory(string columnName, List<Categories> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

            }
            return list;
        }

        public List<ProductType> SortProductType(string columnName, List<ProductType> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

            }
            return list;
        }

        public List<ArrivalOfGoods> SortArrival(string columnName, List<ArrivalOfGoods> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

                case "Employee":
                    return list.AsParallel().OrderBy(x => x.EmployeeId).ToList();

                case "WarehouseNumber":
                    return list.AsParallel().OrderBy(x => x.WarehouseNumber).ToList();

                case "Shipper":
                    return list.AsParallel().OrderBy(x => x.ShipperId).ToList();

            }
            return list;
        }

        public List<TransferOfGoods> SortTransfer(string columnName, List<TransferOfGoods> list)
        {
            switch (columnName)
            {
                case "Id":
                    return list.AsParallel().OrderBy(x => x.Id).ToList();

                case "Name":
                    return list.AsParallel().OrderBy(x => x.Name).ToList();

                case "Employee":
                    return list.AsParallel().OrderBy(x => x.EmployeeId).ToList();

                case "WarehouseNumber":
                    return list.AsParallel().OrderBy(x => x.WarehouseNumber).ToList();

                case "Client":
                    return list.AsParallel().OrderBy(x => x.ClientId).ToList();
            }
            return list;
        }
    }
}
