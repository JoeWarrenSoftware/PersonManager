import { render, screen, waitFor } from "@testing-library/react";
import { describe, it, expect, vi } from "vitest";
import PeopleList from "../pages/PeopleList";
import { apiService } from "../services/apiService";
import { BrowserRouter } from "react-router-dom";
import "@testing-library/jest-dom";

// Mock the API service
vi.mock("../services/apiService", () => ({
  apiService: {
    getPeople: vi.fn(),
  },
}));

describe("PeopleList Component", () => {
  it("renders people list correctly", async () => {
    // Mock API response data
    const mockPeople = [
      {
        id: 1,
        firstName: "John",
        lastName: "Doe",
        dateOfBirth: "1990-01-01",
        departmentId: 1,
        departmentName: "HR",
        email: "john@example.com",
        phoneNumber: "1234567890",
        profileImageUrl: "https://example.com/image.jpg",
      },
    ];

    // Spy on `apiService.getPeople` and mock the return value
    vi.spyOn(apiService, "getPeople").mockResolvedValueOnce(mockPeople);

    // Render component inside Router since it has links
    render(
      <BrowserRouter>
        <PeopleList />
      </BrowserRouter>
    );

    // Wait for the mock data to be displayed
    await waitFor(() => {
      expect(screen.getByText("John Doe")).toBeInTheDocument();
    });
  });
});
