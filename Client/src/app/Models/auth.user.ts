
export class IAuthUser{
  email!:string
  username!:string
  firstName!: string;
  lastName!: string;
  roles!:string[]
  token!:string
  isAuthenticated!:boolean
  message?:string
  refreshTokenExpiration?:Date
  expiresOn!:Date
}
