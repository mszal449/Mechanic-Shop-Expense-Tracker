import { apiUrl } from "@/const";
import { Job } from "@/types/apiTypes";
import axios from "axios";

interface JobResult {
  jobs: Job[];
  totalCount: number;
}

export const getAllJobs = async (filters: any): Promise<JobResult> => {
  const response = await axios.get<JobResult>(`${apiUrl}/job`, {
    params: filters,
  });
  return response.data;
};