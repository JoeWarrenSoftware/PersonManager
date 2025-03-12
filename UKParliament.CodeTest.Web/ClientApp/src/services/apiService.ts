import axios from "axios";
import { Person } from "../models/person";
import { Department } from "../models/department";

const API_BASE_URL = "https://localhost:7048/api";

export const apiService = {
  getPeople: async (): Promise<Person[]> => {
    const response = await axios.get(`${API_BASE_URL}/person`);
    return response.data.items;
  },

  getPersonById: async (id: number): Promise<Person> => {
    const response = await axios.get(`${API_BASE_URL}/person/${id}`);
    return response.data;
  },

  createPerson: async (person: Person) => {
    await axios.post(`${API_BASE_URL}/person`, person);
  },

  updatePerson: async (id: number, person: Person) => {
    await axios.put(`${API_BASE_URL}/person/${id}`, person);
  },

  deletePerson: async (id: number) => {
    await axios.delete(`${API_BASE_URL}/person/${id}`);
  },

  getDepartments: async (): Promise<Department[]> => {
    const response = await axios.get(`${API_BASE_URL}/department`);
    return response.data.items;
  },
};
