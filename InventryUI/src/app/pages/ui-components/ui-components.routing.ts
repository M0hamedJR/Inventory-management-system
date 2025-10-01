import { Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { RolesComponent } from './roles/roles.component';
import { AddRoleComponent } from './roles/add-role/add-role.component';
import { ShelfComponent } from './shelves/shelf.component';
import { AddShelfComponent } from './shelves/add-shelf/add-shelf.component';
import { UpdateShelfComponent } from './shelves/update-shelf/update-shelf.component';
import { CategoryComponent } from './category/category.component';
import { AddCategoryComponent } from './category/add-category/add-category.component';
import { UpdateCategoryComponent } from './category/update-category/update-category.component';
import { ShipmentComponent } from './product/product.component';
import { AddShipmentComponent } from './product/add-product/add-product.component';
import { UpdateShipmentComponent } from './product/update-product/update-product.component';
import { UpdateRoleComponent } from './roles/update-role/update-role.component';
import { UpdateUserComponent } from './users/update-user/update-user.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { AddWarehouseComponent } from './warehouse/add-warhouse/add-warhouse.component';
import { UpdateWarehouseComponent } from './warehouse/update-warehouse/update-warehouse.component';
export const UiComponentsRoutes: Routes = [
  {
    path: '',
    children: [
      { path: 'warehouse', component: WarehouseComponent },
      { path: 'add-warehouse', component: AddWarehouseComponent },
      { path: 'update-warehouse/:id', component: UpdateWarehouseComponent },

      { path: 'user', component: UsersComponent },
      { path: 'add-user', component: AddUserComponent },
      { path: 'update-user/:id', component: UpdateUserComponent },

      { path: 'roles', component: RolesComponent },
      { path: 'add-role', component: AddRoleComponent },
      { path: 'update-role/:id', component: UpdateRoleComponent },

      { path: 'shelf', component: ShelfComponent },
      { path: 'add-shelf', component: AddShelfComponent },
      { path: 'update-shelf/:id', component: UpdateShelfComponent },

      { path: 'category', component: CategoryComponent },
      { path: 'add-category', component: AddCategoryComponent },
      { path: 'update-category/:id', component: UpdateCategoryComponent },

      { path: 'product', component: ShipmentComponent },
      { path: 'add-product', component: AddShipmentComponent },
      { path: 'update-product/:id', component: UpdateShipmentComponent },
    ],
  },
];
