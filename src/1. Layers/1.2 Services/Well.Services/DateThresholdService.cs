namespace PH.Well.Services
{
    using System;
    using Repositories.Contracts;
    using System.Linq;

    public class DateThresholdService : IDateThresholdService
    {
        private readonly ISeasonalDateRepository seasonalDate;
        private readonly IDateThresholdRepository dateThresholdRepository;
        public const string ErrorMessage = "Date Threshold is not defined for branch {0}";

        public DateThresholdService(ISeasonalDateRepository seasonalDate,
            IDateThresholdRepository dateThresholdRepository)
        {
            this.seasonalDate = seasonalDate;
            this.dateThresholdRepository = dateThresholdRepository;
        }

        public DateTime EarliestSubmitDate(DateTime routeDate, int branchId)
        {
            var branch = this.dateThresholdRepository.Get().FirstOrDefault(p => p.BranchId == branchId);

            if (branch == null)
            {
                throw new Exception(string.Format(ErrorMessage, branchId));
            }

            var endDate = routeDate.Date.AddDays(branch.NumberOfDays).Date;

            return endDate.AddDays(AddNonWorkingDays(routeDate, endDate, branchId));
        }

        private int AddNonWorkingDays(DateTime start, DateTime end, int branchId)
        {
            /* possible scenarios */
            /*

            it starts before the period but ends within
            _______________
            |              |
                _______________________
                1Day                  5Day

            it starts and ends during the period
               ____________
               |           |
            _______________________
            1Day                  5Day

            starts during the period and ends after it finish
                           _______________
                           |              |
            _______________________
            1Day                  5Day
            */
            return this.seasonalDate.GetByBranchId(branchId)
                .Where(p => (p.From.Date >= start && p.From.Date <= end)
                         || (p.To.Date >= start && p.To.Date <= end))
                .Select(p => new
                {
                    from = p.From.Date > start.Date ? p.From.Date : start.Date,
                    to = (p.To.Date > end.Date ? end.Date : p.To.Date).AddDays(1).Date 
                    //add one day otherwise a single holiday day will be 0 days
                    //example: 10-10-2008 to 10-10-2008 is 0 days but i need to count it as 1
                    //the same goes for 10-10-2008 to 15-10-2008 it be 5 but i need to count it as 6
                })
                .Sum(p => (p.to - p.from).Days);
        }
    }
}