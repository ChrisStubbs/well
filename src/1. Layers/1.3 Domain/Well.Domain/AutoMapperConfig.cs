using AutoMapper;

namespace PH.Well.Domain.Mappers
{
    public static class AutoMapperConfig 
    {
        private static IMapper config;
        public static IMapper Mapper
        {
            get
            {
                return config;
            }
        }

        static AutoMapperConfig()
        {
            ConfigureMappings();
        }

        private static void ConfigureMappings()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountDTO, Account>();
                cfg.CreateMap<Account, AccountDTO>();

                cfg.CreateMap<JobDTO, Job>();
                cfg.CreateMap<Job, JobDTO>()
                .ForMember(m => m.OrdOuters, p => p.ResolveUsing((s, d) =>
                {
                    if (s.OrdOuters.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "ORDOUTERS", Value = s.OrdOuters.ToString() });

                    return s.OrdOuters;
                }))
                .ForMember(m => m.InvOuters, p => p.ResolveUsing((s, d) =>
                {
                    if (s.InvOuters.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "INVOUTERS", Value = s.InvOuters.ToString() });

                    return s.InvOuters;
                }))
                .ForMember(m => m.ColOuters, p => p.ResolveUsing((s, d) =>
                {
                    if (s.ColOuters.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "COLOUTERS", Value = s.ColOuters.ToString() });

                    return s.ColOuters;
                }))
                .ForMember(m => m.ColBoxes, p => p.ResolveUsing((s, d) =>
                {
                    if (s.ColBoxes.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "COLBOXES", Value = s.ColBoxes.ToString() });

                    return s.ColBoxes;
                }))
                .ForMember(m => m.ReCallPrd, p => p.ResolveUsing((s, d) =>
                {
                    if (s.ReCallPrd)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "RECALLPRD", Value = "Y" });

                    return true;
                }))
                .ForMember(m => m.AllowSoCrd, p => p.ResolveUsing((s, d) =>
                {
                    if (s.AllowSoCrd)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "ALLOWSOCRD", Value = "Y" });

                    return true;
                }))
                .ForMember(m => m.Cod, p => p.ResolveUsing((s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.Cod))
                        d.EntityAttributes.Add(new EntityAttribute { Code = "COD", Value = s.Cod });

                    return string.Empty;
                }))
                .ForMember(m => m.SandwchOrd, p => p.ResolveUsing((s, d) =>
                {
                    if (s.SandwchOrd)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "SandwchOrd", Value = "Y" });

                    return true;
                }))
                .ForMember(m => m.AllowReOrd, p => p.ResolveUsing((s, d) =>
                {
                    if (s.AllowReOrd)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "AllowReOrd", Value = "Y" });

                    return true;
                }))
                .ForMember(m => m.ActionLogNumber, p => p.ResolveUsing((s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.ActionLogNumber))
                        d.EntityAttributes.Add(new EntityAttribute { Code = "ACTLOGNO", Value = s.ActionLogNumber });

                    return string.Empty;
                }))
                .ForMember(m => m.GrnNumber, p => p.ResolveUsing((s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.GrnNumber))
                        d.EntityAttributes.Add(new EntityAttribute { Code = "GRNNO", Value = s.GrnNumber });

                    return string.Empty;
                }))
                .ForMember(m => m.GrnRefusedReason, p => p.ResolveUsing((s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.GrnRefusedReason))
                        d.EntityAttributes.Add(new EntityAttribute { Code = "GRNREFREAS", Value = s.GrnRefusedReason });

                    return string.Empty;
                }))
                .ForMember(m => m.ColBoxes, p => p.ResolveUsing((s, d) =>
                {
                    if (s.ColBoxes.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "COLBOXES", Value = s.ColBoxes.ToString() });

                    return s.ColBoxes;
                }))
                .ForMember(m => m.OuterCount, p => p.ResolveUsing((s, d) =>
                {
                    if (s.OuterCount.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "OUTERCOUNT", Value = s.OuterCount.ToString() });

                    return s.OuterCount;
                }))
                .ForMember(m => m.TotalOutersOver, p => p.ResolveUsing((s, d) =>
                {
                    if (s.TotalOutersOver.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "TOTOVER", Value = s.TotalOutersOver.ToString() });

                    return s.TotalOutersOver;
                }))
                .ForMember(m => m.TotalOutersShort, p => p.ResolveUsing((s, d) =>
                {
                    if (s.TotalOutersShort.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "TOTSHORT", Value = s.TotalOutersShort.ToString() });

                    return s.TotalOutersShort;
                }))
                .ForMember(m => m.DetailOutersOver, p => p.ResolveUsing((s, d) =>
                {
                    if (s.DetailOutersOver.HasValue)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "DETOVER", Value = s.DetailOutersOver.ToString() });

                    return s.DetailOutersOver;
                }))
                .ForMember(m => m.Picked, p => p.ResolveUsing((s, d) =>
                {
                    if (s.Picked)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "AllowReOrd", Value = "Y" });

                    return true;
                }))
                .ForMember(m => m.InvoiceValue, p => p.ResolveUsing((s, d) =>
                {
                    d.EntityAttributes.Add(new EntityAttribute { Code = "INVALUE", Value = s.InvoiceValue.ToString() });

                    return s.InvoiceValue;
                }))
                .ForMember(m => m.JobDetails, c => c.MapFrom(m => m.JobDetails));

                cfg.CreateMap<JobDetailDamage, JobDetailDamageDTO>()
                .ForMember(m => m.JobDetailReason, c => c.Ignore())
                .ForMember(m => m.JobDetailSource, c => c.Ignore())
                .ForMember(m => m.Source, c => c.MapFrom(m => m.Source))
                .ForMember(m => m.Reason, c => c.MapFrom(m => m.Reason));

                cfg.CreateMap<JobDetailDamageDTO, JobDetailDamage>()
                .ForMember(m => m.JobDetailReason, c => c.Ignore())
                .ForMember(m => m.JobDetailSource, c => c.Ignore())
                .ForMember(m => m.Source, c => c.MapFrom(m => m.Source))
                .ForMember(m => m.Reason, c => c.MapFrom(m => m.Reason));

                cfg.CreateMap<JobDetailDTO, JobDetail>()
                .ForMember(m => m.JobDetailDamages, c => c.MapFrom(m => m.JobDetailDamages))
                .ForMember(m => m.Actions, c => c.MapFrom(m => m.Actions))
                .ForMember(m => m.Actions, c => c.MapFrom(m => m.Actions));

                cfg.CreateMap<JobDetail, JobDetailDTO>()
                .ForMember(m => m.JobDetailDamages, c => c.MapFrom(m => m.JobDetailDamages))
                .ForMember(m => m.Actions, c => c.MapFrom(m => m.Actions))
                .ForMember(m => m.Actions, c => c.MapFrom(m => m.Actions));

                cfg.CreateMap<StopDTO, Stop>();
                //.ForMember(m => m.AllowOvers, 
                //    p => p.ResolveUsing((s, d) => string.Equals("y", s.AllowOvers, System.StringComparison.CurrentCultureIgnoreCase)))
                //.ForMember(m => m.CustUnatt,
                //    p => p.ResolveUsing((s, d) => string.Equals("y", s.CustUnatt, System.StringComparison.CurrentCultureIgnoreCase)))
                //.ForMember(m => m.PHUnatt,
                //    p => p.ResolveUsing((s, d) => string.Equals("y", s.PHUnatt, System.StringComparison.CurrentCultureIgnoreCase)));

                cfg.CreateMap<Stop, StopDTO>()
                .ForMember(m => m.AllowOvers, p => p.ResolveUsing((s, d) =>
                    {
                        if (s.AllowOvers)
                            d.EntityAttributes.Add(new EntityAttribute { Code = "ALLOWOVERS", Value = "Y" });

                        return true;
                    }))
                .ForMember(m => m.CustUnatt, p => p.ResolveUsing((s, d) =>
                {
                    if (s.CustUnatt)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "CUSTUNATT", Value = "Y"});

                    return true;
                }))
                .ForMember(m => m.PHUnatt, p => p.ResolveUsing((s, d) =>
                {
                    if (s.PHUnatt)
                        d.EntityAttributes.Add(new EntityAttribute { Code = "PHUNATT", Value = "Y" });

                    return true;
                }))
                .ForMember(m => m.ActualPaymentCash, p => p.ResolveUsing((s, d) =>
                {
                    d.EntityAttributes.Add(new EntityAttribute { Code = "ACTPAYCASH", Value = d.ActualPaymentCash.ToString() });

                    return s.ActualPaymentCash;
                }))
                .ForMember(m => m.ActualPaymentCheque, p => p.ResolveUsing((s, d) =>
                {
                    d.EntityAttributes.Add(new EntityAttribute { Code = "ACTPAYCHEQ", Value = d.ActualPaymentCheque.ToString() });

                    return s.ActualPaymentCheque;
                }))
                .ForMember(m => m.ActualPaymentCard, p => p.ResolveUsing((s, d) =>
                {
                    d.EntityAttributes.Add(new EntityAttribute { Code = "ACTPAYCARD", Value = d.ActualPaymentCard.ToString() });

                    return s.ActualPaymentCard;
                }))
                .ForMember(m => m.AccountBalance, p => p.ResolveUsing((s, d) =>
                {
                    d.EntityAttributes.Add(new EntityAttribute { Code = "ACTPAYCARD", Value = d.AccountBalance.ToString() });

                    return s.AccountBalance;
                }))
                .ForMember(m => m.Jobs, c => c.MapFrom(m => m.Jobs))
                .ForMember(m => m.Account, c => c.MapFrom(m => m.Account));

            }).CreateMapper();
        }
    }
}
