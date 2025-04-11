
export class ProfileImage{
  url!:string
  publicId!:string
}

export class IAuthUser{
  email!:string
  username!:string
  firstName!: string;
  lastName!: string;
  displayName?:string
  roles!:string[]
  token!:string
  isAuthenticated!:boolean
  message:string | null = null
  refreshTokenExpiration?:Date
  expiresOn!:Date
  profileImage?:ProfileImage;
}
