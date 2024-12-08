using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;
using System.Runtime.CompilerServices;

namespace GA360.Server.ViewModels
{
    public class CustomersWithCourseQualificationRecordsViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public int? QualificationId { get; set; }
        public string QualificationName { get; set; }
        public int? CertificateId { get; set; }
        public string CertificateName { get; set; }
        public int? TrainingCentreId { get; set; }
        public string TrainingCentre { get; set; }
        public int Progression { get; set; }
        public int? QualificationStatusId { get; set; }
        public string QualificationStatus {  get; set; }
    }

    public static class CustomersWithCourseQualificationRecordsMapper
    {
    
        public static List<CustomersWithCourseQualificationRecordsViewModel> ToCustomersWithCourseQualificationRecordsViewModel(this Customer customer)
        {
            var customerViewModel = customer.QualificationCustomerCourseCertificates != null ? customer.QualificationCustomerCourseCertificates.Select(x => new CustomersWithCourseQualificationRecordsViewModel
            {
                Id = x.Id,
                CertificateId = x.CertificateId,
                CertificateName = x.Certificate!=null?x.Certificate.Name:string.Empty,
                QualificationId = x.QualificationId,
                QualificationName = x.Qualification!=null?x.Qualification.Name:string.Empty,
                CourseId = x.CourseId,
                CourseName = x.Course !=null? x.Course.Name:string.Empty,
                Email = customer.Email,
                CustomerId = customer.Id,
                Progression = x.CourseProgression,
                TrainingCentre = customer.TrainingCentre!=null?customer.TrainingCentre.Name:string.Empty,
                TrainingCentreId = customer.TrainingCentreId,
                QualificationStatus = x.QualificationStatus!=null?x.QualificationStatus.Name:string.Empty,
                QualificationStatusId = x.QualificationStatus != null ? x.QualificationStatus.Id : null,
            }).ToList() : new List<CustomersWithCourseQualificationRecordsViewModel>();

            return customerViewModel;
        }

        public static CustomersWithCourseQualificationRecordsModel ToCustomersWithCourseQualificationRecordsModel(this CustomersWithCourseQualificationRecordsViewModel viewModel)
        {
            return new CustomersWithCourseQualificationRecordsModel
            {
                Id = viewModel.Id,
                CustomerId = viewModel.CustomerId,
                Email = viewModel.Email,
                CourseId = viewModel.CourseId,
                CourseName = viewModel.CourseName,
                QualificationId = viewModel.QualificationId,
                QualificationName = viewModel.QualificationName,
                CertificateId = viewModel.CertificateId,
                CertificateName = viewModel.CertificateName,
                TrainingCentreId = viewModel.TrainingCentreId,
                TrainingCentre = viewModel.TrainingCentre,
                Progression = viewModel.Progression,
                QualificationStatus = viewModel.QualificationStatus,
                QualificationStatusId= viewModel.QualificationStatusId,
            };
        }
    }
}
