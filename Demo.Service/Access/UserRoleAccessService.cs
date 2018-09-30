using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.EntityModel;
using TMS.Model.Master;
using TMS.Model.PayChequeRequest;
using TMS.Model.TripRequest;
using TMS.Model.Users;
using System.Data.Entity;
using TMS.Model.ViewModels;

namespace TMS.Service.UserRoleAccess
{
    public class UserRoleAccessService : IUserRoleAccessService
    {
        private TransportManagementSystemEntities _Context = new TransportManagementSystemEntities();

        #region Public_Methods


        /// <summary>
        ///  Save Trip Request
        /// </summary>
        /// <param name="TripRequest"></param>
        /// <param name="logId"></param>
        /// <param name="validateOnSaveEnabled"></param>
        /// <param name="mailBodyTemplate"></param>
        /// <returns></returns>
        public bool SavePayChequeRequest(PayChequeRequestViewModel paychequeRequestViewModel, long logId = 0, bool validateOnSaveEnabled = true, string mailBodyTemplate = "")
        {
            bool status = false;

            BookingRequests bookingRequests = new BookingRequests();
            Mapper.Map(paychequeRequestViewModel, bookingRequests);
            bookingRequests.IsActive = true;
            bookingRequests.CreatedOn = DateTime.Now;
            //bookingRequests.Status = (int)Utility.Enums.BookingRequestStatus.PayChequeRequest;
            bookingRequests.IsPayChequeRequest = true;
            bookingRequests.IsDeleted = false;
            bookingRequests.BeginMile = paychequeRequestViewModel.BeginMiles;
            bookingRequests.EndMile = paychequeRequestViewModel.EndMiles;
            bookingRequests.PickUp = paychequeRequestViewModel.PickUp;
            bookingRequests.DropOff = paychequeRequestViewModel.DropOff;
            bookingRequests.BackupVendorId = 123;
            bookingRequests.PaychequeCreated = DateTime.Now;
            //bookingRequests.WaitTimeStart = paychequeRequestViewModel.WaitTimeStart;
            //bookingRequests.WaitTimeEnd = paychequeRequestViewModel.WaitTimeEnd;
            BookingRequestsHistory bookingRequestsHistory = new BookingRequestsHistory();
            Mapper.Map(paychequeRequestViewModel, bookingRequestsHistory);
            _Context.BookingRequests.Add(bookingRequests);
            _Context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
            _Context.SaveChanges();
            bookingRequestsHistory.RequestId = bookingRequests.Id;
            bookingRequestsHistory.RailRoadId = bookingRequests.RailRoadId;
            bookingRequestsHistory.BeginMile = bookingRequests.BeginMile;
            bookingRequestsHistory.EndMile = bookingRequests.EndMile;
            bookingRequestsHistory.CreatedOn = DateTime.Now;
            bookingRequestsHistory.PickUp = paychequeRequestViewModel.PickUp;
            bookingRequestsHistory.DropOff = paychequeRequestViewModel.DropOff;
            bookingRequestsHistory.PaychequeCreated = DateTime.Now;
            //bookingRequestsHistory.WaitTimeStart = paychequeRequestViewModel.WaitTimeStart; ;
            //bookingRequestsHistory.WaitTimeEnd = paychequeRequestViewModel.WaitTimeEnd;
            _Context.BookingRequestsHistory.Add(bookingRequestsHistory);
            _Context.SaveChanges();
            status = true;


            return status;
        }


