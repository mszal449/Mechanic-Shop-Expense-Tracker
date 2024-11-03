import { apiUrl } from "@/const";
import { Job } from "@/types/apiTypes";
import axios from "axios";

interface JobResult {
  jobs: Job[];
  totalCount: number;
}

export const getJob = async (id : number) => {
  const response = await axios.get<Job>(`${apiUrl}/job/${id}`)
  return response.data;
}


export const getAllJobs = async (filters: any): Promise<JobResult> => {
  const response = await axios.get<JobResult>(`${apiUrl}/job`, {
    params: filters,
  });
  return response.data;
};