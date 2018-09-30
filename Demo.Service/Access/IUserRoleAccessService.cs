using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model.Master;
using TMS.Model.PayChequeRequest;
using TMS.Model.TripRequest;
using TMS.Model.ViewModels;

namespace TMS.Service.UserRoleAccess
{
    public interface IUserRoleAccessService
    {
        /// <summary>
        /// Save Booking Request
        /// </summary>
        /// <param name="Bookin gRequest"></param>
        /// <param name="logId"></param>
        /// <param name="validateOnSaveEnabled"></param>
        /// <param name="mailBodyTemplate"></param>
        /// <returns></returns>
        bool SavePayChequeRequest(PayChequeRequestViewModel paychequeRequestViewModel, long logId = 0, bool validateOnSaveEnabled = true, string mailBodyTemplate = "");

        /// <summary>
        /// Update Booking Request information
        /// </summary>
        /// <param name="bookingRequestViewModel"></param>        
        /// <returns></returns>
        bool UpdatePayChequeRequest(PayChequeRequestViewModel paychequeRequestViewModel);
        List<SubMenuViewModel> GetAllRoleAccesByRoleId(long RoleId);
        bool RoleAccesByRoleId(List<SubMenuViewModel> entity, int roleId, long logId = 0);
    }
}
