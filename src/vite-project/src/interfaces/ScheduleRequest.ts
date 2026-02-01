import { Dayjs } from 'dayjs';

export default interface ScheduleRequest {
    Title: string;
    Description: string | undefined;
    Location: string | undefined;
    StartDate: Dayjs;
    EndDate: Dayjs | undefined;
    BucketListId: number | null;
}