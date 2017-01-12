export class DropDownItem {
    description: string;
    value: string;
    requiresServerCall: boolean;
    type: string;

    constructor(description: string = "", value: string = "", requiresServerCall = false, type: string = "string") {
        this.description = description;
        this.value = value;
        this.type = type;
        this.requiresServerCall = requiresServerCall;
    }
}