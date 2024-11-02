export const apiUrl = "https://localhost:5005/api"

export const JOB_STATUS: { [key: string]: { text: string; color: string } } = {
  "0": { text: "Pending", color: "orange" },
  "1": { text: "In Progress", color: "blue" },
  "2": { text: "Completed", color: "green" },
  "3": { text: "Cancelled", color: "red" },
};