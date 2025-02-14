import { Gender } from 'config';
import { extend } from 'lodash';
import { CertificateModel, CourseModel, DocumentFileModel, QualificationModel } from './customerApiModel';

export interface CustomerProps {
  modal: boolean;
}

export interface CustomerList {
  id?: number;
  avatar: number;
  firstName: string;
  lastName: string;
  fatherName: string;
  name: string;
  email: string;
  age: number;
  gender: Gender;
  role: string;
  orders: number;
  progress: number;
  status: number;
  orderStatus: string;
  contact: string;
  country: string;
  location: string;
  about: string;
  skills: string[];
  time: string[];
  date: Date | string | number;
}

export interface CustomerListExtended extends CustomerList {
  avatarImage: string | null;
  dateOfBirth: string;
  ethnicity: string;
  disability: string;
  employeeStatus: string;
  employer: string;
  trainingCentre: number;
  nationalInsurance: string;
  portfolio: string;
  dob: string;
  street: string;
  city: string;
  number: string;
  postcode: string;
  documents: string[];
  fileDocuments: DocumentFileModel[];
  courses: CourseModel [];
  qualifications: QualificationModel [];
  certificates: CertificateModel [];
  trainingCentreId?: number; // Make this property optional
  employmentStatus: string;
}

export interface BasicCustomer {
  id: number;
  email: string;
}

export interface User {
  firstName: string;
  lastName: string;
  email: string;
  customerId: number;
  role: string;
  roleId: number;
  trainingCentreId: number;
  contact: string;
  city: string;
  avatarImage: string;
  employeeStatus: string;
}