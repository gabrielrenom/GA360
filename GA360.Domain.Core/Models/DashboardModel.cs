using GA360.Commons.Helpers;
using System.ComponentModel;
using System.Reflection;

namespace GA360.Domain.Core.Models;


public enum StatisticType
{
    [Description("NEW_LEADS")]
    NEW_LEADS = 1,

    [Description("NEW_LEARNERS")]
    NEW_LEARNERS = 2,

    [Description("ACTIVE_LEARNERS")]
    ACTIVE_LEARNERS = 3,

    [Description("COMPLETED_LEARNERS")]
    COMPLETED_LEARNERS = 4,

    [Description("CANDIDATE_REGISTRATIONS")]
    CANDIDATE_REGISTRATIONS = 5,

    [Description("TRAINING_CENTRES_STATS")]
    TRAINING_CENTRES_STATS = 6
}

public class DashboardModel
{
    private StatisticType _statisticType;

    public StatisticType StatisticType
    {
        get => _statisticType;
        set
        {
            _statisticType = value;
            StatisticTypeDescription = _statisticType.GetDescription();
        }
    }

    public string StatisticTypeDescription { get; private set; }
    public decimal Percentage { get; set; }
    public decimal TotalYear { get; set; }
    public decimal Total { get; set; }
}


