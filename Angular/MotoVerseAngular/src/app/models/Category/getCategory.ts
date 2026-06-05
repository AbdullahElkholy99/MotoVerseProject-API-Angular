export interface GetCategoryDTO{
    id:string;
    name: string;
    description: string;
    imagePath?: string;
    adminId: string;
    productCount?:number
}

export interface GetCategoryPaginatedListQueryDTO{
    pageNumber:number;
    pageSize:number;
    orderBy:CategoryOrderingEnum;
    search?:string;
}
export enum CategoryOrderingEnum
{
    ID = 0,
    Name = 1,
}
