//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PropertyRentalManagementMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [DisplayName("Message Title")]
        public string MessageTitle { get; set; }

        [Required]
        [DisplayName("Message Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime MessageDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [DisplayName("Manager Id")]
        public int ManagerId { get; set; }

        [Required]
        [DisplayName("Tenant Name")]
        public int TenantId { get; set; }
    
        public virtual Manager Manager { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}