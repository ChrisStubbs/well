import { PipeTransform, Pipe } from '@angular/core';
import {FilterOption} from "./filterOption";
import * as moment from 'moment/moment';

@Pipe({
    name: 'optionFilter'
})
export class OptionFilterPipe implements PipeTransform {
    transform(value: any[], args: FilterOption): any[] {

        let filterOption: FilterOption = args; /*args[0] ? args[0] : null;*/

        if (!filterOption) {
            return value;
        }

        if (filterOption.dropDownItem.description == 'Date') {
            return value.filter((delivery: any) => this.filterDate(delivery, filterOption));

        } else {

            return value.filter((delivery: any) => this.filterString(delivery, filterOption));
        }
    }

    filterString(list: any, filterOption: FilterOption) {
        if (list.hasOwnProperty(filterOption.dropDownItem.value)) {
            var propertyValue = list[filterOption.dropDownItem.value].toString().toLocaleLowerCase();
            return propertyValue.indexOf(filterOption.filterText.toLocaleLowerCase()) !== -1
        }
        return true;
    }

    filterDate(list: any, filterOption: FilterOption) {
        //Ignore times in data as filterDate doesn't have a time
        if (list.hasOwnProperty(filterOption.dropDownItem.value)) {
            var filterDate = moment(filterOption.filterText, "DD/MM/YYYY");
            var propertyValue = moment(list[filterOption.dropDownItem.value]);
            return propertyValue.isSame(filterDate, 'day'); 
        }
        return true;
    }
}

