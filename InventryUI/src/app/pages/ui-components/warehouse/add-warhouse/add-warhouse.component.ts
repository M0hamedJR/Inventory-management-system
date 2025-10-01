import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { RepositoryErrorHandlerService } from 'src/app/shared/services/repository-error-handler.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-add-brand',
  templateUrl: './add-warehouse.component.html',
})
export class AddWarehouseComponent implements OnInit {
  public warehouseForm: FormGroup | any;
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;

  constructor(
    private repository: RepositoryService,
    private errorHandler: RepositoryErrorHandlerService,
    private router: Router,
    private modal: BsModalService
  ) {}

  ngOnInit(): void {
    this.warehouseForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      location: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      capacity: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
    });
  }

  validateControl = (controlName: string) => {
    if (
      this.warehouseForm.get(controlName).invalid &&
      this.warehouseForm.get(controlName).touched
    )
      return true;

    return false;
  };

  hasError = (controlName: string, errorName: string) => {
    if (this.warehouseForm.get(controlName).hasError(errorName)) return true;

    return false;
  };

  createWarehouse = (warehouseFormValue: any) => {
    if (this.warehouseForm.valid)
      this.executeWarehouseCreation(warehouseFormValue);
  };

  private executeWarehouseCreation = (warehouseFormValue: any) => {
    const warehouse: any = {
      name: warehouseFormValue.name,
      location: warehouseFormValue.location,
      capacity: warehouseFormValue.capacity,
    };

    const apiUrl = 'api/warehouses';
    console.log(warehouse);
    this.repository.create(apiUrl, warehouse).subscribe({
      next: (createdWarehouse: any) => {
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `Warehuose: ${createdWarehouse.name} created successfully`,
            okButtonText: 'OK',
          },
        };

        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe((_: any) =>
          this.redirectToWarehouseList()
        );
      },
    });
  };

  redirectToWarehouseList = () => {
    this.router.navigate(['/ui-components/warehouse']);
  };
}
