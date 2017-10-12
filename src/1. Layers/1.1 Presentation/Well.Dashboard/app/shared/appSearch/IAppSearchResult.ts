import { IAppSearchResultItem} from './IAppSearchResultItem';
export interface IAppSearchResult {
    branchId: number;
    items: IAppSearchResultItem[];
}