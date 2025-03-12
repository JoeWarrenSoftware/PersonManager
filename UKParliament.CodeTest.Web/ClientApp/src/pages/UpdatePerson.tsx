import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { personValidationSchema } from "../validation/personValidation";
import { apiService } from "../services/apiService";
import { Person } from "../models/person";
import { useNavigate, useParams } from "react-router-dom";
import { Container, Form, Button } from "react-bootstrap";
import ErrorMessage from "../components/ErrorMessage";

const UpdatePerson: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [departments, setDepartments] = useState<{ id: number; name: string }[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<Person>({
    resolver: yupResolver<Person>(personValidationSchema),
  });

  useEffect(() => {
    // Fetch departments for dropdown
    apiService
      .getDepartments()
      .then(setDepartments)
      .catch(() => setError("Failed to load departments."));
  
    // Fetch person details for editing
    if (id) {
      apiService
        .getPersonById(parseInt(id))
        .then((data) => {
          if (data.dateOfBirth) {
            data.dateOfBirth = new Date(data.dateOfBirth).toISOString().split("T")[0];
          }
          
          Object.keys(data).forEach((key) =>
            setValue(key as keyof Person, data[key])
          );
          setLoading(false);
        })
        .catch(() => {
          setError("Failed to load person details.");
          setLoading(false);
        });
    }
  }, [id, setValue]);

  const onSubmit = async (data: Person) => {
    try {
      await apiService.updatePerson(parseInt(id!), data);
      alert("Person updated successfully!");
      navigate("/people");
    } catch {
      setError("Failed to update person.");
    }
  };

  if (loading) return <p className="text-center mt-4">Loading...</p>;

  return (
    <Container className="mt-4">
      <h2 className="text-center mb-4">Update Person</h2>
      {error && <ErrorMessage message={error} />}
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
          Update Person
        </Button>
      </Form>
    </Container>
  );
};

export default UpdatePerson;
