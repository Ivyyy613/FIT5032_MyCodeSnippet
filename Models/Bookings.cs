//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FIT5032_EasyX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Bookings
    {
        public int Booking_Id { get; set; }
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Booking_Date { get; set; }
        [Display(Name = "Booking Content")]
        [Required(ErrorMessage = "Booking content cannot be empty.")]
        public string Booking_Content { get; set; }
        [Display(Name = "Is confirmed by doctor")]

        public bool Booking_IsConfirm { get; set; }
        [Display(Name = "Doctor Email")]

        public string DoctorId { get; set; }
        [Display(Name = "Patient Email")]
        public string PatientId { get; set; }
        public int Rating { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}