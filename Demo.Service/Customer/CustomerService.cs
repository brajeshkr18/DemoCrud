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

namespace Demo.Service.Customer
{
   public class CustomerService :ICustomerService
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
        public bool SaveCustomers(CustomersViewModel customerViewModel)
        {
            bool status = false;

            tblCustomer customers = new tblCustomer();
            Mapper.Map(customerViewModel, customers);
           
            customers.IsActive = true;
            customers.CreatedDate = DateTime.Now;
            customers.ModifiedDate = DateTime.Now;
            customers.CreatedBy = "101";
            customers.ModifiedBy = "101";
            _Context.tblCustomers.Add(customers);
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
        public bool UpdateCustomers(CustomersViewModel customerViewModel)
        {
            bool status = false;
            try
            {
                //var _usrsaltdetails = _Context.Users.FirstOrDefault(x => x.Id == user.Id);
                var _customerDetails = _Context.tblCustomers.Find(customerViewModel.Id);

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
        public List<CustomersViewModel> GetAllCustomer()
        {
            List<CustomersViewModel> entities = new List<CustomersViewModel>();
            // making values as trim  
           
            var list = _Context.tblCustomers.Where(x=>x.IsActive==true).ToList();

            Mapper.Map(list, entities);
            
            return entities;
        }

        /// <summary>
        /// Get all Customers for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        public List<CustomersViewModel> GetCustomersForDropDown(SearchingParams searchingParams)
        {
            return (from customer in GetAllCustomer()
                    orderby customer.Name
                    select new CustomersViewModel
                    {
                        Id = customer.Id,
                        Name = customer.Name
                    }).ToList();
        }


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
