export class LoginResponse{
    constructor(
        public email: string,
        public token: string,
        public role: string,
        public id: number,
    ) {
    }
}
