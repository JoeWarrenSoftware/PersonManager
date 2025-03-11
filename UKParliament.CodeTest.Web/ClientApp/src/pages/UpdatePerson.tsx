import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate, useParams } from "react-router-dom";
import { Container, Form, Button } from "react-bootstrap";

interface Person {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  departmentId: number;
  email: string;
  phoneNumber: string;
  profileImageUrl: string;
}

interface Department {
  id: number;
  name: string;
}

const UpdatePerson: React.FC = () => {
  const { id } = useParams<{ id?: string }>(); // Get ID from the URL
  const navigate = useNavigate();
  const [person, setPerson] = useState<Person | null>(null);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<{ [key: string]: string }>({});

  // Redirect if no ID is provided
  useEffect(() => {
    if (!id) {
      navigate("/people");
      return;
    }

    // Fetch Person data
    axios
      .get(`https://localhost:7048/api/person/${id}`)
      .then((response) => {
        setPerson(response.data);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching person:", error);
        setError("Failed to load person data.");
        setLoading(false);
      });

    // Fetch Departments data
    axios
      .get("https://localhost:7048/api/department")
      .then((response) => {
        setDepartments(response.data.items);
      })
      .catch((error) => {
        console.error("Error fetching departments:", error);
      });
  }, [id, navigate]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    if (person) {
      setPerson({ ...person, [e.target.name]: e.target.value });
      setValidationErrors({ ...validationErrors, [e.target.name]: "" }); // Clear validation error when typing
    }
  };

  const validateForm = () => {
    const errors: { [key: string]: string } = {};

    if (!person?.email.match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)) {
      errors.email = "Invalid email format";
    }

    if (!person?.phoneNumber.match(/^\+?\d{7,15}$/)) {
      errors.phoneNumber = "Invalid phone number format (7-15 digits)";
    }

    if (!person?.profileImageUrl.match(/^(https?:\/\/.*)$/i)) {
      errors.profileImageUrl = "Invalid image URL (must be a valid image link)";
    }

    setValidationErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (validateForm() && person) {
      axios
        .put(`https://localhost:7048/api/person/${id}`, person)
        .then(() => {
          alert("Person updated successfully!");
          navigate("/people");
        })
        .catch((error) => {
          console.error("Error updating person:", error);
          alert("Failed to update person.");
        });
    }
  };

  if (loading) return <p className="text-center mt-4">Loading person data...</p>;
  if (error) return <p className="text-center mt-4 text-danger">{error}</p>;

  return (
    <Container className="mt-4">
      <h2 className="text-center mb-4">Update Person</h2>
      <Form onSubmit={handleSubmit} className="mx-auto" style={{ maxWidth: "600px" }}>
        
        <Form.Group className="mb-3">
          <Form.Label htmlFor="firstName">First Name</Form.Label>
          <Form.Control
            type="text"
            id="firstName"
            name="firstName"
            value={person?.firstName || ""}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="lastName">Last Name</Form.Label>
          <Form.Control
            type="text"
            id="lastName"
            name="lastName"
            value={person?.lastName || ""}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="dateOfBirth">Date of Birth</Form.Label>
          <Form.Control
            type="date"
            id="dateOfBirth"
            name="dateOfBirth"
            value={person?.dateOfBirth.split("T")[0] || ""}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="departmentId">Department</Form.Label>
          <Form.Select
            id="departmentId"
            name="departmentId"
            value={person?.departmentId || ""}
            onChange={handleChange}
            required
          >
            <option value="">Select a department...</option>
            {departments.map((dept) => (
              <option key={dept.id} value={dept.id}>
                {dept.name}
              </option>
            ))}
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="email">Email</Form.Label>
          <Form.Control
            type="email"
            id="email"
            name="email"
            value={person?.email || ""}
            onChange={handleChange}
            isInvalid={!!validationErrors.email}
            required
          />
          <Form.Control.Feedback type="invalid">{validationErrors.email}</Form.Control.Feedback>
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="phoneNumber">Phone Number</Form.Label>
          <Form.Control
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            value={person?.phoneNumber || ""}
            onChange={handleChange}
            isInvalid={!!validationErrors.phoneNumber}
            required
          />
          <Form.Control.Feedback type="invalid">{validationErrors.phoneNumber}</Form.Control.Feedback>
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label htmlFor="profileImageUrl">Profile Image URL</Form.Label>
          <Form.Control
            type="text"
            id="profileImageUrl"
            name="profileImageUrl"
            value={person?.profileImageUrl || ""}
            onChange={handleChange}
            isInvalid={!!validationErrors.profileImageUrl}
            required
          />
          <Form.Control.Feedback type="invalid">{validationErrors.profileImageUrl}</Form.Control.Feedback>
        </Form.Group>

        <Button variant="primary" type="submit">
          Update Person
        </Button>
      </Form>
    </Container>
  );
};

export default UpdatePerson;
