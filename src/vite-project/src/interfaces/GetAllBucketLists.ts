export default interface GetAllBucketLists {
    items: BucketListModel[];
    fullCount: number;
}

export interface BucketListModel {
    id: number;
    friendlyName: string;
    location: string;
    userDisplayName: string;
    plannedEventId: number | null;
}