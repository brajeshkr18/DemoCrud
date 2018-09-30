using ExpressMapper;
using System;
using Demo.Core.EntityModel;
using Demo.Utility;

namespace Demo.Web.Helper
{
    public class Mapping
    {
        public static void RegisterViewModelAndModelMapping()
        {
            //Mapper.Register<Users, UserViewModel>()
            //    .Member(dest => dest.UserTypeCode, src => src.UserTypes.Code)
            //    .Member(dest => dest.UserTypeName, src => src.UserTypes.Code);
        }

    }
}