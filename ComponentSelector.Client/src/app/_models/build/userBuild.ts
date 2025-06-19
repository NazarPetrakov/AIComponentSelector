import { ComputerComponent } from '../computerComponent';

export interface UserBuild {
  id: number;
  cpu: ComputerComponent;
  motherboard: ComputerComponent;
  ram: ComputerComponent;
  storage: ComputerComponent;
  gpu: ComputerComponent;
  psu: ComputerComponent;
  case: ComputerComponent;
  userId: number;
  totalPrice: number;
}
