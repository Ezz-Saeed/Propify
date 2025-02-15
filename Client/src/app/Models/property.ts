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
  photos?:IPhoto[]
  imageUrl?:string

}

export class IGetPropery implements IProperty{
  photos?: IPhoto[] | undefined;
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
