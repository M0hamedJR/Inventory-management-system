import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Warehouse } from 'src/app/_interface/inventory/warehouse';
import { ErrorModalComponent } from 'src/app/shared/modals/error-modal/error-modal.component';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { RepositoryErrorHandlerService } from 'src/app/shared/services/repository-error-handler.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
})
export class WarehouseComponent implements OnInit {
  public errorMessage: string = '';
  public bsModalRef?: BsModalRef;
  public displayedColumns = [
    'name',
    'location',
    'capacity',
    'delete',
    'update',
  ];
  public dataSource = new MatTableDataSource<Warehouse>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private repoService: RepositoryService,
    private errorService: RepositoryErrorHandlerService,
    private router: Router,
    private dialogserve: DialogService,
    private modal: BsModalService,
    private authService: AuthenticationService
  ) {}

  ngOnInit() {
    this.getAllWarehouses();
  }

  public getAllWarehouses = () => {
    this.repoService.getData('api/warehouses').subscribe(
      (res) => {
        this.dataSource.data = res as Warehouse[];
      },
      (error: HttpErrorResponse) => {
        this.errorService.handleError(error);
      }
    );
  };

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public doFilter = (value: string) => {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  };

  public createWarehouse() {
    if (this.authService.isUserAdmin()) {
      this.router.navigate(['/ui-components/add-warehouse']);
    } else {
      const config: ModalOptions = {
        initialState: {
          modalHeaderText: 'Error Message',
          modalBodyText: 'Only Admins allowed',
          okButtonText: 'OK',
        },
      };
      this.modal.show(ErrorModalComponent, config);
    }
  }

  public redirectToUpdate = (id: string) => {
    if (this.authService.isUserAdmin()) {
      this.router.navigate([`/ui-components/update-warehouse/${id}`]);
    } else {
      const config: ModalOptions = {
        initialState: {
          modalHeaderText: 'Error Message',
          modalBodyText: 'Only Admin allowed',
          okButtonText: 'OK',
        },
      };
      this.modal.show(ErrorModalComponent, config);
    }
  };

  DeleteWarehouse(id: any) {
    if (this.authService.isUserAdmin()) {
      this.dialogserve
        .openConfirmDialog('Are you sure, you want to delete the warehouse ?')
        .afterClosed()
        .subscribe((res) => {
          if (res) {
            const deleteUri: string = `api/warehouses/${id}`;
            this.repoService.delete(deleteUri).subscribe((res) => {
              const config: ModalOptions = {
                initialState: {
                  modalHeaderText: 'Success Message',
                  modalBodyText: `Warehouse deleted successfully`,
                  okButtonText: 'OK',
                },
              };

              this.bsModalRef = this.modal.show(SuccessModalComponent, config);
              this.bsModalRef.content.redirectOnOk.subscribe((_: any) =>
                this.getAllWarehouses()
              );
            });
          }
        });
    } else {
      const config: ModalOptions = {
        initialState: {
          modalHeaderText: 'Error Message',
          modalBodyText: 'Only Admin allowed',
          okButtonText: 'OK',
        },
      };
      this.modal.show(ErrorModalComponent, config);
    }
  }
}
