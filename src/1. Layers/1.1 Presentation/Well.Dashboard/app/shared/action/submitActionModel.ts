export interface ISubmitActionModel {
    action: string;
    jobIds: number[];
}

export interface ISubmitActionResult {
    message: string;
    isValid: boolean;
    warnings: string[];
}