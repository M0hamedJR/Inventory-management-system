import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Shelfs } from 'src/app/_interface/inventory/shelfs';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { RepositoryErrorHandlerService } from 'src/app/shared/services/repository-error-handler.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-add-device',
  templateUrl: './add-product.component.html',
})
export class AddShipmentComponent implements OnInit {
  public deviceForm: FormGroup | any;
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;
  public categories: any[] = [];
  public shelfs: any[] = [];
  public weights: any[] = [];
  public shipped_From: any[] = [];
  public shipped_To: any[] = [];
  public shipped_Address_From: any[] = [];
  public shipped_Address_To: any[] = [];
  public shelfId: any[] = [];
  public WarehouseId: any[] = [];
  public Warehouse: any[] = [];

  constructor(
    private repository: RepositoryService,
    private errorHandler: RepositoryErrorHandlerService,
    private router: Router,
    private modal: BsModalService
  ) {}

  ngOnInit(): void {
    this.deviceForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      categoryId: new FormControl('', [Validators.required]),
      Weight: new FormControl('', [Validators.required]),
      Price: new FormControl('', [Validators.required]),
      Shipped_From: new FormControl(new Date(), [Validators.required]),
      Shipped_To: new FormControl(new Date(), [Validators.required]),
      Shipped_Address_To: new FormControl('', [Validators.required]),
      Shipped_Address_From: new FormControl('', [Validators.required]),
      customerName: new FormControl('', [Validators.required]),
      phone: new FormControl('', [Validators.required]),
      dealer: new FormControl('', [Validators.required]),
    });

    this.loadDropdownData();
  }

  private loadDropdownData() {
    this.repository.getData('api/categories').subscribe(
      (res) => (this.categories = res as any[]),
      (err) => this.errorHandler.handleError(err)
    );

    this.repository.getData('api/shelfs').subscribe(
      (res) => {
        // Filter available shelves
        this.shelfs = (res as any[]).filter((shelf) => shelf.isAvailable);
      },
      (err) => this.errorHandler.handleError(err)
    );

    this.repository.getData('api/Warehouses').subscribe(
      (res) => (this.Warehouse = res as any[]),
      (err) => this.errorHandler.handleError(err)
    );
  }

  validateControl(controlName: string): boolean {
    return (
      this.deviceForm.get(controlName).invalid &&
      this.deviceForm.get(controlName).touched
    );
  }

  hasError(controlName: string, errorName: string): boolean {
    return this.deviceForm.get(controlName).hasError(errorName);
  }

  createShipment(deviceFormValue: any): void {
    if (this.deviceForm.valid) {
      this.executeDeviceCreation(deviceFormValue);
    }
  }

  private executeDeviceCreation(deviceFormValue: any): void {
    const product = {
      name: deviceFormValue.name,
      categoryId: deviceFormValue.categoryId,
      weight: deviceFormValue.Weight,
      price: deviceFormValue.Price,
      shipped_From: deviceFormValue.Shipped_From,
      shipped_To: deviceFormValue.Shipped_To,
      shipped_Address_From: deviceFormValue.Shipped_Address_From,
      shipped_Address_To: deviceFormValue.Shipped_Address_To,
      customerName: deviceFormValue.customerName,
      phone: deviceFormValue.phone,
      dealer: deviceFormValue.dealer,
    };

    console.log(product);
    const apiUrl = 'api/products';
    this.repository.create(apiUrl, product).subscribe({
      next: (createdProduc: any) => {
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `Shipment: ${createdProduc.name} created successfully`,
            okButtonText: 'OK',
          },
        };
        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe(() =>
          this.redirectToShipmentList()
        );
      },
      error: (err: any) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  }

  redirectToShipmentList(): void {
    this.router.navigate(['/ui-components/product']);
  }
}
