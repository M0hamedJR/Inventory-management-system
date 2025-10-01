import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { RepositoryErrorHandlerService } from 'src/app/shared/services/repository-error-handler.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-update-brand',
  templateUrl: './update-shelf.component.html',
})
export class UpdateShelfComponent implements OnInit {
  public shelfForm: FormGroup | any;
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;
  private shelfId: string;
  public warehouses: any[] = [];
  public categories: any[] = [];

  constructor(
    private repository: RepositoryService,
    private errorHandler: RepositoryErrorHandlerService,
    private router: Router,
    private route: ActivatedRoute,
    private modal: BsModalService
  ) {}

  ngOnInit(): void {
    this.shelfId = this.route.snapshot.paramMap.get('id')!;
    this.loadShelf();
    this.loadWarehouses();
    this.loadCategories();
    this.shelfForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(60),
      ]),
      warehouse_Id: new FormControl('', [Validators.required]),
      categoryId: new FormControl('', [Validators.required]),
      isAvailable: new FormControl('', [Validators.required]),
    });
  }

  private loadShelf = () => {
    const apiUrl = `api/shelfs/${this.shelfId}`;
    this.repository.getData(apiUrl).subscribe({
      next: (shelf: any) => {
        this.shelfForm.patchValue(shelf);
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

  private loadWarehouses = () => {
    this.repository.getData('api/warehouses').subscribe({
      next: (data: any) => {
        this.warehouses = data;
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
      },
    });
  };

  private loadCategories = () => {
    this.repository.getData('api/categories').subscribe({
      next: (data: any) => {
        this.categories = data;
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
      },
    });
  };

  updateShelf = (shelfFormValue: any) => {
    if (this.shelfForm.valid) this.executeShelfUpdate(shelfFormValue);
  };

  private executeShelfUpdate = (shelfFormValue: any) => {
    const shelf: any = {
      name: shelfFormValue.name,
      isAvailable: shelfFormValue.isAvailable,
      warehouse_Id: shelfFormValue.warehouse_Id,
      categoryId: shelfFormValue.categoryId,
    };

    console.log(shelf);
    const apiUrl = `api/shelfs/${this.shelfId}`;
    this.repository.update(apiUrl, shelf).subscribe({
      next: () => {
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `Shelf updated successfully`,
            okButtonText: 'OK',
          },
        };
        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe((_: any) =>
          this.redirectToShelfList()
        );
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandler.handleError(err);
        this.errorMessage = this.errorHandler.errorMessage;
      },
    });
  };

  public hasError(controlName: string, errorName: string): boolean {
    return this.shelfForm.controls[controlName].hasError(errorName);
  }

  redirectToShelfList = () => {
    this.router.navigate(['/ui-components/shelf']);
  };
}
