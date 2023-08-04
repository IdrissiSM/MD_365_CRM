export interface Product {
    productId: string;
    productNumber: string;
    name: string;
    description: string;
    productStructure: string;
    price: string;
    price_Base: string;
    productTypeCode: string;
    quantityOnHand: string;
    standardCost: string;
    currentCost: string;
    currentCost_Base: string;
    standardCost_Base: string;
    stateCode: number;
    statusCode: number;
    stockVolume: string;
    exchangeRate: string;
    versionNumber: string;
    validFromDate: Date;
    validToDate: Date;
    createdOn: Date;
}
