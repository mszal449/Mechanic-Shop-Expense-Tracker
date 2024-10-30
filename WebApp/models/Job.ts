// Job interface matching backend model
interface Job {
    jobId: number;
    name: string;
    description?: string;
    carModel: string;
    jobStatus: Status;
    supervisor?: string;
    price: number;
    createdAt: Date;
    updatedAt?: Date;
}

// Enum for Job Status
enum Status {
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3,
}

// JobFilters interface for filters state
interface IJobFilters {
    jobId?: number;
    name?: string;
    description?: string;
    carModel?: string;
    jobStatus?: Status;
    supervisor?: string;
    price?: number;
  }