import { NgModule } from '@angular/core';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';
import { CurrencyPipe, DatePipe, JsonPipe, NgClass } from '@angular/common';
import { MultiSelectModule } from 'primeng/multiselect';
import { SliderModule } from 'primeng/slider';
import { FileUploadModule } from 'primeng/fileupload';
import { DropdownModule } from 'primeng/dropdown';
import { SplitButtonModule } from 'primeng/splitbutton';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RatingModule } from 'primeng/rating';
import { RippleModule } from 'primeng/ripple';
import { ProgressBarModule } from 'primeng/progressbar';
import { TagModule } from 'primeng/tag';
import { ChartModule } from 'primeng/chart';
import { ScrollTopModule } from 'primeng/scrolltop';
import { SkeletonModule } from 'primeng/skeleton';
import { DividerModule } from 'primeng/divider';
import {AutoCompleteModule} from "primeng/autocomplete";

@NgModule({
    exports: [
        ButtonModule,
        TableModule,
        ButtonModule,
        InputTextModule,
        InputTextareaModule,
        PasswordModule,
        CheckboxModule,
        DatePipe,
        JsonPipe,
        CurrencyPipe,
        MultiSelectModule,
        SliderModule,
        FileUploadModule,
        NgClass,
        DropdownModule,
        SplitButtonModule,
        DynamicDialogModule,
        ConfirmDialogModule,
        ToastModule,
        RatingModule,
        RippleModule,
        ProgressBarModule,
        TagModule,
        ChartModule,
        ScrollTopModule,
        SkeletonModule,
        DividerModule,
    ],
    imports: [
        ButtonModule,
        TableModule,
        ButtonModule,
        InputTextModule,
        InputTextareaModule,
        PasswordModule,
        CheckboxModule,
        DatePipe,
        JsonPipe,
        CurrencyPipe,
        MultiSelectModule,
        SliderModule,
        FileUploadModule,
        NgClass,
        DropdownModule,
        SplitButtonModule,
        DynamicDialogModule,
        ConfirmDialogModule,
        ToastModule,
        RatingModule,
        RippleModule,
        ProgressBarModule,
        TagModule,
        ChartModule,
        ScrollTopModule,
        SkeletonModule,
        DividerModule,
        AutoCompleteModule,
    ],
})
export class PrimeNGModule {}
