import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { ProductCategoryReport } from 'src/app/_interface/inventory/device';
import { Warehouse } from 'src/app/_interface/inventory/warehouse';
import { Shelfs } from 'src/app/_interface/inventory/shelfs';
import { RepositoryService } from 'src/app/shared/services/repository.service';
import { LegendPosition } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.style.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AppDashboardComponent implements OnInit, OnDestroy {
  productReports: ProductCategoryReport[] = [];
  warehouseReports: Warehouse[] = [];
  shelfReports: Shelfs[] = [];
  errorMessage: string = '';
  warehouseName: string = '';
  currentDate: Date = new Date();
  legendPosition = LegendPosition.Right;
  customShelfColors = [
    { name: 'Available', value: '#4CAF50' },
    { name: 'Unavailable', value: '#FF0000' },
  ];

  totalCategories: number = 0;
  totalWarehouses: number = 0;
  totalShelves: number = 0;
  totalPrice: number = 0;

  categoryChartData: any[] = [];
  warehouseChartData: any[] = [];
  shelfChartData: any[] = [];

  private refreshIntervalId: any;

  constructor(private repositoryService: RepositoryService) {}

  ngOnInit(): void {
    this.loadDashboardData();

    // Refresh every 3 seconds
    this.refreshIntervalId = setInterval(() => {
      this.loadDashboardData();
    }, 3000);
  }

  ngOnDestroy(): void {
    // Clear the interval to prevent memory leaks
    if (this.refreshIntervalId) {
      clearInterval(this.refreshIntervalId);
    }
  }

  private loadDashboardData(): void {
    this.getDeviceCategoryReport();
    this.getWarehousesReport();
    this.getShelvesReport();
    this.getTotalPrice();
    this.currentDate = new Date();
  }

  public getDeviceCategoryReport() {
    const url = `api/products/count`;
    this.repositoryService.getData(url).subscribe(
      (res) => {
        this.productReports = res as ProductCategoryReport[];
        this.totalCategories = this.productReports.length;
        this.categoryChartData = this.productReports.map((report) => ({
          name: `${report.categoryName} - Total`,
          value: report.totalProducts,
        }));
      },
      (error) => {
        this.errorMessage = error.message;
      }
    );
  }

  public getWarehousesReport() {
    const url = `api/Warehouses`;
    this.repositoryService.getData(url).subscribe(
      (res) => {
        this.warehouseReports = res as Warehouse[];
        this.totalWarehouses = this.warehouseReports.length;

        if (this.warehouseReports.length > 0) {
          this.warehouseName = this.warehouseReports[0].name;
        }

        this.warehouseChartData = this.warehouseReports.map((report) => ({
          name: report.name,
          value: report.capacity,
        }));
      },
      (error) => {
        this.errorMessage = error.message;
      }
    );
  }

  public getShelvesReport() {
    const url = `api/shelfs`;
    this.repositoryService.getData(url).subscribe(
      (res) => {
        this.shelfReports = res as Shelfs[];
        this.totalShelves = this.shelfReports.length;

        this.shelfChartData = [
          {
            name: 'Available',
            value: this.shelfReports.filter((shelf) => shelf.isAvailable)
              .length,
          },
          {
            name: 'Unavailable',
            value: this.shelfReports.filter((shelf) => !shelf.isAvailable)
              .length,
          },
        ];
      },
      (error) => {
        this.errorMessage = error.message;
      }
    );
  }

  public getTotalPrice() {
    const url = `api/products`;
    this.repositoryService.getData(url).subscribe(
      (res) => {
        const products = res as any[];
        this.totalPrice = products.reduce(
          (sum, product) => sum + (product.price || 0),
          0
        );
      },
      (error) => {
        this.errorMessage = error.message;
      }
    );
  }
}
