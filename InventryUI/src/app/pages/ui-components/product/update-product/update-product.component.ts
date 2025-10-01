import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { RepositoryErrorHandlerService } from 'src/app/shared/services/repository-error-handler.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-update-device',
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css'],
})
export class UpdateShipmentComponent implements OnInit {
  public deviceForm: FormGroup | any;
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;
  private deviceId: string | null = '';
  public categories: any[] = [];
  public shelfs: any[] = [];
  public warehouses: any[] = [];

  public suppliers: any[] = [];

  constructor(
    private repository: RepositoryService,
    private errorHandler: RepositoryErrorHandlerService,
    private router: Router,
    private modal: BsModalService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.deviceId = this.route.snapshot.paramMap.get('id');
    this.deviceForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),

      id: new FormControl('', [Validators.required]),
      in_Out: new FormControl(),
      price: new FormControl('', [Validators.required]),
      categoryId: new FormControl('', [Validators.required]),
      shelfId: new FormControl(''),
      weight: new FormControl(1, [Validators.required]),
      shipped_From: new FormControl(Date, [Validators.required]),
      shipped_To: new FormControl(Date, [Validators.required]),
      shipped_Address_From: new FormControl('', [Validators.required]),
      shipped_Address_To: new FormControl('', [Validators.required]),
      warehouseId: new FormControl(''),
      customerName: new FormControl('', [Validators.required]),
      dealer: new FormControl('', [Validators.required]),
      phone: new FormControl('', [Validators.required]),
    });
    this.initializeForm();
    this.getCategories();
    this.getshelfs();
    this.getWarhouses();
  }

  private initializeForm = () => {
    if (this.deviceId) {
      this.repository.getData(`api/products/${this.deviceId}`).subscribe({
        next: (device: any) => {
          this.deviceForm.patchValue({
            name: device.name,
            id: device.id,
            categoryId: device.categoryId,
            shelfId: device.shelfId,
            weight: device.weight,
            in_Out: device.in_Out,
            price: device.price,
            shipped_From: device.shipped_From,
            shipped_To: device.shipped_To,
            shipped_Address_From: device.shipped_Address_From,
            shipped_Address_To: device.shipped_Address_To,
            warehouseId: device.warehouseId,
            phone: device.phone,
            dealer: device.dealer,
            customerName: device.customerName,
          });
          console.log(device);
        },

        error: (err: HttpErrorResponse) => {
          this.errorHandler.handleError(err);
          this.errorMessage = this.errorHandler.errorMessage;
        },
      });
    }
  };

  private getCategories = () => {
    this.repository.getData('api/categories').subscribe({
      next: (categories: any[] | any) => (this.categories = categories),
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };
  private getWarhouses = () => {
    this.repository.getData('api/warehouses').subscribe({
      next: (warehouses: any[] | any) => (this.warehouses = warehouses),
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

  private getshelfs = () => {
    this.repository.getData('api/shelfs').subscribe({
      next: (shelfs: any[] | any) => {
        // Filter only available shelves
        this.shelfs = shelfs.filter((shelf: any) => shelf.isAvailable);
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

  validateControl = (controlName: string) => {
    if (
      this.deviceForm.get(controlName).invalid &&
      this.deviceForm.get(controlName).touched
    )
      return true;

    return false;
  };

  hasError = (controlName: string, errorName: string) => {
    if (this.deviceForm.get(controlName).hasError(errorName)) return true;

    return false;
  };

  updateShipment = (deviceFormValue: any) => {
    if (this.deviceForm.valid) this.executeShipmentUpdate(deviceFormValue);
  };

  private executeShipmentUpdate = (deviceFormValue: any) => {
    const updatedDevice: any = {
      id: this.deviceId ? this.deviceId : '',
      name: deviceFormValue.name,
      categoryId: deviceFormValue.categoryId,
      shelfId: deviceFormValue.shelfId,
      weight: deviceFormValue.weight,
      price: deviceFormValue.price,
      in_Out: deviceFormValue.in_Out,
      shipped_From: deviceFormValue.shipped_From,
      shipped_To: deviceFormValue.shipped_To,
      shipped_Address_From: deviceFormValue.shipped_Address_From,
      shipped_Address_To: deviceFormValue.shipped_Address_To,
      warehouseId: deviceFormValue.warehouseId,
      phone: deviceFormValue.phone,
      dealer: deviceFormValue.dealer,
      customerName: deviceFormValue.customerName,
    };
    console.log(updatedDevice);
    const apiUrl = `api/products/${this.deviceId}`;
    this.repository.update(apiUrl, updatedDevice).subscribe({
      next: () => {
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `Shipment: ${updatedDevice.name} updated successfully`,
            okButtonText: 'OK',
          },
        };

        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe(() =>
          this.redirectToShipmentList()
        );
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

  redirectToShipmentList = () => {
    this.router.navigate(['/ui-components/product']);
  };
}
