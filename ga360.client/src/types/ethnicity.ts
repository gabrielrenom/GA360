export interface Ethnicity {
    id: number;
    name: string;
    customers: any | null;
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modyfiedBy: string;
    createdAt: string;
    modifiedAt: string;
    tenantId: number | null;
}
  