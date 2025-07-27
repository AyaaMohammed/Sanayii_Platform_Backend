using System.ComponentModel.DataAnnotations;

namespace Sanayii.Enums
{
    public enum ServiceStatus
    {
        [Display(Name = "Service Requested")]
        Service_Requested = 1,

        [Display(Name = "In Progress")]
        In_Progress,

        [Display(Name = "Artisan on the Way")]
        Artisan_On_The_Way,

        [Display(Name = "Artisan Nearing Location")]
        Artisan_Nearing_Location,

        [Display(Name = "Artisan Arrived")]
        Artisan_Arrived,

        [Display(Name = "Service Undergoing")]
        Service_Undergoing,

        [Display(Name = "Service Completed")]
        Service_Completed,

        [Display(Name = "Service Cancelled")]
        Service_Cancelled,

        [Display(Name = "Awaiting Approval")]
        Awaiting_Approval,

        [Display(Name = "Artisan Busy")]
        Artisan_Busy,

        [Display(Name = "Service done Successfully, you Should complete payment method!")]
        Service_done_Successfully__you_Should_complete_payment_method,

        [Display(Name = "Service done Successfully, and payment done Successfully")]
        Service_done_Successfully__and_payment_done_Successfully,


        [Display(Name = "Service Failed")]
        Service_Failed

    }
}