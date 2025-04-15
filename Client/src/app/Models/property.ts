import { IPhoto } from "./photo";

export interface IProperty {
  description: string;
  address: string;
  city: string;
  price: number;
  bedRooms?: number ;
  bathRooms?: number;
  area: number;
  isAvailable: boolean;
  isRental: boolean;
  images?:IPhoto[]
  imageUrl?:string
  id:number
  typeId:number
}

export class IGetPropery implements IProperty{
  id!:number
  typeId!:number
  images!: IPhoto[];
  imageUrl?: string;
  description!: string;
  address!: string;
  city!: string;
  price!: number;
  bedRooms?: number | undefined;
  bathRooms?: number | undefined;
  area!: number;
  isAvailable!: boolean;
  isRental!: boolean;
  typeName!: string;
  categoryName!: string;
  ownerName!: string;
}

export class Pagination{
  pageNumber!:number
  pageSize!:number;
  totalCount!:number;
}

export class PaginatedResult{
  // pageNumber!:number
  // pageSize!:number;
  // totalCount!:number;
  data!:IGetPropery[]
  pagination!:Pagination
}
