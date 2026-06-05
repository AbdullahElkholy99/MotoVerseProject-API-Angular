export interface ResponeResult<T>  {
  statusCode: number
  meta: any
  succeeded: boolean
  message: string
  errors: any
  errorsBag: any
  data: T
}

export interface ResponePagedResult<T>  {

  data: T

  currentPage: number,
  totalPages:  number,
  totalCount: number,
  meta: null,
  pageSize:  number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
  messages: any,
  succeeded: boolean



}

export interface AuthResponse {
  accessToken: string;
  refreshToken: RefreshToken;
}

export interface RefreshToken {
  email: string;
  displayName: string;
  tokenString: string;
  expireAt: string;
}


