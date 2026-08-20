using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServicePlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
   //         ProcurementApprovalUserMappings = new HashSet<ProcurementApprovalUserMapping>();
   //         IndentMasters = new HashSet<IndentMaster>();
			//IndentApprovalHistories = new HashSet<IndentApprovalHistory>();
            PageAccesses = new HashSet<PageAccessLog>();
			UserPageAuthorizations = new HashSet<UserPageAuthorization>();
            ClickEventAuthorizations = new HashSet<ClickEventAuthorization>();
			POS_Invoices = new HashSet<POS_Invoice>();
            UserMenuAccesses = new HashSet<UserMenuAccess>();
            UserCreationLinks = new HashSet<UserCreationLink>();
		}


		public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }

        public bool? IsDeactivated { get; set; }
        public DateTime? DeactivationDateTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }



        // Collection navigation containing dependents
  //      public virtual ICollection<ProcurementApprovalUserMapping> ProcurementApprovalUserMappings { get; set; } = new List<ProcurementApprovalUserMapping>();
  //      public virtual ICollection<IndentMaster> IndentMasters { get; set; } = new List<IndentMaster>();
		//public virtual ICollection<IndentApprovalHistory> IndentApprovalHistories { get; set; } = new List<IndentApprovalHistory>();
        public virtual ICollection<PageAccessLog> PageAccesses { get; set; } = new List<PageAccessLog>();
        public virtual ICollection<UserPageAuthorization> UserPageAuthorizations { get; set; } = new List<UserPageAuthorization>();
        public virtual ICollection<ClickEventAuthorization> ClickEventAuthorizations { get; set; } = new List<ClickEventAuthorization>();
		public virtual ICollection<POS_Invoice> POS_Invoices { get; set; } = new List<POS_Invoice>();
        public virtual ICollection<UserMenuAccess> UserMenuAccesses { get; set; } = new List<UserMenuAccess>();
		public virtual ICollection<UserCreationLink> UserCreationLinks { get; set; } = new List<UserCreationLink>();
		public virtual ICollection<POS_PackingToRetailTransfer> POS_PackingToRetailTransfers { get; set; } = new List<POS_PackingToRetailTransfer>();
		//public virtual ICollection<ArticlePicture> ArticlePictures { get; set; } = new List<ArticlePicture>();
		public virtual ICollection<POS_RetailPrice> POS_RetailPrices { get; set; } = new List<POS_RetailPrice>();


	}

}

