import { apiUrl } from "@/const";
import { Job } from "@/types/apiTypes";

export const getAllJobs = async (pageNumber?: number, pageSize?: number) : Promise<Job[]> => {
  let url = `${apiUrl}/job`
  if (pageNumber && pageSize) {
    url = `${url}?pageNumber=${pageNumber}&pageSize=${pageSize}`
  }
  console.log(url)
  const response = await fetch(url);
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