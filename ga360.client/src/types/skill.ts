export interface Skill {
    id: number;
    name: string;
    description: string;
    customerSkills: any | null;
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modyfiedBy: string;
    createdAt: string;
    modifiedAt: string;
    tenantId: number | null;
  }
  