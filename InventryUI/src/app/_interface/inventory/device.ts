export interface Device {
  id: string;
  name: string;
  price: number;
  in_Out: number;
  serialNumber: string;
  categoryId?: string;
  categoryName: string;
  shelfName: string;
  isFaulty: boolean;
  Weight: number;
  Shipped_From: Date;
  Shipped_To: Date;
  Shipped_Address_From: string;
  Shipped_Address_To: string;
  ShelfId: string;
  WarehouseId: string;
  Warehouse: string;
}

export interface ProductCategoryReport {
  categoryName: string;
  totalProducts: number;
  available: number;
  faulty: number;
}
