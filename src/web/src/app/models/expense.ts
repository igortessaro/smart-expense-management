export class Expense {
    constructor(description: string, value: number, userUuid: string) {
        this.description = description;
        this.value = value;
        this.userUuid = userUuid;
    }

    id: string = '';
    userUuid: string = '';
    description: string = '';
    value: number = 0;
    createdAt: Date = new Date();
}
