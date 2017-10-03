export class AppSearchParameters
{
    public branchId?: number;
    public date?: Date;
    public account: string;
    public invoice: string;
    public route: string;
    public driver: string;
    public deliveryType?: number;
    public status?: number;
    public routeIds: number[];
    public upliftInvoiceNumber: string;
}