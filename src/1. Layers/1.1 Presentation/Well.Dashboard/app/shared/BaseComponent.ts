import { OnInit, OnDestroy }                from '@angular/core';
import { NavigateQueryParametersService }   from './NavigateQueryParametersService';
import { NavigateQueryParameters }          from './NavigateQueryParameters';
import { IOptionFilter, INavigationPager }  from './IOptionFilter';
import { FilterOption }                     from './filterOption';
import { DropDownItem }                     from './dropDownItem';
 
export abstract class BaseComponent implements OnInit, IOptionFilter, OnDestroy, INavigationPager {
    public options: DropDownItem[];
    public filterOption: FilterOption = new FilterOption();
    public selectedOption: DropDownItem;
    public selectedFilter: string;
    public currentPage: number;
    private navigationSubscriber: any;
    public readonly rowCount: number = 10;

    constructor(private navigateQueryParametersService: NavigateQueryParametersService) {}

    public onSortDirectionChanged(isDesc: boolean): void {
        const item = new NavigateQueryParameters(undefined, 1, isDesc ? 'desc' : 'asc');
        NavigateQueryParametersService.SaveSort(item);
        this.navigateQueryParametersService.Navigate(this);
    }

    public ngOnDestroy(): void {
        this.navigationSubscriber.unsubscribe();
    }

    public onFilterClicked(filterOption: FilterOption)  {
        this.filterOption = filterOption;
        this.navigateQueryParametersService.Navigate(this);
    };

    public ngOnInit(): void {
        this.navigateQueryParametersService.Navigate(this);

        this.navigationSubscriber = this.navigateQueryParametersService.BrowserNavigation
            .subscribe(p => this.navigateQueryParametersService.Navigate(this));
    }

    public SetCurrentPage(pageNumber: number): void {
        this.currentPage = pageNumber

        const item = new NavigateQueryParameters(undefined, this.currentPage);
        NavigateQueryParametersService.SavePageNumber(item);
    }
}