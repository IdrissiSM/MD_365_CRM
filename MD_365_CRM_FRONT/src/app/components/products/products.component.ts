import { Component, ElementRef, ViewChild } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { Table } from 'primeng/table';
import { APIResponse } from 'src/app/Models/APIResponse';
import { Product } from 'src/app/Models/Product';
import { ProductService } from 'src/app/services/product.service';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.scss'],
})
export class ProductsComponent {
    loading: boolean = true;
    products: Product[] = [];
    // activityValues: number[] = [0.0000000001, 100000000000];
    stateChart: any;
    structureChart: any;
    pieOptions: any;
    quantityChart: any;
    barOptions: any;

    @ViewChild('filter') filter!: ElementRef;

    constructor(
        private productService: ProductService,
        private router: Router
    ) {}

    ngOnInit() {
        this.getProducts();
    }

    initCharts() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        this.initStateChart(documentStyle);
        this.initStructureChart(documentStyle);
        this.initQuantityChart(documentStyle);
        this.pieOptions = {
            cutout: '50%',
            plugins: {
                legend: {
                    labels: {
                        color: textColor,
                    },
                },
            },
        };
    }

    initStateChart(documentStyle: CSSStyleDeclaration) {
        const stateCodeCount = (this.products as any[]).reduce(
            (acc, product) => {
                if (acc.hasOwnProperty(product.stateCode)) {
                    acc[product.stateCode] += 1;
                } else {
                    acc[product.stateCode] = 1;
                }
                return acc;
            },
            {}
        );
        for (let i = 0; i <= 3; i++) {
            if (!(i in stateCodeCount)) {
                stateCodeCount[i] = 0;
            }
        }
        this.stateChart = {
            labels: this.states.map((state) => state.label),
            datasets: [
                {
                    data: Object.values(stateCodeCount),
                    backgroundColor: [
                        documentStyle.getPropertyValue('--blue-500'),
                        documentStyle.getPropertyValue('--yellow-500'),
                        documentStyle.getPropertyValue('--green-500'),
                        documentStyle.getPropertyValue('--red-500'),
                    ],
                    hoverBackgroundColor: [
                        documentStyle.getPropertyValue('--blue-400'),
                        documentStyle.getPropertyValue('--yellow-400'),
                        documentStyle.getPropertyValue('--green-400'),
                        documentStyle.getPropertyValue('--red-400'),
                    ],
                },
            ],
        };
    }
    initStructureChart(documentStyle: CSSStyleDeclaration) {
        const structureCodeCount = (this.products as any[]).reduce(
            (acc, product) => {
                if (acc.hasOwnProperty(product.productStructure)) {
                    acc[product.productStructure] += 1;
                } else {
                    acc[product.productStructure] = 1;
                }
                return acc;
            },
            {}
        );
        for (let i = 1; i <= 3; i++) {
            if (!(i in structureCodeCount)) {
                structureCodeCount[i] = 0;
            }
        }
        this.structureChart = {
            labels: this.structures.map((structure) => structure.label),
            datasets: [
                {
                    data: Object.values(structureCodeCount),
                    backgroundColor: [
                        documentStyle.getPropertyValue('--indigo-500'),
                        documentStyle.getPropertyValue('--purple-500'),
                        documentStyle.getPropertyValue('--teal-500'),
                    ],
                    hoverBackgroundColor: [
                        documentStyle.getPropertyValue('--indigo-400'),
                        documentStyle.getPropertyValue('--purple-400'),
                        documentStyle.getPropertyValue('--teal-400'),
                    ],
                },
            ],
        };
    }

    initQuantityChart(documentStyle: CSSStyleDeclaration) {
        const productQuantityMap: Map<string, number> = new Map();
        for (const product of this.products) {
            productQuantityMap.set(
                product.productNumber,
                parseInt(product.quantityOnHand, 10)
            );
        }
        const sortedProductQuantityMap = new Map(
            [...productQuantityMap.entries()].sort((a, b) => a[1] - b[1])
        );
        const slicedProductQuantityMap = new Map(
            [...sortedProductQuantityMap.entries()].slice(0, 5)
        );

        const textColor = documentStyle.getPropertyValue('--text-color');
        const textColorSecondary = documentStyle.getPropertyValue(
            '--text-color-secondary'
        );
        const surfaceBorder =
            documentStyle.getPropertyValue('--surface-border');
        this.quantityChart = {
            labels: Array.from(slicedProductQuantityMap.keys()),
            datasets: [
                {
                    label: 'Lowest Quantity Products',
                    data: Array.from(slicedProductQuantityMap.values()),
                    backgroundColor: [
                        'rgba(255, 159, 64, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(48, 132, 96, 0.2)',
                    ],
                    borderColor: [
                        'rgb(255, 159, 64)',
                        'rgb(75, 192, 192)',
                        'rgb(54, 162, 235)',
                        'rgb(153, 102, 255)',
                        'rgba(48, 132, 96)',
                    ],
                    borderWidth: 1,
                },
            ],
        };

        this.barOptions = {
            plugins: {
                legend: {
                    labels: {
                        color: textColor,
                    },
                },
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        color: textColorSecondary,
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false,
                    },
                },
                x: {
                    ticks: {
                        color: textColorSecondary,
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false,
                    },
                },
            },
        };
    }

    productDetails(id: number) {
        const navigationExtras: NavigationExtras = {
            queryParams: { id: id },
            //   skipLocationChange: true
        };
        this.router.navigate(['/product-details'], navigationExtras);
    }

    // GET All
    getProducts() {
        this.productService
            .getAllProducts()
            .subscribe((response: APIResponse) => {
                if (response.success) {
                    this.products = response.result;
                    this.loading = false;
                    this.initCharts();
                } else {
                    console.log(response.errorMessages);
                }
            });
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal(
            (event.target as HTMLInputElement).value,
            'contains'
        );
    }

    clear(table: Table) {
        table.clear();
        this.filter.nativeElement.value = '';
    }

    getFirstNWords(text: string, n: number): string {
        const wordsArray = text?.split(' ');
        const firstNWords = wordsArray?.slice(0, n);
        return firstNWords?.join(' ');
    }
    getTypeLabel(typeCode: number): string {
        const type = this.types.find((type: any) => type.value == typeCode);
        return type ? type.label : '';
    }
    getStructureLabel(structureCode: number): string {
        const structure = this.structures.find(
            (structure: any) => structure.value == structureCode
        );
        return structure ? structure.label : '';
    }
    getStateLabel(stateCode: number): string {
        const state = this.states.find(
            (state: any) => state.value == stateCode
        );
        return state ? state.label : '';
    }
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
    states = [
        { value: 0, label: 'Active' },
        { value: 1, label: 'Retired' },
        { value: 2, label: 'Draft' },
        { value: 3, label: 'Under Revision' },
    ];
}
