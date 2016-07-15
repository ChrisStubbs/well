export class DropDownItem {
    description: string;
    value: string;
    requiresServerCall: boolean;

    constructor(description: string = "", value: string = "", requiresServerCall = false) {
        this.description = description;
        this.value = value;
        this.requiresServerCall = requiresServerCall;
    }
}