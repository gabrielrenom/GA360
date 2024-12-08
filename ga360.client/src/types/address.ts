export interface Address {
    id: number;
    street: string;
    number: string;
    postcode: string;
    city: string;
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modyfiedBy: string;
    createdAt: string;
    modifiedAt: string;
    tenantId: number | null;
  }
