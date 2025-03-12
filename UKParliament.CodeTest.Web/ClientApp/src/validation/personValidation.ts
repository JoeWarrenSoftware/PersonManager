import * as yup from "yup";
import { Person } from "../models/person";

export const personValidationSchema: yup.ObjectSchema<Person> = yup.object({
    id: yup.number().nullable().optional(), // ✅ Fix: Allow null/undefined
    firstName: yup.string().required("First name is required"),
    lastName: yup.string().required("Last name is required"),
    dateOfBirth: yup.string().required("Date of Birth is required"), 
    departmentId: yup.number().required("Department is required"),
    departmentName: yup.string().nullable().optional(), // ✅ Fix: Optional
    email: yup.string().email("Invalid email format").required("Email is required"),
    phoneNumber: yup.string().matches(/^\+?\d{7,15}$/, "Invalid phone number").required("Phone is required"),
    profileImageUrl: yup.string().url("Invalid image URL").nullable().optional(), // ✅ Fix: Allow null
    isActive: yup.boolean().nullable().optional(), // ✅ Fix: Allow null
  });