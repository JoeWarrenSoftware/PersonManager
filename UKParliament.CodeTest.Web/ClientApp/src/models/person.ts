export interface Person {
    id?: number | null;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    departmentId: number;
    departmentName?: string;
    email: string;
    phoneNumber: string;
    profileImageUrl?: string | null;
    isActive?: boolean;
}