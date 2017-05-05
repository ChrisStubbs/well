export interface SingleRoute
{
    stop: string;
    invoice: string;
    jobType: string;
    jobStatus: string;
    cod: string;
    pod: boolean;
    exceptions: number;
    clean: number;
    tba: number;
    credit?: number;
    assignee: string;
    selected?: boolean;
    status: string;
}