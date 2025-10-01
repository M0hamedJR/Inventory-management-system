import { Routes } from '@angular/router';
import { AppSideLoginComponent } from './login/login.component';
import { AddUserComponent } from '../ui-components/users/add-user/add-user.component';

export const AuthenticationRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'login',
        component: AppSideLoginComponent,
      },

      {
        path: 'register-user',
        component: AddUserComponent,
      },
    ],
  },
];
