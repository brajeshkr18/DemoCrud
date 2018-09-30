using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Demo.Model.Store
{
    public class StoreViewModel
    {
           public int Id { get; set; }
         
            public string Name { get; set; }

            public string Address { get; set; }

            
    }
}
