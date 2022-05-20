import { HubConnection, IStreamResult } from "@microsoft/signalr"

export class ChatHub {
    constructor(private connection: HubConnection) {
    }

    send(message: string): Promise<void> {
        return this.connection.invoke('Send', message);
    }

    addPerson(person: Person): Promise<void> {
        return this.connection.invoke('AddPerson', person);
    }

    getEvents(): IStreamResult<Event> {
        return this.connection.stream('GetEvents');
    }

    registerCallbacks(implementation: IChatHubCallbacks) {
        this.connection.on('Welcome', () => implementation.welcome());
        this.connection.on('Send', (message) => implementation.send(message));
    }

    unregisterCallbacks(implementation: IChatHubCallbacks) {
        this.connection.off('Welcome', () => implementation.welcome());
        this.connection.off('Send', (message) => implementation.send(message));
    }
}

export interface IChatHubCallbacks {
    welcome(): void;
    send(message: string): void;
}

export interface Person {
    firstName: string | undefined;
    lastName: string | undefined;
}

export interface Event {
    type: string | undefined;
}