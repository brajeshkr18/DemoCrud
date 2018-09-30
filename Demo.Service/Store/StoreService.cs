using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.EntityModel;
using Demo.Model.Store;
using Demo.Model.Master;
using Demo.Service.Store;

namespace Demo.Service.Store
{
   public class StoreService : IStoreService
    {
        private TransportManagementSystemEntities _Context = new TransportManagementSystemEntities();

        #region Public_Methods

        /// <summary>
        /// Save Store
        /// </summary>
        /// <param name="Store"></param>

        /// <returns></returns>
        public bool SaveStores(StoreViewModel customerViewModel)
        {
            bool status = false;

            tblStore stores = new tblStore();
            Mapper.Map(customerViewModel, stores);

            stores.IsActive = true;
            stores.CreatedDate = DateTime.Now;
            stores.ModifiedDate = DateTime.Now;
            stores.CreatedBy = "101";
            stores.ModifiedBy = "101";
            _Context.tblStores.Add(stores);
            _Context.Configuration.ValidateOnSaveEnabled = true;
            _Context.SaveChanges();
            status = true;


            return status;
            // for new users
        }

        /// <summary>
        /// Update Customers information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ProfileMedia"></param>
        /// <returns></returns>
        public bool UpdateStores(StoreViewModel customerViewModel)
        {
            bool status = false;
            try
            {
                //var _usrsaltdetails = _Context.Users.FirstOrDefault(x => x.Id == user.Id);
                var _customerDetails = _Context.tblStores.Find(customerViewModel.Id);

                if (_customerDetails != null)
                {
                    Mapper.Map(customerViewModel, _customerDetails);
                    _customerDetails.ModifiedDate = DateTime.Now;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;

                }

            }

            catch (Exception ex)
            {
            }
            // for new users
            return status;
        }

        /// <summary>
        /// Delete Customers
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(long Id)
        {
            try
            {
                var entity = _Context.tblStores.Find(Id);
                if (entity != null)
                {
                    entity.IsActive = false;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }

            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Get all Customers
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        public List<StoreViewModel> GetAllStores()
        {
            List<StoreViewModel> entities = new List<StoreViewModel>();
            // making values as trim  
           
            var list = _Context.tblStores.Where(x=>x.IsActive==true).ToList();

            Mapper.Map(list, entities);
            
            return entities;
        }

        /// <summary>
        /// Get all Customers for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        public List<StoreViewModel> GetStoresForDropDown()
        {
            return (from customer in GetAllStores()
                    orderby customer.Name
                    select new StoreViewModel
                    {
                        Id = customer.Id,
                        Name = customer.Name
                    }).ToList();
        }


        /// <summary>
        /// Get Store detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public StoreViewModel GetStoresDetailsById(long Id)
        {
            tblStore store = _Context.tblStores.Where(x => x.id == Id).FirstOrDefault();
            return Mapper.Map(store, new StoreViewModel());

        }


        #endregion
    }
}
