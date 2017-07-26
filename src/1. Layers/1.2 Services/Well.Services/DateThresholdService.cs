namespace PH.Well.Services
{
    using System;
    using Repositories.Contracts;
    using System.Linq;
    using Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DateThresholdService : IDateThresholdService
    {
        private readonly ISeasonalDateRepository seasonalDate;
        private readonly IDateThresholdRepository dateThresholdRepository;
        private readonly ICustomerRoyaltyExceptionRepository customerRoyaltyExceptionRepository;
        private CustomerRoyaltyException[] customerRoyaltyExceptions;

        public const string ErrorMessage = "Date Threshold is not defined for branch {0}";

        public DateThresholdService(
            ISeasonalDateRepository seasonalDate,
            IDateThresholdRepository dateThresholdRepository,
            ICustomerRoyaltyExceptionRepository customerRoyaltyExceptionRepository)
        {
            this.seasonalDate = seasonalDate;
            this.dateThresholdRepository = dateThresholdRepository;
            this.customerRoyaltyExceptionRepository = customerRoyaltyExceptionRepository;
        }

        public DateTime RouteGracePeriodEnd(DateTime routeDate, int branchId)
        {
            var branch = GetBranchDateThreshold(branchId);
            return GetGracePeriodEndDate(routeDate, branch.NumberOfDays, branchId);
        }

        public async Task<DateTime> RouteGracePeriodEndAsync(DateTime routeDate, int branchId)
        {
            var branch = await GetBranchDateThresholdAsync(branchId);
            return await GetGracePeriodEndDateAsync(routeDate, branch.NumberOfDays, branchId);
        }

        public DateTime GracePeriodEnd(DateTime routeDate, int branchId, int royaltyCode)
        {
            var branch = GetBranchDateThreshold(branchId);
            var gracePeriodDays = branch.NumberOfDays;
            var customerRoyaltyException = GetCustomerRoyaltyException(royaltyCode);

            if (customerRoyaltyException != null && customerRoyaltyException.ExceptionDays > gracePeriodDays)
            {
                gracePeriodDays = customerRoyaltyException.ExceptionDays;
            }

            return GetGracePeriodEndDate(routeDate, gracePeriodDays, branchId);
        }

        public async Task<DateTime> GracePeriodEndAsync(DateTime routeDate, int branchId, int royaltyCode)
        {
            var branch = await GetBranchDateThresholdAsync(branchId);
            var gracePeriodDays = branch.NumberOfDays;
            var customerRoyaltyException = GetCustomerRoyaltyException(royaltyCode);

            if (customerRoyaltyException != null && customerRoyaltyException.ExceptionDays > gracePeriodDays)
            {
                gracePeriodDays = customerRoyaltyException.ExceptionDays;
            }

            return await GetGracePeriodEndDateAsync(routeDate, gracePeriodDays, branchId);
        }

        private async Task<DateTime> GetGracePeriodEndDateAsync(DateTime routeDate, byte gracePeriodDays, int branchId)
        {
            var endDate = routeDate.Date.AddDays(gracePeriodDays).Date;
            var values = await this.GetSeasonalDatesAsync(branchId);

            return endDate.AddDays(AddNonWorkingDays(values, routeDate, endDate));
        }

        private DateTime GetGracePeriodEndDate(DateTime routeDate, byte gracePeriodDays, int branchId)
        {
            var endDate = routeDate.Date.AddDays(gracePeriodDays).Date;

            return endDate.AddDays(AddNonWorkingDays(GetSeasonalDates(branchId), routeDate, endDate));
        }

        private Task<IEnumerable<SeasonalDate>> GetSeasonalDatesAsync(int branchId)
        {
            return this.seasonalDate.GetByBranchIdAsync(branchId);
        }

        private IEnumerable<SeasonalDate> GetSeasonalDates(int branchId)
        {
            return this.seasonalDate.GetByBranchId(branchId);
        }

        private int AddNonWorkingDays(IEnumerable<SeasonalDate> dates, DateTime start, DateTime end)
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
            return dates
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

        private async Task<DateThreshold> GetBranchDateThresholdAsync(int branchId)
        {
            var all = await this.dateThresholdRepository.GetAsync();
            var branch = all.FirstOrDefault(p => p.BranchId == branchId);

            if (branch == null)
            {
                throw new Exception(string.Format(ErrorMessage, branchId));
            }

            return branch;
        }

        private DateThreshold GetBranchDateThreshold(int branchId)
        {
            var branch = this.dateThresholdRepository.Get().FirstOrDefault(p => p.BranchId == branchId);

            if (branch == null)
            {
                throw new Exception(string.Format(ErrorMessage, branchId));
            }

            return branch;
        }

        private CustomerRoyaltyException GetCustomerRoyaltyException(int royaltyCode)
        {
            if (customerRoyaltyExceptions == null)
            {
                customerRoyaltyExceptions = customerRoyaltyExceptionRepository.GetCustomerRoyaltyExceptions().ToArray();
            }

            return customerRoyaltyExceptions.FirstOrDefault(x => x.RoyaltyCode == royaltyCode);
        }
    }
}