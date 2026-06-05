import { MotorcycleStatus } from '../../Enums/motorcycle-status.enum';
import { Booking } from './booking.interface';
import { Favorite } from './favorite.interface';
import { MotorcycleImage } from './motorcycle-image.interface';
import { ReviewMotorCycle } from './review-motorcycle.interface';

export interface MotorcycleDTO {
  id: string;
  nameAr: string;
  nameEn: string;

  brand: string;

  model: string;

  year: number;

  color?: string;

  plateNumber?: string;

  engineCC: number;

  pricePerDay: number;

  description?: string;

  imagePath?: string;
  imagesPath?: string[];

  status: MotorcycleStatus;

  createdAt: Date;

  ownerId: string;

  owner?: string;

  images?: MotorcycleImage[];

  bookings?: Booking[];

  favorites?: Favorite[];

  reviewMotorCycles?: ReviewMotorCycle[];
}

export interface MotorcyclePaginatedQueryDTO {
  pageNumber: number;
  pageSize: number;
  search?: string;
  brand?: string;
  model?: string;
  status?: MotorcycleStatus ;
  minPrice?: number;
  maxPrice?: number;
}



export interface AddMotorcycleDTO {

  nameAr: string;
  nameEn: string;
  brand: string;

  model: string;

  year: number;

  color?: string;

  plateNumber?: string;

  engineCC: number;

  pricePerDay: number;

  description?: string;

 ImageFile?: File;

  Images?: File[];

  status: MotorcycleStatus;

  createdAt: Date;

  ownerId: string;

  owner?: string;


}
