import React from "react";
import { Navbar, Nav, Container } from "react-bootstrap";
import { Link } from "react-router-dom";

const NavBar: React.FC = () => {
  return (
    <Navbar style={{ padding: "8px 0" }}>
      <Container className="justify-content-center">
        <Nav>
          <Nav.Link as={Link} to="/add-person" className="text-white mx-3" style={{ fontWeight: "bold" }}>
            Add Person
          </Nav.Link>
          <Nav.Link as={Link} to="/people" className="text-white mx-3" style={{ fontWeight: "bold" }}>
            Show All People
          </Nav.Link>
        </Nav>
      </Container>
    </Navbar>
  );
};

export default NavBar;
