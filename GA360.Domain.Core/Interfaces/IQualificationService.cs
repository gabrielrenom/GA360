using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;

namespace GA360.Domain.Core.Interfaces;

public interface IQualificationService
{
    Qualification GetQualification(int id);
    Task<List<Qualification>> GetAllQualifications();
    Task<List<Qualification>> GetAllQualificationsByEmail(string email);
    Task<List<QualificationTrainingModel>> GetAllQualificationsByTrainingCentreId(int trainingCentreId);
    Task<Qualification> AddQualification(Qualification qualification);
    Task<Qualification> UpdateQualification(Qualification qualification);
    void DeleteQualification(int id);
    Task<List<QualificationStatus>> GetAllQualificationsStatus();
    Task<List<QualificationWithTrainingModel>> GetAllQualificationsWithTrainingCentres(int? id = null);
    Task<QualificationWithTrainingModel> AddQualification(QualificationWithTrainingModel qualification);
    Task<QualificationWithTrainingModel> UpdateQualification(QualificationWithTrainingModel qualification);
    Task<List<Qualification>> GetQualificationsByTrainingCentreWithEmail(string email);
    Task<List<Qualification>> GetAllQualificationsByCandidateId(int id);
    Task<List<QualificationLearnerModel>> GetAllDetailedQualificationsByCandidateId(int id);
}
