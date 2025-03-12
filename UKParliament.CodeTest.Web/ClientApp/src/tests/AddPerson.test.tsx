import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { describe, it, expect, vi, beforeEach } from "vitest";
import "@testing-library/jest-dom";
import { BrowserRouter } from "react-router-dom";
import AddPerson from "../pages/AddPerson";
import { apiService } from "../services/apiService";

vi.mock("../services/apiService", () => ({
  apiService: {
    createPerson: vi.fn(),
    getDepartments: vi.fn(),
  },
}));

describe("AddPerson Component", () => {
  beforeEach(() => {
    vi.resetAllMocks();

    (apiService.getDepartments as jest.Mock).mockResolvedValue(
      Promise.resolve([
        { id: 1, name: "HR" },
        { id: 2, name: "IT" },
      ])
    );
  });

  it("renders form and allows submission", async () => {
    render(
      <BrowserRouter>
        <AddPerson />
      </BrowserRouter>
    );

    // Wait for departments to load
    await waitFor(() => expect(screen.queryByText("Loading...")).not.toBeInTheDocument());

    // Fill in form fields
    fireEvent.change(screen.getByLabelText("First Name"), { target: { value: "Alice" } });
    fireEvent.change(screen.getByLabelText("Last Name"), { target: { value: "Johnson" } });
    fireEvent.change(screen.getByLabelText("Email"), { target: { value: "alice@example.com" } });
    fireEvent.change(screen.getByLabelText("Phone Number"), { target: { value: "1234567890" } });
    fireEvent.change(screen.getByLabelText("Date of Birth"), { target: { value: "1995-05-15" } });

    // Select a department
    fireEvent.change(screen.getByLabelText("Department"), { target: { value: "1" } });

    // Submit form
    fireEvent.click(screen.getByRole("button", { name: "Add Person" }));

    // Ensure API call was made
    await waitFor(() => expect(apiService.createPerson).toHaveBeenCalled());
  });
});
