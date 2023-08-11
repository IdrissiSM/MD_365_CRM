export enum AccountStateCode {
    ACTIVE,
    INACTIVE
}
export class Account {
    accountid: string;
    name: string;
    description: string;
    emailaddress1: string;
    entityimage_url: string | null;
    revenue: number;
    statecode: number;

    constructor(
        accountid: string,
        name: string,
        description: string,
        emailaddress1: string,
        entityimage_url: string | null,
        revenue: number,
        statecode: number
    ) {
        this.accountid = accountid;
        this.name = name;
        this.description = description;
        this.emailaddress1 = emailaddress1;
        this.entityimage_url = entityimage_url;
        this.revenue = revenue;
        this.statecode = statecode;
    }
}
