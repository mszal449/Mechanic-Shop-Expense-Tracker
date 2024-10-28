import { apiUrl } from "@/const";

export const getAllJobs = async () => {
  const response = await fetch(`${apiUrl}/job`);
  if (!response.ok) throw new Error("Failed to fetch jobs");
  
  const data = await response.json();
  
  // Convert date strings to Date objects for createdAt and updatedAt
  const jobsData = data.map((job: any) => ({
    ...job,
    createdAt: new Date(job.createdAt),
    updatedAt: job.updatedAt ? new Date(job.updatedAt) : null,
  }));

  return jobsData
}