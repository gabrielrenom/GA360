export interface Country {
    id: number;
    tenantId: number | null;
    name: string;
    code: string;
    prefix: string;
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modyfiedBy: string;
    createdAt: string;
    modifiedAt: string;
}
