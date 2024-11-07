import { apiUrl } from "@/const";
import { Job } from "@/types/apiTypes";
import axios from "axios";

interface JobResult {
  jobs: Job[];
  totalCount: number;
}

// Ignore SSL for development
const api = axios.create({
  baseURL: `${apiUrl}`,
  httpsAgent: new (require('https').Agent)({ rejectUnauthorized: false })
});


export const getJobAsync = async (id : number) => {
  const response = await api.get<Job>(`${apiUrl}/job/${id}`)
  return response.data;
}


export const getAllJobsAsync = async (filters: any): Promise<JobResult> => {
  const response = await api.get<JobResult>(`${apiUrl}/job`, {
    params: filters,
  });
  return response.data;
};


export const GetJobsStatisticsAsync = async () => {
  const response = await api.get(`${apiUrl}/job/statistics`);
  return response.data;
}