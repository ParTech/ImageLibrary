//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParTech.ImageLibrary.Core.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gender
    {
        public Gender()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int GenderID { get; set; }
        public string Name { get; set; }
        public int LanguageID { get; set; }
        public System.DateTime created { get; set; }
        public System.DateTime updated { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
    }
}
