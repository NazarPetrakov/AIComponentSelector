import { HttpParams } from '@angular/common/http';
import { ChatRequestQueryParams } from '../_models/queryParams/charRequestQueryParams';

export function setChatBuildHeaders(chatParams: ChatRequestQueryParams) {
  let params = new HttpParams();

  if (chatParams.price) {
    params = params.append('price', chatParams.price);
  }
  if (chatParams.description) {
    params = params.append('description', chatParams.description);
  }
  if (chatParams.purpose) {
    params = params.append('purpose', chatParams.purpose);
  }
  if (chatParams.lang) {
    params = params.append('lang', chatParams.lang);
  }

  return params;
}
