import { Customer } from '../customer';
import { MotorcycleDTO } from './motorcycle.interface';

export interface ReviewMotorCycle {
  id: string;

  rating: number;

  comment?: string;

  createdAt: Date;

  customerId: string;

  customer?: Customer;

  motorcycleId: string;

  motorcycle?: MotorcycleDTO;
}
