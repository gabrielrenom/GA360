import { Gender } from "config";
import { CustomerList, CustomerListExtended } from "./customer";

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
}

export const mapCustomerApiModelToCustomerList = (user: CustomerApiModel): CustomerList => {
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

export const mapCustomerListToCustomerApiModel = (user: CustomerList): CustomerApiModel =>{
 return {
    id: user.id,
    avatar: user.avatar,
    firstName: user.firstName,
    lastName: user.lastName,
    fatherName: user.fatherName,
    name: user.name,
    email: user.email,
    age: user.age,
    gender: "Gender.MALE",
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
    date:""
    }   
}

export const mapCustomerListToCustomerApiModelExtended = (user: CustomerListExtended): CustomerApiModelExtended =>{
    return {
       id: user.id,
       avatar: user.avatar,
       firstName: user.firstName,
       lastName: user.lastName,
       fatherName: user.fatherName,
       name: user.name,
       email: user.email,
       age: user.age,
       gender: "Gender.MALE",
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
       date:"",
       avatarImage: user.avatarImage || null, // Default to null if undefined
       dateOfBirth: user.dateOfBirth || "",
       ethnicity: user.ethnicity || "",
       disability: user.disability || "",
       employeeStatus: user.employeeStatus || "",
       employer: user.employer || "",
       trainingCentre: user.trainingCentre || "",
       nationalInsurance: user.nationalInsurance || "",
       portfolio: user.portfolio || "",
       dob: user.dob || "",
       street: user.street || "",
       city: user.city || "",
       number: user.number || "",
       postcode: user.postcode || ""
       }   
   }