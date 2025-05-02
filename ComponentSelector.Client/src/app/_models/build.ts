import { Compatibility } from './compatibility';
import { ComputerComponent } from './computerComponent';

export interface Build {
  cpu: ComputerComponent;
  motherboard: ComputerComponent;
  ram: ComputerComponent;
  storage: ComputerComponent;
  gpu: ComputerComponent;
  psu: ComputerComponent;
  case: ComputerComponent;
  totalPrice: number;
  compatibility: Compatibility;
}
