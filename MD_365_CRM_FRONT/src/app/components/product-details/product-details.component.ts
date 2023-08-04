import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIResponse } from 'src/app/Models/APIResponse';
import { Product } from 'src/app/Models/Product';
import { ProductService } from 'src/app/services/product.service';

@Component({
    selector: 'app-product-details',
    templateUrl: './product-details.component.html',
    styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent {
    loading: boolean = true;
    id!: string;
    product?: Product;

    constructor(
        private productService: ProductService,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.route.queryParams.subscribe((params) => {
            this.id = params['id'];
        });
        this.getProduct();
    }
    getProduct() {
        this.productService
            .getProductById(this.id)
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.product = response.result;
                    console.log(response.result);
                    this.loading = false;
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    getStateLabel(stateCode: any): string {
        const state = this.states.find(
            (state: any) => state.value == stateCode
        );
        return state ? state.label : '';
    }

    getStatusLabel(statusCode: any): string {
        const status = this.statuses.find(
            (status: any) => status.value == statusCode
        );
        return status ? status.label : '';
    }

    getTypeLabel(typeCode: any): string {
        const type = this.types.find((type: any) => type.value == typeCode);
        return type ? type.label : '';
    }

    getStructureLabel(structureCode: any): string {
        const structure = this.structures.find(
            (structure: any) => structure.value == structureCode
        );
        return structure ? structure.label : '';
    }

    statuses = [
        { value: 0, label: 'Draft' },
        { value: 1, label: 'Active' },
        { value: 2, label: 'Retired' },
        { value: 3, label: 'Under Revision' },
    ];

    states = [
        { value: 0, label: 'Active' },
        { value: 1, label: 'Retired' },
        { value: 2, label: 'Draft' },
        { value: 3, label: 'Under Revision' },
    ];

    types = [
        { value: 1, label: 'Sales Inventory' },
        { value: 2, label: 'Miscellaneous Charges' },
        { value: 3, label: 'Services' },
        { value: 4, label: 'Flat Fees' },
    ];

    structures = [
        { value: 1, label: 'Product' },
        { value: 2, label: 'Product Family' },
        { value: 3, label: 'Product Bundle' },
    ];
}
