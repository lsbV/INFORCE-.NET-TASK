export class EntireUrlInfo{
  constructor(
    public originalUrl: string,
    public shortenedUrl: string,
    public expiration: Date,
    public visits: number,
    public createdBy: number,
    public createdAt: Date,
    public creatorEmail: string,
  ) {
  }
}
