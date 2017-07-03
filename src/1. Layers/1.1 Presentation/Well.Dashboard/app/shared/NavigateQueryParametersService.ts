import { Injectable, EventEmitter }                 from '@angular/core';
import * as _                                       from 'lodash';
import { Location }                                 from '@angular/common';
import { NavigateQueryParameters, DictionaryItem }  from './NavigateQueryParameters';
import { IOptionFilter }                            from './IOptionFilter';
import { DropDownItem }                             from '../shared/dropDownItem';

@Injectable()
export  class NavigateQueryParametersService {
    private static paramName = 'filter.';
    private static paramPage = 'pagenumber';
    private static paramSort = 'sort';

    public BrowserNavigation: EventEmitter<string> = new EventEmitter<string>();

    constructor(location: Location) {
        location.subscribe((value: any) => {
            this.BrowserNavigation.emit(value.url);
        });
    }

    private static Save(qs: any, value?: NavigateQueryParameters): void {

        if (value.HasSort()) {
            qs = _.extend(JSON.parse(JSON.stringify(qs)), {[this.paramSort]: value.Sort});
        }

        if (value.HasPageNumber()) {
            qs = _.extend(JSON.parse(JSON.stringify(qs)), {[this.paramPage]: value.Page});
        }

        NavigateQueryParametersService.SaveHistory(qs);
    }

    public static SaveSort(value?: NavigateQueryParameters): void {

        let qs = NavigateQueryParametersService.GetQueryStringObject();

        qs = NavigateQueryParametersService.DeleteQueryStringParameter(qs, NavigateQueryParametersService.paramSort);
        qs = NavigateQueryParametersService.DeleteQueryStringParameter(qs, NavigateQueryParametersService.paramPage);
        this.Save(qs, value);
    }

    public static SaveFilter(value?: NavigateQueryParameters): void {

        let qs = NavigateQueryParametersService.GetQueryStringObject();
        qs = NavigateQueryParametersService.DeleteQueryStringParameter(
            qs,
            NavigateQueryParametersService.GetActiveFilterParameterName(qs));
        qs = NavigateQueryParametersService.DeleteQueryStringParameter(
            qs,
            NavigateQueryParametersService.paramPage);
        qs = NavigateQueryParametersService.DeleteQueryStringParameter(
            qs,
            NavigateQueryParametersService.paramSort);

        if (value.HasFilter()) {
            const proName = _.keys(value.Filter)[0];
            const newParam = new DictionaryItem();

            newParam[this.paramName + proName] = value.Filter[proName];
            qs = _.extend(JSON.parse(JSON.stringify(qs)), newParam);
        }

        NavigateQueryParametersService.SaveHistory(qs);
    }

    public static SavePageNumber(value?: NavigateQueryParameters): void {

        let qs = NavigateQueryParametersService.GetQueryStringObject();

        qs = NavigateQueryParametersService.DeleteQueryStringParameter(qs, NavigateQueryParametersService.paramPage);
        this.Save(qs, value);
    }

    private static SaveHistory(queryStringObject: any): void {

        const newqs = NavigateQueryParametersService.SerializeQueryParams(queryStringObject);
        const newurl = window.location.protocol + '//' + window.location.host + window.location.pathname + newqs;

        window.history.pushState({path: newurl}, '', newurl);
    }

    private static DeleteQueryStringParameter(qs: any, paramName: string) {

        if (!_.isUndefined(qs) && !_.isUndefined(paramName)) {
            delete qs[paramName];
        }

        return qs;
    }

