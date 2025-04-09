export class TokenClaims {
    role: string;
    username: string;

    constructor(role: string, username: string) {
        this.role = role;
        this.username = username;
    }
}