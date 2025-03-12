import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { describe, it, expect, vi, beforeEach } from "vitest";
import "@testing-library/jest-dom";
import { BrowserRouter } from "react-router-dom";
import UpdatePerson from "../pages/UpdatePerson";
import { apiService } from "../services/apiService";

vi.mock("../services/apiService", () => ({
  apiService: {
    updatePerson: vi.fn(),
    getPersonById: vi.fn(),
    getDepartments: vi.fn(),
  },
}));

describe("UpdatePerson Component", () => {
  beforeEach(() => {
    vi.resetAllMocks();

    (apiService.getPersonById as jest.Mock).mockResolvedValue(
      Promise.resolve({
        id: 1,
        firstName: "John",
        lastName: "Doe",
        email: "john@example.com",
        dateOfBirth: "1990-01-01",
        departmentId: 1,
      })
    );

    (apiService.getDepartments as jest.Mock).mockResolvedValue(
      Promise.resolve([{ id: 1, name: "HR" }])
    );
  });

  it("renders form and allows submission", async () => {
    render(
      <BrowserRouter>
        <UpdatePerson />
      </BrowserRouter>
    );

    // Wait for API data to load
    await waitFor(() => expect(screen.queryByText("Loading...")).not.toBeInTheDocument());

    // Change a field and submit
    fireEvent.change(screen.getByLabelText("First Name"), { target: { value: "Johnny" } });
    fireEvent.click(screen.getByRole("button", { name: "Update Person" }));

    // Ensure API call was made
    await waitFor(() => expect(apiService.updatePerson).toHaveBeenCalled());
  });
});
