using PH.Well.Domain;
using PH.Well.Domain.Enums;

namespace PH.Well.Services
{
    public interface IGetJobResolutionStatus
    {
        ResolutionStatus GetCurrentResolutionStatus(Job job);
        ResolutionStatus GetNextResolutionStatus(Job job);
    }
}