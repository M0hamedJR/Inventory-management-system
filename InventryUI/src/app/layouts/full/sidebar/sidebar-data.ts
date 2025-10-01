import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    displayName: 'Dashboard',
    iconName: 'layout-dashboard',
    route: '/dashboard',
  },
  {
    displayName: 'Inventory items',
    iconName: 'list',
    children: [
      {
        displayName: 'Shipments',
        iconName: 'devices',
        route: '/ui-components/product',
      },
      {
        displayName: 'Category',
        iconName: 'rosette',
        route: '/ui-components/category',
      },
      {
        displayName: 'Shelves',
        iconName: 'list',
        route: '/ui-components/shelf',
      },

      {
        displayName: 'Warehouse',
        iconName: 'list',
        route: '/ui-components/warehouse',
      },
    ],
  },
  {
    displayName: 'User Management',
    iconName: 'user',
    children: [
      {
        displayName: 'Roles',
        iconName: 'circle-key',
        route: '/ui-components/roles',
      },
      {
        displayName: 'User',
        iconName: 'user-check',
        route: '/ui-components/user',
      },
    ],
  },
];