    public Navigate(optionFilter: IOptionFilter): void {

        const qs = NavigateQueryParametersService.GetQueryStringObject();
        let selectedOption = DropDownItem.CreateDefaultOption();
        let filterText = '';
        let currentPage = 1;
        let currentSort = '';

        if (!_.isUndefined(qs)) {
            //lets get the filter
            const current = NavigateQueryParametersService.GetActiveFilterParameterName(qs);

            if (current.length > 0) {
                //remove the prefix
                const currentNoPrefix = current.split('.')[1];
                //search in the options array if the current filter exists
                const opt = _.find(
                    optionFilter.options,
                    (o: DropDownItem) => o.value.toLowerCase() == currentNoPrefix.toLowerCase());
                if (!_.isUndefined(opt)) {
                    //i found it, so now lets save the values
                    selectedOption = opt;
                    filterText = qs[current];
                }
            }

            //now lets get the page
            currentPage = NavigateQueryParametersService.GetCurrentPage(qs);

            // and finally lets get the sort
            currentSort = NavigateQueryParametersService.GetCurrentSort(qs);
        }

        optionFilter.filterOption.filterText = filterText;
        optionFilter.filterOption.dropDownItem = selectedOption;

        optionFilter.selectedFilter = filterText;
        optionFilter.selectedOption = selectedOption;
        optionFilter.currentPage = currentPage;
        optionFilter.sortDirection = currentSort;
    }

    private static GetQueryStringObject() {

        const url = window.location.href;

        // get query string from url (optional) or window
        let queryString = url ? url.split('?')[1] : window.location.search.slice(1);

        // we'll store the parameters here
        const obj = {};

        // if query string exists
        if (queryString) {

            // stuff after # is not part of query string, so get rid of it
            queryString = queryString.split('#')[0];

            // split our query string into its component parts
            const arr = queryString.split('&');

            for (let i = 0; i < arr.length; i++) {
                // separate the keys and the values
                const a = arr[i].split('=');

                // in case params look like: list[]=thing1&list[]=thing2
                let paramNum = '';
                let paramName = '';

                paramName = a[0].replace(/\[\d*\]/, v => {
                    paramNum = v.slice(1, -1);
                    return '';
                });

                // set parameter value (use 'true' if empty)
                let paramValue = typeof(a[1]) === 'undefined' ? 'true' : a[1];

                // (optional) keep case consistent
                paramName = decodeURIComponent(paramName.toLowerCase());
                paramValue = paramValue.toLowerCase();

                // if parameter name already exists
                if (obj[paramName]) {
                    // convert value to array (if still string)
                    if (typeof obj[paramName] === 'string') {
                        obj[paramName] = [obj[paramName]];
                    }
                    // if no array index number specified...
                    if (typeof paramNum === 'undefined') {
                        // put the value on the end of the array
                        obj[paramName].push(paramValue);
                    }
                    // if array index number specified...
                    else {
                        // put the value at that index number
                        obj[paramName][paramNum] = paramValue;
                    }
                }
                // if param name doesn't exist yet, set it
                else {
                    obj[paramName] = paramValue;
                }
                obj[paramName] = decodeURIComponent(obj[paramName]);
            }
        }

        return obj;
    }

    private static GetActiveFilterParameterName(qs: any): string {

        const current = _.chain(qs)
            .keys()
            .filter(p =>  p.startsWith(NavigateQueryParametersService.paramName))
            .value();

        if (current.length > 0) {
            return current[0];
        }

        return '';
    }

    private static GetCurrentPage(qs: any): number {

        const current = _.chain(qs)
            .keys()
            .filter(p => p.toLowerCase() == NavigateQueryParametersService.paramPage)
            .first()
            .value();

        if (current) {
            return parseInt(qs[current], 10);
        }

        return 1;
    }

    private static GetCurrentSort(qs: any): string
    {
        return qs[NavigateQueryParametersService.paramSort] || 'desc';
    }

    public static GetCurrentSorting(): string
    {
        const qs = NavigateQueryParametersService.GetQueryStringObject();

        return this.GetCurrentSort(qs);
    }

    private static SerializeQueryParams(params: any): string {

        const result = _.chain(params)
            .keys()
            .map((p: string) => {
                return this.Encode(p) + '=' + this.Encode(params[p]);
            })
            .value();

        return result.length > 0 ? '?' + result.join('&') : '';
    }

    private static Encode(value: string): string {
        //to avoid encode 2 times the same value
        return decodeURIComponent(encodeURIComponent(value));
    }
}