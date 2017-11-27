namespace PH.Well.Domain.Enums
{
    public enum EventAction
    {
        // values 1 to 11 correspond to the transaction record type required by ADAM
        // commented out the ones that are not used so far because they may need renaming
        Credit = 1,
        //CreditAndReorder = 2,
        // = 3,
        Grn = 4,
        Pod = 5,
        // 
        //ReplanInRoadnet = 6,
        //ReplanInTranSend = 7,
        //ReplanInTheQueue = 8,
        StandardUplift = 8, 
        Amendment = 9,
        GlobalUplift = 10,
        RecirculateDocuments = 11,
        PodTransaction = 20

    }
}