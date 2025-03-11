import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import TopBar from "./components/TopBar";
import NavBar from "./components/NavBar";
import Footer from "./components/Footer";
import PeopleList from "./pages/PeopleList";
import AddPerson from "./pages/AddPerson";
import UpdatePerson from "./pages/UpdatePerson";
import { Container } from "react-bootstrap";

const App: React.FC = () => {
  return (
    <Router>
      <TopBar />
      <NavBar />
      <Container className="main-content">
        <Routes>
          <Route path="/" element={<h1>Welcome to the App</h1>} />
          <Route path="/people" element={<PeopleList />} />
          <Route path="/add-person" element={<AddPerson />} />
          <Route path="/update-person/:id" element={<UpdatePerson />} />
        </Routes>
      </Container>
      <Footer />
    </Router>
  );
};

export default App;
