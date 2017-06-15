using PH.Well.Domain;
using PH.Well.Domain.Enums;

namespace PH.Well.Services
{
    public interface IJobResolutionStatus
    {
        ResolutionStatus GetStatus(Job job);
    }
}