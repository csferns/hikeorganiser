import { DateType } from "../enums/DateType.ts";

export default interface ScheduleRequest {
    Title: string;
    Description: string | undefined;
    Location: string | undefined;
    StartDate: Date;
    EndDate: Date | undefined;
    DateType: DateType;
}