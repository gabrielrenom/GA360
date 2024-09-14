
import { Address } from "./address";

 export interface TrainingCentre {
    id: number;
    name: string;
    addressId: number;
    address: Address;
    documentTrainingCentres: any[] | null;
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modyfiedBy: string;
    createdAt: string;
    modifiedAt: string;
    tenantId: number | null;
  }
  