namespace TravelExpress.Entities.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddUpdateCustomerRequest
    {
        [Display(Name = nameof(Resources.Resource.Name_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Name_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceName = "Name_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Display(Name = nameof(Resources.Resource.FamilyName1_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "FamilyName1_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceName = "FamilyName1_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string FamilyName1 { get; set; }

        [Display(Name = nameof(Resources.Resource.FamilyName2_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "FamilyName2_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceName = "FamilyName2_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string FamilyName2 { get; set; }

        [Display(Name = nameof(Resources.Resource.Telephone_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Telephone_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(30, MinimumLength = 1, ErrorMessageResourceName = "Telephone_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Telephone { get; set; }

        [Display(Name = nameof(Resources.Resource.Cellphone_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Cellphone_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(30, MinimumLength = 1, ErrorMessageResourceName = "Cellphone_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Cellphone { get; set; }

        [Display(Name = nameof(Resources.Resource.Email_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Email_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(254, MinimumLength = 1, ErrorMessageResourceName = "Email_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "Email_Invalid", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

        [Display(Name = nameof(Resources.Resource.BirthDate_Display), ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "BirthDate_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime BirthDate { get; set; }

        [Display(Name = nameof(Resources.Resource.PhoneCallsEnabled_Display), ResourceType = typeof(Resources.Resource))]
        [Required]
        public bool PhoneCallsEnabled { get; set; }

        [Display(Name = nameof(Resources.Resource.MailEnabled_Display), ResourceType = typeof(Resources.Resource))]
        [Required]
        public bool MailEnabled { get; set; }
    }
}