using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Model.Customers;
using Demo.Model.Master;


namespace Demo.Service.Customer
{
   public interface ICustomerService
    {
        
        bool SaveCustomers(CustomersViewModel customerViewModel);

        /// <summary>
        /// Update Customer information
        /// </summary>
        /// <param name="Customer"></param>        
        /// <returns></returns>
        bool UpdateCustomers(CustomersViewModel customerViewModel);


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
        List<CustomersViewModel> GetAllCustomer();

        /// <summary>
        /// Get all Customer for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        List<CustomersViewModel> GetCustomersForDropDown(SearchingParams searchingParams);


        /// <summary>
        /// Get Customer detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        CustomersViewModel GetCustomersDetailsById(long Id);

        

    }
}
