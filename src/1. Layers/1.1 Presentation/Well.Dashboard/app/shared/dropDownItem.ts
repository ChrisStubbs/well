export class DropDownItem {
    description: string;
    value: string;

    constructor(description: string = "", value: string = "") {
        this.description = description;
        this.value = value;
    }
}