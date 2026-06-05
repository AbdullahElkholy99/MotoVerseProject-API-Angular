export interface CustomerDTO{
  id: string
  displayName: string
  imagePath: string
  phone: string
  email: string
  address: string
  isActive: boolean
  orderCount: number
  rentalCount: number
}
export interface GetCustomerByIdResponse{
  id: string
  displayName: string
  imagePath: string
  phone: string
  email: string
  address: string
  isActive: boolean
  orderCount: number
  rentalCount: number
  createdAt: Date
}

export interface GetCustomerPaginatedListQueryDTO{
    pageNumber:number;
    pageSize:number;
    orderBy:CustomerOrderingEnum;
    search?:string;
}
export enum CustomerOrderingEnum
{
    ID = 0,
    Name = 1,
}
