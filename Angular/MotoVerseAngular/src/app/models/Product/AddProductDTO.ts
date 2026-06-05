import { ProductStatus } from "./ProductDTO"

export interface AddProductDTO {
  nameAr: string
  nameEn: string
  quantity: number
  price: number
  imageFile: File
  imagePath:string
  categoryId: string
  adminId:string
  description:string
  stock:number
  status:ProductStatus
  rating: number
}

