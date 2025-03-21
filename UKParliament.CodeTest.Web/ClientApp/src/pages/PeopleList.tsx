﻿import React, { useEffect, useState } from "react";
import { apiService } from "../services/apiService.ts";
import { Person } from "../models/person.ts";
import { Container, Card, Button, Row, Col } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const PeopleList: React.FC = () => {
    const [people, setPeople] = useState<Person[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
  
    useEffect(() => {
        fetchPeople();
    }, []);

    const fetchPeople = async () => {
        try {
            const data = await apiService.getPeople();
            setPeople(data);
        } catch (err) {
            console.error("Error fetching people:", err);
            setError("Failed to load people.");
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id: number) => {
        if (window.confirm("Are you sure you want to delete this person?")) {
          try {
            await apiService.deletePerson(id);
            setPeople(people.filter((p) => p.id !== id));
          } catch (err) {
            console.error("Error deleting person:", err);
            setError("Failed to delete person.");
          }
        }
    };

    if (loading) return <p className="text-center mt-4">Loading people...</p>;
    if (error) return <p className="text-center text-danger">{error}</p>;

  return (
    <Container className="mt-4">
      <h2 className="text-center w-100 fw-bold mb-4">People List</h2>
      <Row className="justify-content-center g-4">
        {people.map((person) => (
          <Col key={person.id} xs={12} sm={6} md={4} lg={3} className="d-flex">
            <Card className="shadow-sm text-center flex-fill" style={{ maxWidth: "300px", minWidth: "270px", height: "100%" }}>
              <Card.Img
                variant="top"
                src={person.profileImageUrl ?? "https://i.pravatar.cc/300?u=2@site.com"}
                alt={`${person.firstName} ${person.lastName}`}
                style={{ height: "200px", objectFit: "cover" }}
              />
              <Card.Body className="d-flex flex-column">
                <Card.Title className="fw-bold">
                  {person.firstName} {person.lastName}
                </Card.Title>
                <Card.Text className="text-muted small text-start">
                  <strong>📅 Date of Birth: </strong><span>{new Date(person.dateOfBirth).toLocaleDateString()}</span> <br />
                  <strong>🏢 Department: </strong><span>{person.departmentName}</span> <br />
                  <strong>📧 Email: </strong><span>{person.email}</span> <br />
                  <strong>📞 Phone: </strong><span>{person.phoneNumber}</span>
                </Card.Text>
                <div className="mt-auto d-flex justify-content-between gap-2">
                  <Button
                    variant="primary"
                    className="flex-grow-1"
                    onClick={() => navigate(`/update-person/${person.id}`)}
                  >
                    Edit
                  </Button>
                  <Button
                    variant="danger"
                    className="flex-grow-1"
                    onClick={() => handleDelete(person.id!)}
                  >
                    Delete
                  </Button>
                </div>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </Container>
  );
};

export default PeopleList;
