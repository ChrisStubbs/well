import { PipeTransform, Pipe } from '@angular/core';
import {FilterOption} from "./filterOption";

@Pipe({
    name: 'optionFilter'
})
export class OptionFilterPipe implements PipeTransform {
    transform(value: any[], args: FilterOption[]): any[] {
        
        let filterOption: FilterOption = args[0] ? args[0] : null;

        return filterOption
            ? value.filter((delivery: any) => {
                return delivery.hasOwnProperty(filterOption.dropDownItem.value)
                    ? delivery[filterOption.dropDownItem.value].toString().toLocaleLowerCase()
                        .indexOf(filterOption.filterText.toLocaleLowerCase()) !== -1
                    : true;
            }) : value;
    }
}

