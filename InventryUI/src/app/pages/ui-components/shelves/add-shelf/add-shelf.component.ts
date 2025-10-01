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
  templateUrl: './add-shelf.component.html',
})
export class AddShelfComponent implements OnInit {
  public shelfForm: FormGroup | any;
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;
  public Warehouse: any[] = [];
  public warehouse_Id: any[] = [];
  public categories: any[] = [];

  constructor(
    private repository: RepositoryService,
    private errorHandler: RepositoryErrorHandlerService,
    private router: Router,
    private modal: BsModalService
  ) {}

  ngOnInit(): void {
    this.shelfForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      isAvailable: new FormControl(true, [
        Validators.required,
        Validators.maxLength(60),
      ]),
      warehouse_Id: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      categoryId: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      WarehouseId: new FormControl(''),
    });
    this.loadDropdownData();
  }

  private loadDropdownData() {
    this.repository.getData('api/categories').subscribe(
      (res) => (this.categories = res as any[]),
      (err) => this.errorHandler.handleError(err)
    );
    this.repository.getData('api/Warehouses').subscribe(
      (res) => (this.Warehouse = res as any[]),
      (err) => this.errorHandler.handleError(err)
    );
  }

  validateControl = (controlName: string) => {
    if (
      this.shelfForm.get(controlName).invalid &&
      this.shelfForm.get(controlName).touched
    )
      return true;

    return false;
  };

  hasError = (controlName: string, errorName: string) => {
    if (this.shelfForm.get(controlName).hasError(errorName)) return true;

    return false;
  };

  createShelf = (shelfFormValue: any) => {
    if (this.shelfForm.valid) this.executeShelfCreation(shelfFormValue);
  };

  private executeShelfCreation = (shelfFormValue: any) => {
    const shelf: any = {
      name: shelfFormValue.name,
      isAvailable: shelfFormValue.isAvailable,
      warehouse_Id: shelfFormValue.warehouse_Id,
      categoryId: shelfFormValue.categoryId,
    };

    console.log(shelf);
    const apiUrl = 'api/shelfs';
    this.repository.create(apiUrl, shelf).subscribe({
      next: (createdBrand: any) => {
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `Shelf: ${createdBrand.name} created successfully`,
            okButtonText: 'OK',
          },
        };

        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe((_: any) =>
          this.redirectToShelfList()
        );
      },
    });
  };

  redirectToShelfList = () => {
    this.router.navigate(['/ui-components/shelf']);
  };
}
