import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { RepositoryErrorHandlerService } from 'src/app/shared/services/repository-error-handler.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-update-warehouse',
  templateUrl: './update-warehouse.component.html',
})
export class UpdateWarehouseComponent implements OnInit {
  public warehouseForm: FormGroup | any;
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;
  private warehouseId: string;

  constructor(
    private repository: RepositoryService,
    private errorHandler: RepositoryErrorHandlerService,
    private router: Router,
    private route: ActivatedRoute,
    private modal: BsModalService
  ) {}

  ngOnInit(): void {
    this.warehouseId = this.route.snapshot.paramMap.get('id')!;
    this.loadWarehouse();
    this.warehouseForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      id: new FormControl('', [Validators.required, Validators.maxLength(60)]),
      capacity: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      location: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
    });
  }

  private loadWarehouse = () => {
    const apiUrl = `api/warehouses/${this.warehouseId}`;
    this.repository.getData(apiUrl).subscribe({
      next: (warehouse: any) => {
        this.warehouseForm.patchValue(warehouse);
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

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

  updateWarehouse = (warehouseFormValue: any) => {
    if (this.warehouseForm.valid)
      this.executeWarehouseUpdate(warehouseFormValue);
  };

  private executeWarehouseUpdate = (warehouseFormValue: any) => {
    const warehouse: any = {
      name: warehouseFormValue.name,
      id: warehouseFormValue.id,
      capacity: warehouseFormValue.capacity,
      location: warehouseFormValue.location,
    };

    const apiUrl = `api/warehouses/${this.warehouseId}`;
    this.repository.update(apiUrl, warehouse).subscribe({
      next: () => {
        console.log(warehouse);
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `Warehouse updated successfully`,
            okButtonText: 'OK',
          },
        };

        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe((_: any) =>
          this.redirectToWarehouseList()
        );
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

  redirectToWarehouseList = () => {
    this.router.navigate(['/ui-components/warehouse']);
  };
}
