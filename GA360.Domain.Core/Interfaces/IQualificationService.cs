﻿using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;

namespace GA360.Domain.Core.Interfaces;

public interface IQualificationService
{
    Qualification GetQualification(int id);
    Task<List<Qualification>> GetAllQualifications();
    Task<List<QualificationTrainingModel>> GetAllQualificationsByTrainingCentreId(int trainingCentreId);
    Task<Qualification> AddQualification(Qualification qualification);
    Task<Qualification> UpdateQualification(Qualification qualification);
    void DeleteQualification(int id);
    Task<List<QualificationStatus>> GetAllQualificationsStatus();
}
