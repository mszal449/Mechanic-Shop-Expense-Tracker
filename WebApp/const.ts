export const apiUrl = "https://localhost:5005/api"

export const JOB_STATUS: { [key: string]: { text: string; color: string } } = {
  "0": { text: "Pending", color: "text-orange-500" },
  "1": { text: "In Progress", color: "text-blue-500" },
  "2": { text: "Completed", color: "text-green-500" },
  "3": { text: "Cancelled", color: "text-red-500" },
};