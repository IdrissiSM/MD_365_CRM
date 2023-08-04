export interface APIResponse {
    httpStatusCode: number;
    success: boolean;
    errorMessages: string[];
    result: any;
}
