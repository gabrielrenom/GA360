import { Gender } from "config";
import { CustomerList, CustomerListExtended } from "./customer";
import { i } from "vite/dist/node/types.d-aGj9QkWt";

export interface CustomerApiModel {
  id?: number;
  firstName: string;
  lastName: string;
  name: string;
  gender: string;
  age: number;
  contact: string;
  email: string;
  country: string;
  location: string;
  fatherName: string;
  role: string;
  about: string;
  orderStatus: string;
  orders: number;
  progress: number;
  status: number;
  skills: string[];
  time: string;
  date: string;
  avatar: number;
}

export interface DocumentFileModel
{
    name: string;
    content: Blob | null;
    url: string;
    blobId: string;
}

export interface CustomerApiModelExtended {
  id?: number;
  firstName: string;
  lastName: string;
  name: string;
  gender: string;
  age: number;
  contact: string;
  email: string;
  country: string;
  location: string;
  fatherName: string;
  role: string;
  about: string;
  orderStatus: string;
  orders: number;
  progress: number;
  status: number;
  skills: string[];
  time: string;
  date: string;
  avatar: number;
  avatarImage: string | null;
  dateOfBirth: string;
  ethnicity: string;
  disability: string;
  employeeStatus: string;
  employer: string;
  trainingCentre: string;
  nationalInsurance: string;
  portfolio: string;
  city: string;
  number: string;
  postcode: string;
  street: string;
  fileDocuments: DocumentFileModel[]
}

export const mapCustomerApiModelToCustomerListExtended = (
  source: CustomerApiModelExtended
): CustomerListExtended => {
    let genderEnum;
    switch (source.gender) {
      case "Male":
        genderEnum = Gender.MALE;
        break;
      case "Female":
        genderEnum = Gender.FEMALE;
        break;
      case "Nonbinary":
        genderEnum = Gender.NONBINARY;
        break;
      case "Prefer not to say":
        genderEnum = Gender.PREFERNOTTOSAY;
        break;
      default:
        genderEnum = Gender.NONBINARY; // Default or handle other cases
    }
  return {
    id: source.id,
    avatar: source.avatar,
    firstName: source.firstName,
    lastName: source.lastName,
    fatherName: source.fatherName,
    name: source.name,
    email: source.email,
    age: source.age,
    gender: genderEnum,
    role: source.role,
    orders: source.orders,
    progress: source.progress,
    status: source.status,
    orderStatus: source.orderStatus,
    contact: source.contact,
    country: source.country,
    location: source.location,
    about: source.about,
    skills: source.skills,
    time: [source.time],
    date: source.date,
    avatarImage: source.avatarImage,
    dateOfBirth: source.dateOfBirth,
    ethnicity: source.ethnicity,
    disability: source.disability,
    employeeStatus: source.employeeStatus,
    employer: source.employer,
    trainingCentre: source.trainingCentre,
    nationalInsurance: source.nationalInsurance,
    portfolio: source.portfolio,
    dob: source.dateOfBirth,
    street: source.street,
    city: source.city,
    number: source.number,
    postcode: source.postcode,
    documents: [],
    fileDocuments: source.fileDocuments
  };
};

export const mapCustomerApiModelToCustomerList = (
  user: CustomerApiModel
): CustomerList => {
  return {
    id: user.id,
    avatar: user.avatar,
    firstName: user.firstName,
    lastName: user.lastName,
    fatherName: user.fatherName,
    name: user.name,
    email: user.email,
    age: user.age,
    gender: Gender.MALE,
    role: user.role,
    orders: user.orders,
    progress: user.progress,
    status: user.status,
    orderStatus: user.orderStatus,
    contact: user.contact,
    country: user.country,
    location: user.location,
    about: user.about,
    skills: ["JavaScript", "React"],
    time: [""],
    date: new Date(user.date),
  };
};

export const mapCustomerListToCustomerApiModel = (
  user: CustomerList
): CustomerApiModel => {
  
  return {
    id: user.id,
    avatar: user.avatar,
    firstName: user.firstName,
    lastName: user.lastName,
    fatherName: user.fatherName,
    name: user.name,
    email: user.email,
    age: user.age,
    gender: user.gender,
    role: user.role,
    orders: user.orders,
    progress: user.progress,
    status: user.status,
    orderStatus: user.orderStatus,
    contact: user.contact,
    country: user.country,
    location: user.location,
    about: user.about,
    skills: ["JavaScript", "React"],
    time: "",
    date: "",
  };
};

export const mapCustomerListToCustomerApiModelExtended = (
  user: CustomerListExtended
): CustomerApiModelExtended => {
  let genderEnum;
  switch (user.gender.toString()) {
    case "Male":
      genderEnum = Gender.MALE;
      break;
    case "Female":
      genderEnum = Gender.FEMALE;
      break;
    case "Nonbinary":
      genderEnum = Gender.NONBINARY;
      break;
    case "Prefer not to say":
      genderEnum = Gender.PREFERNOTTOSAY;
      break;
    default:
      genderEnum = Gender.NONBINARY; // Default or handle other cases
  }
  return {
    id: user.id,
    avatar: user.avatar,
    firstName: user.firstName,
    lastName: user.lastName,
    fatherName: user.fatherName,
    name: user.name,
    email: user.email,
    age: user.age,
    gender: genderEnum,
    role: user.role,
    orders: user.orders,
    progress: user.progress,
    status: user.status,
    orderStatus: user.orderStatus,
    contact: user.contact,
    country: user.country,
    location: user.location,
    about: user.about,
    skills: user.skills,
    time: "",
    date: "",
    avatarImage: user.avatarImage || null, // Default to null if undefined
    dateOfBirth: user.dateOfBirth || "",
    ethnicity: user.ethnicity || "",
    disability: user.disability || "",
    employeeStatus: user.employeeStatus || "",
    employer: user.employer || "",
    trainingCentre: user.trainingCentre || "",
    nationalInsurance: user.nationalInsurance || "",
    portfolio: user.portfolio || "",
    street: user.street || "",
    city: user.city || "",
    number: user.number || "",
    postcode: user.postcode || "",
  };
};
