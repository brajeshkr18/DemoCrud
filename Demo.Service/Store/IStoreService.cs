using System.Collections.Generic;
using Demo.Model.Master;
using Demo.Model.Store;

namespace Demo.Service.Store
{
    public interface IStoreService
    {

        bool SaveStores(StoreViewModel customerViewModel);

        /// <summary>
        /// Update Customer information
        /// </summary>
        /// <param name="Customer"></param>        
        /// <returns></returns>
        bool UpdateStores(StoreViewModel StoreViewModel);


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
        List<StoreViewModel> GetAllStores();

        /// <summary>
        /// Get all Customer for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        List<StoreViewModel> GetStoresForDropDown();


        /// <summary>
        /// Get Customer detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        StoreViewModel GetStoresDetailsById(long Id);



    }
}
