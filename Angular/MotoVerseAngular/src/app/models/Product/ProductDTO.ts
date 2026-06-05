import { ReviewDTO } from "./reviews/reviewDto"

export interface ProductDTO {
  id: string
  name: string
  price: number
  imagePath: string
  categoryName: any
  rating: number
  quantity: number
  description: string
  status: ProductStatus
  categoryId:string
  reviews:ReviewDTO[]
}

export enum ProductStatus
{
    NoStatus = 0,
    pending = 1,
    Active = 2,
    OutOfStock = 3,
    Discontinued = 4,
    Deleted = 5
}

export interface GetProductPaginatedListQueryDTO {
  pageNumber: number;
  pageSize: number;
  orderBy: ProductOrderingEnum;
  search?: string;
  categoryId :string

}
export enum ProductOrderingEnum {
  ID = 0,
  Name = 1,
  Quantity = 2,
  Price = 3,
  DepartmentName = 4,
}



export interface ProductRecommendationsDTO {
  id: string
  name: string
  price: number
  imagePath: string
}
