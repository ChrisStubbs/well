namespace PH.Well.Domain.Enums
{
    public enum ExceptionAction
    {
        Credit = 1,

        CreditAndReorder = 2,

        ReplanInRoadnet = 3,

        ReplanInTranSend = 4,

        ReplanInTheQueue = 5,

        Reject = 6
    }
}