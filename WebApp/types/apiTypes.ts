export const Status = [
  "Pending",
  "In Progress",
  "Completed",
  "Cancelled"
]

export interface Job {
  jobId: number;
  name: string;
  description?: string;
  carModel: string;
  jobStatus: number;
  supervisor?: string;
  price: number; // decimal(10, 2) precision should be ensured when handling data.
  createdAt: Date;
  updatedAt?: Date;
}