        /// <summary>
        /// Update Trip Request
        /// </summary>
        /// <param name="Trip Request"></param>
        /// <returns></returns>
        public bool UpdatePayChequeRequest(PayChequeRequestViewModel paychequeRequestViewModel)
        {
            bool status = false;
            try
            {
                BookingRequestsHistory bookingRequestsHistory = new BookingRequestsHistory();
                var _bookingDetails = _Context.BookingRequests.FirstOrDefault(x => x.Id == paychequeRequestViewModel.Id);

                if (_bookingDetails != null)
                {

                  
                    _bookingDetails.ActualDistance = paychequeRequestViewModel.TotalMiles;
                    _bookingDetails.WaitTime = paychequeRequestViewModel.WaitTime;
                    //_bookingDetails.Status = (int)Utility.Enums.BookingRequestStatus.PayChequeRequest;

                    _bookingDetails.CreatedBy = paychequeRequestViewModel.CreatedBy;
                    _bookingDetails.BeginMile = paychequeRequestViewModel.BeginMiles;
                    _bookingDetails.EndMile = paychequeRequestViewModel.EndMiles;
                    _bookingDetails.ModifiedOn = DateTime.Now;
                    _bookingDetails.PickUp = paychequeRequestViewModel.PickUp;
                    _bookingDetails.DropOff = paychequeRequestViewModel.DropOff;
                    _bookingDetails.IsPayChequeRequest = true;
                    _bookingDetails.PaychequeCreated = DateTime.Now;
                    ////_bookingDetails.WaitTimeStart = paychequeRequestViewModel.WaitTimeStart;
                    ////_bookingDetails.WaitTimeEnd = paychequeRequestViewModel.WaitTimeEnd;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    Mapper.Map(_bookingDetails, bookingRequestsHistory);
                    bookingRequestsHistory.RequestId = paychequeRequestViewModel.Id;
                    bookingRequestsHistory.CreatedOn = DateTime.Now;
                    bookingRequestsHistory.ActualDistance = paychequeRequestViewModel.TotalMiles;
                    bookingRequestsHistory.WaitTime = paychequeRequestViewModel.WaitTime;
                    bookingRequestsHistory.Status = (int)Utility.Enums.BookingRequestStatus.PayChequeRequest;
                    bookingRequestsHistory.BeginMile = paychequeRequestViewModel.BeginMiles;
                    bookingRequestsHistory.EndMile = paychequeRequestViewModel.EndMiles;
                    bookingRequestsHistory.ModifiedOn = DateTime.Now;
                    bookingRequestsHistory.PickUp = paychequeRequestViewModel.PickUp;
                    bookingRequestsHistory.DropOff = paychequeRequestViewModel.DropOff;
                    bookingRequestsHistory.PaychequeCreated = DateTime.Now;
                    //bookingRequestsHistory.WaitTimeStart = paychequeRequestViewModel.WaitTimeStart;
                    //bookingRequestsHistory.WaitTimeEnd = paychequeRequestViewModel.WaitTimeEnd;
                    _Context.BookingRequestsHistory.Add(bookingRequestsHistory);
                    _Context.SaveChanges();

                    status = true;

                }

            }

            catch (Exception ex)
            {
            }
            return status;
        }

        public List<SubMenuViewModel> GetAllRoleAccesByRoleId(long RoleId)
        {

            List<SubMenuViewModel> lst = new List<SubMenuViewModel>();
            var menulist = (from Sm in _Context.tblSubMenu
                                       join m in _Context.tblMainMenu on Sm.MainMenuId equals m.Id
                                       join role in _Context.UserTypes on Sm.RoleId equals role.Id
                                       //where vAlloc.VehicleId == VehicleId
                                       //orderby vAlloc.StartDate
                                       select new SubMenuViewModel
                                       {
                                           Role=role.Name,
                                           Id=Sm.Id,
                                           RoleId=Sm.RoleId,
                                           Action=Sm.Action,
                                           Controller=Sm.Controller,
                                           SubMenu = Sm.SubMenu,
                                           MainMenu=m.MainMenu,
                                           CreatedOn=Sm.CreatedOn,
                                           MenuOrder=Sm.MenuOrder,
                                           IsActive= Sm.IsActive,
                                           IsDeleted= Sm.IsDeleted
                                       
                                       }).Where(x=>x.RoleId==RoleId).ToList();
            Mapper.Map(menulist, lst);
            return lst;
        }
        public bool RoleAccesByRoleId(List<SubMenuViewModel> entity,int RoleId,long logId = 0)
        { 
        bool status = false;
            if (entity != null)
            {
                foreach (var item in entity)
                {
                    var _SubMenuDetails = _Context.tblSubMenu.FirstOrDefault(x => x.Id == item.Id && x.RoleId== RoleId);

                    if (_SubMenuDetails != null)
                    {
                        _SubMenuDetails.IsActive = item.IsActive;
                       _Context.SaveChanges();
                    }
                }
               
                status = true;
            }
            return status;
        }


        #endregion 
    }
}
  

