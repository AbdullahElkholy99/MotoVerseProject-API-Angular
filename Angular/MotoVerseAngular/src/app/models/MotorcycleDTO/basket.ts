import {v4 as uuidv4} from "uuid"

export interface IBasket {
  id: string
  basketItems: IBasketItem[]
}

export interface IBasketItem {
  id: string
  name: string
  imagePath: string
  quantity: number
  price: number
  categoryId: string
}

