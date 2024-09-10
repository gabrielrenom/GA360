import { Gender } from "config";
import { CustomerList } from "./customer";

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