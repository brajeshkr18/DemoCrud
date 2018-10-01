using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.EntityModel;
using Demo.Model.Customers;
using Demo.Model.Master;
using Demo.Service.Customer;
using Demo.Model.Sales;

namespace Demo.Service.Sales
{
   public class SalesService : ISalesService
    {
        private TransportManagementSystemEntities _Context = new TransportManagementSystemEntities();

        #region Public_Methods

        /// <summary>
        /// Save Users
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="logId"></param>
        /// <param name="validateOnSaveEnabled"></param>
        /// <param name="mailBodyTemplate"></param>
        /// <returns></returns>
        public bool SaveSales(SalesViewModel salesViewModel)
        {
            bool status = false;

            tblProductSold product = new tblProductSold();
            Mapper.Map(salesViewModel, product);
            product.ProductId = salesViewModel.ProductId;
            product.CustomerId = salesViewModel.CustomerId;
            product.StoreId = salesViewModel.StoreId;
            product.DateSold = salesViewModel.PurchasedDate;
            product.IsActive = true;
            product.CreatedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;
            product.CreatedBy = "101";
            product.ModifiedBy = "101";
            _Context.tblProductSolds.Add(product);
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
        public bool UpdateCustomers(SalesViewModel salesViewModel)
        {
            bool status = false;
            try
            {
                //var _usrsaltdetails = _Context.Users.FirstOrDefault(x => x.Id == user.Id);
                var _customerDetails = _Context.tblProductSolds.Find(salesViewModel.Id);

                if (_customerDetails != null)
                {
                    Mapper.Map(salesViewModel, _customerDetails);
                    _customerDetails.ProductId = salesViewModel.ProductId;
                    _customerDetails.CustomerId = salesViewModel.CustomerId;
                    _customerDetails.StoreId = salesViewModel.StoreId;
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
                var entity = _Context.tblCustomers.Find(Id);
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
        public List<SalesViewModel> GetAllSalesRecord(int Id)
        {
            List<SalesViewModel> entities = new List<SalesViewModel>();
            // making values as trim  

            //var list = _Context.tblProductSolds.Include("tblCustomer").Include("tblProduct").Include("tblStore").Where(x=>x.IsActive==true).ToList();
            var list = _Context.GetSalesDetail(Id).ToList();
            Mapper.Map(list, entities);
            
            return entities;
        }

        /// <summary>
        /// Get all Customers for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
       


        /// <summary>
        /// Get Customers detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomersViewModel GetCustomersDetailsById(long Id)
        {
            tblCustomer customers = _Context.tblCustomers.Where(x => x.id == Id).FirstOrDefault();
            return Mapper.Map(customers, new CustomersViewModel());

        }

       
        #endregion
    }
}
