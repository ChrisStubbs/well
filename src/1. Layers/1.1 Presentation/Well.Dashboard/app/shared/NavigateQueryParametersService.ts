import {Injectable}                                 from '@angular/core';
import { ActivatedRoute }                           from '@angular/router';
import * as _                                       from 'lodash';
import { NavigateQueryParameters, DictionaryItem }  from './NavigateQueryParameters';
import { IOptionFilter }                            from './IOptionFilter';
import {DropDownItem}                               from "../shared/dropDownItem";

@Injectable()
export  class NavigateQueryParametersService{
    private paramName = 'filter.';

    constructor(private activatedRoute: ActivatedRoute){}

    public Save(value?: NavigateQueryParameters): void {
        let qs = this.DeleteFilter();

        if (this.IsValidOptionFilter(value)) {
            const proName = _.keys(value)[0];
            const newParam = new DictionaryItem();
            newParam[this.paramName + proName] = value[proName];

            qs = _.extend(JSON.parse(JSON.stringify(qs)), newParam);
        }

        this.SaveHistory(qs);
    }

    private SaveHistory(queryStringObject: any): void{
        const newqs = this.serializeQueryParams(queryStringObject);
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + newqs;
        window.history.pushState({path: newurl}, '', newurl);
    }

    private DeleteFilter() {
        let qs = this.GetQueryStringObject();

        if (!_.isUndefined(qs)) {
            const current = this.GetActiveFilterParameterName(qs);

            if (current.length > 0) {
                delete qs[current];
            }
        }

        return qs;
    }

    private IsValidOptionFilter(value?: NavigateQueryParameters): boolean {

        if (value == null){
            return false;
        }

        var k = _.keys(value)[0];
        var v = value[k];

        return !(_.isNull(v) || _.isUndefined(v) || _.trim(v) == '');
    }

    public Navigate(optionFilter: IOptionFilter): void {
        let qs = this.GetQueryStringObject();
        let selectedOption = new DropDownItem("Option", "");
        let filterText = '';

        if (!_.isUndefined(qs)) {
            const current = this.GetActiveFilterParameterName(qs);

            if (current.length > 0){
                //remove the perfix
                const currentNoPrefix = current.split('.')[1];
                //search in the options array if the current filter exists
                const opt = _.find(optionFilter.options, (o: DropDownItem) => o.value == currentNoPrefix);
                 if (!_.isUndefined(opt)){
                     //i found it, so now lets save the values
                     selectedOption = opt;
                     filterText = qs[current];
                 }
            }
        }

        optionFilter.filterText = filterText;
        optionFilter.selectedOption = selectedOption;
        optionFilter.applyFilter();
    }

    private GetQueryStringObject() {
        let qs = {};
        this.activatedRoute.queryParams.subscribe( p => qs = p);

        return _.extend({}, qs);
    }

    private GetActiveFilterParameterName(qs: any): string{

        const current = _.chain(qs)
            .keys()
            .filter(p => { return p.startsWith(this.paramName)})
            .value();

        if (current.length > 0){
            return current[0];
        }

        return '';
    }

    private serializeQueryParams(params: any): string {

        let result = _.chain(params)
            .keys()
            .map((p: string) => {
                return this.encode(p) + "=" + this.encode(params[p]);
            })
            .value();

        return result.length > 0 ? "?" + result.join("&") : '';
    }

    private encode(value: string): string{
        //to avoid encode 2 times the same value
        return decodeURIComponent(encodeURIComponent(value))
    }
}
