import { PaginationParams } from './paginationParams';

export class ComponentQueryParams extends PaginationParams {
  category?: string;
  orderBy?: string = 'id';
  orderByDesc?: string;
  minPrice?: number;
  maxPrice?: number;
  availability?: boolean;
  searchTerm?: string;
}
