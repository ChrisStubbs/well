namespace PH.Well.Domain.Enums
{
    public enum JobStatus
    {
        /// <summary>
        /// Initial state of a job when received from ADAM
        /// </summary>
        AwaitingInvoice = 1,
        /// <summary>
        /// Invoice received from ADAM update or Transend
        /// </summary>
        InComplete = 2,
        /// <summary>
        /// Transend update containing zero shorts and damages and no split between invoice where
        /// delivered quatity is greater than the invoiced quantity
        /// </summary>
        Clean = 3,
        /// <summary>
        /// Transend update containing any shorts or damages or invoice splits
        /// </summary>
        Exception = 4,
        /// <summary>
        /// Exception has been fully actioned (credit, closed)
        /// </summary>
        Resolved = 5
    }
}