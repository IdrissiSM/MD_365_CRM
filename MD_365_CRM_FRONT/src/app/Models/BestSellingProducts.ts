export class BestSellingProducts {
    productName: string;
    quantity: number;
    pricePerUnit: number;

    constructor(productName: string, quantity: number, pricePerUnit: number) {
        this.productName = productName;
        this.quantity = quantity;
        this.pricePerUnit = pricePerUnit;
    }
}
