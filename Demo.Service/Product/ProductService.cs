using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.EntityModel;
using Demo.Model.Product;
using Demo.Model.Master;
using Demo.Service.Product;

namespace Demo.Service.Product
{
   public class ProductService : IProductService
    {
        private TransportManagementSystemEntities _Context = new TransportManagementSystemEntities();

        #region Public_Methods

        /// <summary>
        /// Save Products
        /// </summary>
        /// <param name="customer"></param>

        public bool SaveProducts(ProductViewModel customerViewModel)
        {
            bool status = false;

            tblProduct customers = new tblProduct();
            Mapper.Map(customerViewModel, customers);
           
            customers.IsActive = true;
            customers.CreatedDate = DateTime.Now;
            customers.ModifiedDate = DateTime.Now;
            customers.CreatedBy = "101";
            customers.ModifiedBy = "101";
            _Context.tblProducts.Add(customers);
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
        public bool UpdateProducts(ProductViewModel customerViewModel)
        {
            bool status = false;
            try
            {
                //var _usrsaltdetails = _Context.Users.FirstOrDefault(x => x.Id == user.Id);
                var _customerDetails = _Context.tblProducts.Find(customerViewModel.Id);

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
                var entity = _Context.tblProducts.Find(Id);
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
        public List<ProductViewModel> GetAllProducts()
        {
            List<ProductViewModel> entities = new List<ProductViewModel>();
            // making values as trim  
           
            var list = _Context.tblProducts.Where(x=>x.IsActive==true).ToList();

            Mapper.Map(list, entities);
            
            return entities;
        }

        /// <summary>
        /// Get all Customers for drop down (get only Id and Name)
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        public List<ProductViewModel> GetProductsForDropDown(SearchingParams searchingParams)
        {
            return (from customer in GetAllProducts()
                    orderby customer.Name
                    select new ProductViewModel
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
        public ProductViewModel GetProductsDetailsById(long Id)
        {
            tblProduct customers = _Context.tblProducts.Where(x => x.id == Id).FirstOrDefault();
            return Mapper.Map(customers, new ProductViewModel());

        }
       
        #endregion
    }
}
