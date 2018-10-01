using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Model.Customers;
using Demo.Model.Master;
using Demo.Model.Sales;

namespace Demo.Service.Sales
{
   public interface ISalesService
    {
        
        bool SaveSales(SalesViewModel customerViewModel);

        /// <summary>
        /// Update Customer information
        /// </summary>
        /// <param name="Customer"></param>        
        /// <returns></returns>
        bool UpdateCustomers(SalesViewModel customerViewModel);


        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool Delete(long Id);

        /// <summary>
        /// Get all Customer
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        List<SalesViewModel> GetAllSalesRecord(int Id);

        


        /// <summary>
        /// Get Customer detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        CustomersViewModel GetCustomersDetailsById(long Id);

        

    }
}
