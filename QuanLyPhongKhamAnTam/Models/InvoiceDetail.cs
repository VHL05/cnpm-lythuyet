//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyPhongKhamAnTam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceDetail
    {
        public int DetailID { get; set; }
        public int InvoiceID { get; set; }
        public int ServiceID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    
        public virtual Invoice Invoice { get; set; }
        public virtual Service Service { get; set; }
    }
}
