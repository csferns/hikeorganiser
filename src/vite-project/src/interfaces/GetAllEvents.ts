import { Dayjs } from "dayjs";

export default interface GetAllEvents {
    items: Event[];
    fullCount: number;
}

export interface Event {
    id: number;
    title: string;
    description: string | undefined;
    location: string | undefined;
    startDate: Dayjs;
    endDate: Dayjs | undefined;
    
    currentUser: AttendeeInformation;
    attendees: AttendeeInformation[]
}

export interface AttendeeInformation {
    name: string;
    status: number;
}