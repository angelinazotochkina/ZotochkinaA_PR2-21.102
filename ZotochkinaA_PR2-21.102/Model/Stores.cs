//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZotochkinaA_PR2_21._102.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stores
    {
        public Stores()
        {
            this.Sales = new HashSet<Sales>();
            this.TaxAuthorities = new HashSet<TaxAuthorities>();
            this.Users = new HashSet<Users>();
        }
    
        public int IDStore { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string Comment { get; set; }
    
        public virtual ICollection<Sales> Sales { get; set; }
        public virtual ICollection<TaxAuthorities> TaxAuthorities { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}