export interface ISubmitActionModel {
    jobIds: number[];
}

export interface ISubmitActionResult {
    message: string;
    isValid: boolean;
    warnings: string[];
}