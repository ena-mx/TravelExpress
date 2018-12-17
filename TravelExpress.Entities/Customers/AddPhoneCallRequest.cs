namespace TravelExpress.Entities.Customers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AddPhoneCallRequest
    {
        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(254, MinimumLength = 1, ErrorMessageResourceName = "PhoneCallTextStringLenghtValidation", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }
    }

    public class AddObservationRequest
    {
        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(254, MinimumLength = 1, ErrorMessageResourceName = "ObservationTextStringLenghtValidation", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }
    }
}
