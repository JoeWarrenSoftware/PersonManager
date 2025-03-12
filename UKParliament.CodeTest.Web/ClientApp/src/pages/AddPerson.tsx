import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { personValidationSchema } from "../validation/personValidation";
import { apiService } from "../services/apiService";
import { Person } from "../models/person";
import { useNavigate } from "react-router-dom";
import { Container, Form, Button } from "react-bootstrap";
import ErrorMessage from "../components/ErrorMessage";

const AddPerson: React.FC = () => {
  const navigate = useNavigate();
  const [departments, setDepartments] = useState<{ id: number; name: string }[]>([]);

  useEffect(() => {
    apiService.getDepartments().then(setDepartments);
  }, []);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Person>({
    resolver: yupResolver<Person>(personValidationSchema),
  });

  const onSubmit = async (data: Person) => {
    try {
      await apiService.createPerson(data);
      alert("Person added successfully!");
      navigate("/people");
    } catch (error) {
      console.error("Error adding person:", error);
      alert("Failed to add person.");
    }
  };

  return (
    <Container className="mt-4">
      <h2 className="text-center mb-4">Add Person</h2>
      <Form onSubmit={handleSubmit(onSubmit)} className="mx-auto" style={{ maxWidth: "600px" }}>
      <Form.Group className="mb-3">
          <Form.Label htmlFor="firstName">First Name</Form.Label>
          <Form.Control id="firstName" type="text" {...register("firstName")} />
          <ErrorMessage message={errors.firstName?.message} />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="lastName">Last Name</Form.Label>
          <Form.Control id="lastName" type="text" {...register("lastName")} />
          <ErrorMessage message={errors.lastName?.message} />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="dateOfBirth">Date of Birth</Form.Label>
          <Form.Control id="dateOfBirth" type="date" {...register("dateOfBirth")} />
          <ErrorMessage message={errors.dateOfBirth?.message} />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="departmentId">Department</Form.Label>
          <Form.Select id="departmentId" {...register("departmentId")}>
            <option value="">Select Department...</option>
            {departments.map((dept) => (
              <option key={dept.id} value={dept.id}>
                {dept.name}
              </option>
            ))}
          </Form.Select>
          <ErrorMessage message={errors.departmentId?.message} />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="email">Email</Form.Label>
          <Form.Control id="email" type="email" {...register("email")} />
          <ErrorMessage message={errors.email?.message} />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="phoneNumber">Phone Number</Form.Label>
          <Form.Control id="phoneNumber" type="tel" {...register("phoneNumber")} />
          <ErrorMessage message={errors.phoneNumber?.message} />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="profileImageUrl">Profile Image URL</Form.Label>
          <Form.Control id="profileImageUrl" type="text" {...register("profileImageUrl")} />
          <ErrorMessage message={errors.profileImageUrl?.message} />
        </Form.Group>

        <Button variant="primary" type="submit" className="mx-auto d-block">
          Add Person
        </Button>
      </Form>
    </Container>
  );
};

export default AddPerson;