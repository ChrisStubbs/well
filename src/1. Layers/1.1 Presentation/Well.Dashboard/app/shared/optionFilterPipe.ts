import { PipeTransform, Pipe }  from '@angular/core';
import {FilterOption}           from './filterOption';
import * as moment              from 'moment/moment';

@Pipe({
    name: 'optionFilter'
})
export class OptionFilterPipe implements PipeTransform {
    public transform(value: any[], args: FilterOption): any[] {

        const filterOption: FilterOption = args; /*args[0] ? args[0] : null;*/

        if (!filterOption || filterOption.filterText == '') {
            return value;
        }
 
        if (filterOption.dropDownItem.type == 'date') {
            return value.filter((delivery: any) => this.filterDate(delivery, filterOption));
        } 
        else if (filterOption.dropDownItem.type == 'number') {
            return value.filter((delivery: any) => this.filterNumber(delivery, filterOption));
        } 
        
        else if (filterOption.dropDownItem.type == 'numberLessThanOrEqual') {
            return value.filter((delivery: any) => this.filterNumberLessThanOrEqual(delivery, filterOption));
        } 
        else {
            return value.filter((delivery: any) => this.filterString(delivery, filterOption));
        }
    }

    private filterString(list: any, filterOption: FilterOption) {
        if (list.hasOwnProperty(filterOption.dropDownItem.value)) {
            const propertyValue = list[filterOption.dropDownItem.value].toString().toLocaleLowerCase();
            return propertyValue.indexOf(filterOption.filterText.toLocaleLowerCase()) !== -1;
        }

        return true;
    }

    private filterNumber(list: any, filterOption: FilterOption) {
        if (list.hasOwnProperty(filterOption.dropDownItem.value)) {
            const propertyValue = list[filterOption.dropDownItem.value].toString().toLocaleLowerCase();
            const threshold = parseInt(filterOption.filterText, 10);

            return propertyValue == threshold;
        }

        return true;   
    }

    private filterNumberLessThanOrEqual(list: any, filterOption: FilterOption) {
        if (list.hasOwnProperty(filterOption.dropDownItem.value)) {
            const propertyValue = list[filterOption.dropDownItem.value].toString().toLocaleLowerCase();
            const threshold = parseInt(filterOption.filterText, 10);
            return propertyValue <= threshold;
        }

        return true;        
    }

    private filterDate(list: Array<string>, filterOption: FilterOption) {
        //Ignore times in data as filterDate doesn't have a time
        if (list.hasOwnProperty(filterOption.dropDownItem.value)) {
            const filterDate = moment(filterOption.filterText, 'DD/MM/YYYY');
            const propertyValue = moment(list[filterOption.dropDownItem.value]);
            return propertyValue.isSame(filterDate, 'day'); 
        }
        return true;
    }
}