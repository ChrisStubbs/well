import { IAppSearchItem } from './IAppSearchItem';
export class AppSearchLocationItem implements IAppSearchItem {
    public itemType: number;
    public id: number;
    public name: string;
    public accountNumber: string;
}