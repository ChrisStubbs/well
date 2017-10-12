import { IAppSearchItem } from './IAppSearchItem';
export class AppSearchInvoiceItem implements IAppSearchItem {
    public itemType: number;
    public id: number;
    public type: string;
    public documentNumber: string;
    public locationName: string;
    public date: string;
}