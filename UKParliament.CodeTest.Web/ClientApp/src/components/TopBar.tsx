import React, { useEffect, useState } from "react";
import { Container, Navbar, Button } from "react-bootstrap";

const TopBar: React.FC = () => {
  const [isDarkMode, setIsDarkMode] = useState<boolean>(() => {
    return localStorage.getItem("theme") !== "light";
  });

  useEffect(() => {
    if (isDarkMode) {
      document.documentElement.classList.add("dark-mode"); // Ensure it applies to HTML root
      document.body.classList.add("dark-mode");
      localStorage.setItem("theme", "dark");
    } else {
      document.documentElement.classList.remove("dark-mode");
      document.body.classList.remove("dark-mode");
      localStorage.setItem("theme", "light");
    }
  }, [isDarkMode]);

  return (
    <Navbar className="topbar py-2">
      <Container className="d-flex justify-content-between align-items-center">
        <span>{new Date().toLocaleDateString()}</span>
        <h5 className="text-center flex-grow-1">Person Manager App</h5>
        <Button
          variant={isDarkMode ? "light" : "dark"}
          onClick={() => setIsDarkMode(!isDarkMode)}
        >
          {isDarkMode ? "Light Mode ☀️" : "Dark Mode 🌙"}
        </Button>
      </Container>
    </Navbar>
  );
};

export default TopBar;
