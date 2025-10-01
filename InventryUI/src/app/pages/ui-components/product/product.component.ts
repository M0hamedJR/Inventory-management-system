import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Shelfs } from 'src/app/_interface/inventory/shelfs';
import { Category } from 'src/app/_interface/inventory/category';
import { Device } from 'src/app/_interface/inventory/device';
import { ErrorModalComponent } from 'src/app/shared/modals/error-modal/error-modal.component';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { RepositoryService } from 'src/app/shared/services/repository.service';

@Component({
  selector: 'app-device',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ShipmentComponent implements OnInit, AfterViewInit {
  public categories: Category[] = [];
  public shelfs: Shelfs[] = [];
  public filterForm: FormGroup;
  public errorMessage: string = '';
  public displayedColumns = [
    'name',
    'serialNumber',
    'categoryName',
    'shelfName',
    'warehouseName',
    'price',
    'in_Out',
    'customerName',
    'phone',
    'dealer',
    'actions',
  ];
  public dataSource = new MatTableDataSource<Device>();
  private allDevices: Device[] = [];
  public bsModalRef?: BsModalRef;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private repoService: RepositoryService,
    private fb: FormBuilder,
    private dialogService: DialogService,
    private modal: BsModalService,
    private router: Router,
    private authService: AuthenticationService
  ) {}

  public isAdmin = this.authService.isUserAdmin();

  ngOnInit() {
    this.filterForm = this.fb.group({
      category: [''],
      shelves: [''],
    });

    this.loadDropdownData();
    this.getAllShipment();

    this.filterForm.valueChanges.subscribe(() => {
      this.applyFilter();
    });
  }

  public getInOutStatus(value: number | null | boolean): string {
    if (value === true) {
      return 'In';
    } else if (value === false) {
      return 'Out';
    } else {
      return 'Not assigned yet';
    }
  }

  public createShipment() {
    this.router.navigate(['/ui-components/add-product']);
  }

  private loadDropdownData = () => {
    this.repoService
      .getData('api/categories')
      .subscribe((res) => (this.categories = res as Category[]));

    this.repoService.getData('api/shelfs').subscribe((res) => {
      this.shelfs = res as Shelfs[];
    });
  };

  private getAllShipment() {
    this.repoService.getData('api/products').subscribe(
      (res) => {
        this.allDevices = res as Device[];
        this.dataSource.data = this.allDevices;
        this.applyFilter();
        console.log(res);
      },
      (error) => {
        this.errorMessage = 'Failed to load devices. Please try again later.';
        console.error('Error fetching devices:', error);
      }
    );
  }

  private applyFilter() {
    const filters = this.filterForm.value;

    // Filter devices based on selected filters
    const filteredDevices = this.allDevices.filter((device) => {
      const matchesCategory =
        !filters.category || device.categoryName === filters.category;
      const matchesBrand =
        !filters.shelves || device.shelfName === filters.shelves;

      return matchesCategory && matchesBrand;
    });

    this.dataSource.data = filteredDevices;

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  public doFilter(value: string) {
    this.dataSource.filter = value.trim().toLowerCase();
  }

  public redirectToUpdate(id: string) {
    if (this.authService.isUserAdmin()) {
      this.router.navigate([`/ui-components/update-product/${id}`]);
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

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public deleteShipment = (id: string) => {
    if (this.authService.isUserAdmin()) {
      this.dialogService
        .openConfirmDialog('Are you sure you want to delete this Shipment?')
        .afterClosed()
        .subscribe((res) => {
          if (res) {
            this.repoService.delete(`api/products/${id}`).subscribe(() => {
              const config: ModalOptions = {
                initialState: {
                  modalHeaderText: 'Success Message',
                  modalBodyText: `Shipment deleted successfully`,
                  okButtonText: 'OK',
                },
              };

              this.bsModalRef = this.modal.show(SuccessModalComponent, config);
              this.bsModalRef.content.redirectOnOk.subscribe(() =>
                this.getAllShipment()
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
  };
}
