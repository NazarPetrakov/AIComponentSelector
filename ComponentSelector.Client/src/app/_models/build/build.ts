import { ComputerComponent } from '../computerComponent';
import { Compatibility } from './compatibility';

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
  chatBuildResponse: ChatBuildResponse;
}
export interface ChatBuildResponse {
  build: ChatBuild;
  totalPrice: number;
  compatibility: Compatibility;
}
interface ChatBuild {
  cpu: ComponentWithCharacteristics;
  motherboard: ComponentWithCharacteristics;
  ram: ComponentWithCharacteristics;
  storage: ComponentWithCharacteristics;
  gpu: ComponentWithCharacteristics;
  psu: ComponentWithCharacteristics;
  case: ComponentWithCharacteristics;
}
interface ComponentWithCharacteristics {
  category: string;
  title: string;
  price: number;
  availability: string;
  link: string;
  imageUrl: string;
  reviews: number;
  characteristics: Characteristic[];
}
interface Characteristic {
  attributeName: string;
  attributeValue: string;
}
