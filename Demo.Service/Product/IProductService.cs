using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Model.Customers;
using Demo.Model.Master;
using Demo.Model.Product;

namespace Demo.Service.Product
{
   public interface IProductService
    {
        
        bool SaveProducts(ProductViewModel customerViewModel);

        /// <summary>
        /// Update Customer information
        /// </summary>
        /// <param name="Customer"></param>        
        /// <returns></returns>
        bool UpdateProducts(ProductViewModel ProductViewModel);


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
        List<ProductViewModel> GetAllProducts();

        /// <summary>
        /// Get all Customer for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        List<ProductViewModel> GetProductsForDropDown(SearchingParams searchingParams);


        /// <summary>
        /// Get Customer detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ProductViewModel GetProductsDetailsById(long Id);

        

    }
}
