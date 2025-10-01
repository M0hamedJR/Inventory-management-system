import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../../material.module';
import { TablerIconsModule } from 'angular-tabler-icons';
import * as TablerIcons from 'angular-tabler-icons/icons';
import { UiComponentsRoutes } from './ui-components.routing';
import { MatNativeDateModule } from '@angular/material/core';
import { UsersComponent } from './users/users.component';
import { RolesComponent } from './roles/roles.component';
import { ToastrModule } from 'ngx-toastr';
import { AddUserComponent } from './users/add-user/add-user.component';
import { UpdateUserComponent } from './users/update-user/update-user.component';
import { AddRoleComponent } from './roles/add-role/add-role.component';
import { UpdateRoleComponent } from './roles/update-role/update-role.component';
import { ShelfComponent } from './shelves/shelf.component';
import { AddShelfComponent } from './shelves/add-shelf/add-shelf.component';
import { UpdateShelfComponent } from './shelves/update-shelf/update-shelf.component';
import { CategoryComponent } from './category/category.component';
import { AddCategoryComponent } from './category/add-category/add-category.component';
import { UpdateCategoryComponent } from './category/update-category/update-category.component';
import { ShipmentComponent } from './product/product.component';
import { AddShipmentComponent } from './product/add-product/add-product.component';
import { UpdateShipmentComponent } from './product/update-product/update-product.component';
import { UpdateUserRoleComponent } from './users/update-user-role/update-user-role.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { AddWarehouseComponent } from './warehouse/add-warhouse/add-warhouse.component';
import { UpdateWarehouseComponent } from './warehouse/update-warehouse/update-warehouse.component';
import { BooleanToShapePipe } from 'src/app/boolean-to-shape.pipe';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(UiComponentsRoutes),
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    TablerIconsModule.pick(TablerIcons),
    MatNativeDateModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: true,
      closeButton: true,
    }),
  ],
  declarations: [
    UsersComponent,
    RolesComponent,
    AddUserComponent,
    UpdateUserComponent,
    AddRoleComponent,
    UpdateRoleComponent,
    ShelfComponent,
    AddShelfComponent,
    UpdateShelfComponent,
    CategoryComponent,
    AddCategoryComponent,
    UpdateCategoryComponent,
    ShipmentComponent,
    AddShipmentComponent,
    UpdateShipmentComponent,
    UpdateUserRoleComponent,
    WarehouseComponent,
    AddWarehouseComponent,
    UpdateWarehouseComponent,
    BooleanToShapePipe,
  ],
})
export class UicomponentsModule {}
