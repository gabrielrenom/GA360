
import { Address } from "./address";
import { DocumentFileModel } from "./customerApiModel";

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
  export interface TrainingCentreList {
    id: number;
    name: string;
    addressId: number;
    address: {
      id: 1;
      street: "Battersbay Grove";
      number: 16;
      postcode: "SK74QW";
      city: "MAnchester";
      isDeleted: false;
      deleterUserId: null;
      deletionTime: null;
      createdBy: 1;
      modyfiedBy: 1;
      createdAt: "2024-04-04";
      modifiedAt: "2024-04-04";
      tenantId: 1;
    };
    documentTrainingCentres: [];
    documents: string[];
    fileDocuments: DocumentFileModel[];
  }
  